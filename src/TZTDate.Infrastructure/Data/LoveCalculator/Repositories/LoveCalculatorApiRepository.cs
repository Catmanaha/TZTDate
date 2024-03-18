using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using TZTDate.Core.Data.DateApi.Managers;
using TZTDate.Core.Data.LoveCalculator.Models;
using TZTDate.Core.Data.LoveCalculator.Repositories;

namespace TZTDate.Infrastructure.Data.LoveCalculator.Repositories;

public class LoveCalculatorApiRepository : ILoveCalculatorRepository
{
    private readonly HttpClient client;

    public LoveCalculatorApiRepository(IOptions<ApiManager> options, HttpClient client)
    {
        this.client = client;
        client.BaseAddress = new Uri(options.Value.BaseUrl);

        foreach (var header in options.Value.Headers)
        {
            client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

    }

    public async Task<LoveCalculatorModel> GetLovePercentage(string fname, string sname)
    {
        var result = await client.GetAsync($"getPercentage?fname={fname}&sname={sname}");
        var json = await result.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        return JsonSerializer.Deserialize<LoveCalculatorModel>(json);
    }
}
