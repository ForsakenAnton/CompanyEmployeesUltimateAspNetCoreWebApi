
using Microsoft.AspNetCore.Mvc; // (!) Deprecated
using Service.Contracts;
using Shared.DataTransferObjects;
using CompanyEmployees.Presentation.ModelBinders;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    public CompaniesController(IServiceManager service)
    {
        _service = service;
    }


    [HttpGet]
    public IActionResult GetCompanies()
    {
        //try
        //{
        //throw new Exception("Exception 0_o");
            var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);

            return Ok(companies);
        //}
        //catch
        //{
        //    return StatusCode(500, "Internal server error");
        //}
    }

    //[HttpGet("{id:guid}")]
    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService
            .GetCompany(id, trackChanges: false);

        return Ok(company);
    }


    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
    {
        if (company is null)
        {
            return BadRequest("CompanyForCreationDto object is null");
        }

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }

        var createdCompany = _service.CompanyService
            .CreateCompany(company);

        return CreatedAtRoute(
            routeName: "CompanyById",
            routeValues: new { id = createdCompany.Id },
            value: createdCompany);
    }


    // Companies Collection ///////////////////////////////////////////////
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = _service.CompanyService
            .GetByIds(ids, trackChanges: false);

        return Ok(companies);
    }


    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result = _service.CompanyService
            .CreateCompanyCollection(companyCollection);

        return CreatedAtRoute(
            "CompanyCollection", 
            new { result.ids },
            result.companies);
    }
    // /////////////////////////////////////////////////////////////////////////


    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCompany(Guid id)
    {
        _service.CompanyService
            .DeleteCompany(id, trackChanges: false);

        return NoContent();
    }


    [HttpPut("{id:guid}")]
    public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        if (company is null)
        {
            return BadRequest("CompanyForUpdateDto object is null");
        }

        _service.CompanyService
            .UpdateCompany(id, company, trackChanges: true);

        return NoContent();
    }
}
