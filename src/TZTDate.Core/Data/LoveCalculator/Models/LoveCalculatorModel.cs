using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TZTDate.Core.Data.LoveCalculator.Models
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