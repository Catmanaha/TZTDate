using TZTDate.Core.Data.LoveCalculator.Models;

namespace TZTDate.Core.Data.LoveCalculator.Repositories;

public interface ILoveCalculatorRepository
{
    public Task<LoveCalculatorModel> GetLovePercentage(string fname, string sname);
}
