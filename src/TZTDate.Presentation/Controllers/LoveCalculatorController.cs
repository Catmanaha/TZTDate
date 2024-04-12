using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.DateApi.Dtos;
using TZTDate.Core.Data.LoveCalculator.Repositories;

namespace TZTDate.Presentation.Controllers;

public class LoveCalculatorController : Controller
{
    private readonly ILoveCalculatorRepository loveCalculatorRepository;

    public LoveCalculatorController(ILoveCalculatorRepository loveCalculatorRepository)
    {
        this.loveCalculatorRepository = loveCalculatorRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoveCalculatorDto loveCalculatorDto)
    {
        var result = await loveCalculatorRepository.GetLovePercentage(loveCalculatorDto.fname, loveCalculatorDto.sname);
       
        return View(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}