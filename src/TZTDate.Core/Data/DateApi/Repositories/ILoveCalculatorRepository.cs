using TZTDate.Core.Data.DateApi.Models;

namespace TZTDate.Core.Data.LoveCalculator.Repositories;

public interface ILoveCalculatorRepository
{
    public Task<LoveCalculatorModel> GetLovePercentage(string fname, string? sname = "");
}
