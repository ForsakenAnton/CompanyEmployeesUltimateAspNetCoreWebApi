
using Microsoft.AspNetCore.Mvc; // (!) Deprecated
using Service.Contracts;
using Shared.DataTransferObjects;
using CompanyEmployees.Presentation.ModelBinders;
using CompanyEmployees.Presentation.ActionFilters;
using Asp.Versioning;

namespace CompanyEmployees.Presentation.Controllers;

//[Asp.Versioning.ApiVersion("1.0")]
//[Microsoft.AspNetCore.Mvc.ApiVersion("1.0")]
[Route("api/companies")]
[ApiController]
//[ResponseCache(CacheProfileName = "120SecondsDuration")]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    public CompaniesController(IServiceManager service)
    {
        _service = service;
    }


    [HttpGet(Name = "GetCompanies")]
    public async Task<IActionResult> GetCompanies()
    {
        //try
        //{
        //throw new Exception("Exception 0_o");
            var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);

            return Ok(companies);
        //}
        //catch
        //{
        //    return StatusCode(500, "Internal server error");
        //}
    }

    //[HttpGet("{id:guid}")]
    [HttpGet("{id:guid}", Name = "CompanyById")]
    //[ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _service.CompanyService
            .GetCompanyAsync(id, trackChanges: false);

        return Ok(company);
    }


    [HttpPost(Name = "CreateCompany")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
        //if (company is null)
        //{
        //    return BadRequest("CompanyForCreationDto object is null");
        //}

        //if (!ModelState.IsValid)
        //{
        //    return UnprocessableEntity(ModelState);
        //}

        var createdCompany = await _service.CompanyService
            .CreateCompanyAsync(company);

        return CreatedAtRoute(
            routeName: "CompanyById",
            routeValues: new { id = createdCompany.Id },
            value: createdCompany);
    }


    // Companies Collection ///////////////////////////////////////////////
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = await _service.CompanyService
            .GetByIdsAsync(ids, trackChanges: false);

        return Ok(companies);
    }


    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result = await _service.CompanyService
            .CreateCompanyCollectionAsync(companyCollection);

        return CreatedAtRoute(
            "CompanyCollection", 
            new { result.ids },
            result.companies);
    }
    // /////////////////////////////////////////////////////////////////////////


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService
            .DeleteCompanyAsync(id, trackChanges: false);

        return NoContent();
    }


    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        //if (company is null)
        //{
        //    return BadRequest("CompanyForUpdateDto object is null");
        //}

        //if (!ModelState.IsValid)
        //{
        //    return UnprocessableEntity(ModelState);
        //}

        await _service.CompanyService
            .UpdateCompanyAsync(id, company, trackChanges: true);

        return NoContent();
    }


    [HttpOptions]
    public IActionResult GetCompaniesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST");

        return Ok();
    }
}
