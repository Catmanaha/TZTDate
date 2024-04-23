using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.DateApi.Dtos;
using TZTDate.Core.Data.DateApi.Models;
using TZTDate.Core.Data.LoveCalculator.Repositories;
using TZTDate.WebApi.Filters;

namespace TZTDate.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class LoveCalculatorController : ControllerBase
{
    private readonly ILoveCalculatorRepository loveCalculatorRepository;

    public LoveCalculatorController(ILoveCalculatorRepository loveCalculatorRepository)
    {
      this.loveCalculatorRepository = loveCalculatorRepository;
    }

    [HttpPost]
    public async Task<LoveCalculatorModel> Index(LoveCalculatorDto loveCalculatorDto)
    {
      var result = await loveCalculatorRepository.GetLovePercentage(
          loveCalculatorDto.fname, loveCalculatorDto.sname);

      return result;
    }
}