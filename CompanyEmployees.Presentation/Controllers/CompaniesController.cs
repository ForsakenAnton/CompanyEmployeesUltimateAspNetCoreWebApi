
using Microsoft.AspNetCore.Mvc; // (!) Deprecated
using Service.Contracts;
using Shared.DataTransferObjects;

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

        var createdCompany = _service.CompanyService
            .CreateCompany(company);

        return CreatedAtRoute(
            routeName: "CompanyById",
            routeValues: new { id = createdCompany.Id },
            value: createdCompany);
    }
}
