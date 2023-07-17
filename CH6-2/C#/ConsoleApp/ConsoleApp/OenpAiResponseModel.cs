using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class OenpAiResponseModel
    {
        [JsonProperty(PropertyName = "created")]
        public long Created { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<Link>? Data { get; set; }
    }

    // serves as our input model
    public class Input
    {
        [JsonProperty(PropertyName = "prompt")]
        public string? Prompt { get; set; }
        
        [JsonProperty(PropertyName = "n")]
        public short? N { get; set; }
        
        [JsonProperty(PropertyName = "size")]
        public string? Size { get; set; }
    }

    // model for the image url
    public class Link
    {
        [JsonProperty(PropertyName = "url")]
        public string? Url { get; set; }
    }
}
