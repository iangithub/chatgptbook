using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ApiRequestModelGpt3
    {
        [JsonProperty(PropertyName = "prompt")]
        public string Prompt { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public float Temperature { get; set; }
        [JsonProperty(PropertyName = "top_p")]
        public float Top_p { get; set; }
        [JsonProperty(PropertyName = "frequency_penalty")]
        public int Frequency_Penalty { get; set; }
        [JsonProperty(PropertyName = "presence_penalty")]
        public int Presence_Penalty { get; set; }
        [JsonProperty(PropertyName = "max_tokens")]
        public int Max_Tokens { get; set; }
        [JsonProperty(PropertyName = "best_of")]
        public int Best_of { get; set; }
        [JsonProperty(PropertyName = "stop")]
        public string Stop { get; set; }
    }
}
