using Newtonsoft.Json;
namespace TZTDate.Core.Data.DateApi.Models
{
    public class LoveCalculatorModel
    {
        [JsonProperty("first name")]
        public string? fname { get; set; }  
        [JsonProperty("second name")]
        public string? sname { get; set; }
        public string? result { get; set; }
    }
}