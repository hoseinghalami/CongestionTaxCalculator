using Application.Dtos;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TaxController(ITaxService taxService) : ControllerBase
{

    [HttpPost(nameof(CalculateTax))]
    public async Task<ActionResult> CalculateTax([FromBody] TaxCalculationDto taxCalculationDto)
    {
        var result = await taxService.CalculateTax(taxCalculationDto);
        return result == -1 ?
            BadRequest() :
            Ok(result);
    }
}

