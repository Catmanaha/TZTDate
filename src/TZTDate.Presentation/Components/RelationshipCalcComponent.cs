using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.LoveCalculator.Repositories;

namespace TZTDate.Presentation.Components;

public class RelationshipCalcComponent : ViewComponent
{
    private readonly ILoveCalculatorRepository loveCalculatorRepository;

    public RelationshipCalcComponent(ILoveCalculatorRepository loveCalculatorRepository)
    {
        this.loveCalculatorRepository = loveCalculatorRepository;
    }
    public async Task<IViewComponentResult> InvokeAsync(string name)
    {
        var result = await loveCalculatorRepository.GetLovePercentage(name);
        return View("PartialForDetails", result.result);
    }
}
