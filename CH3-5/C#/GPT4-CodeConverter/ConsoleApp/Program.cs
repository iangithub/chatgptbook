// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Text;


//API授權金鑰
const string api_Key = "{aoai_key}";
//AOAI服務名稱
const string aoai_Service_Name = "{aoai_service_name}";
//AOAI部署名稱
const string deployment_Name = "{aoai_deploy_name}";
//API版本
const string api_Version = "2023-03-15-preview";


//使用 Chat Completions API 搭配 GPT-4 模型
const string api_Endpoint = $"https://{aoai_Service_Name}.openai.azure.com/openai/deployments/{deployment_Name}/chat/completions?api-version={api_Version}";

try
{
    using (HttpClient client = new HttpClient())
    {
        var requestModel = new ApiRequestModelGpt4("現在開始你是一位C#與node.js專家，將會提供這2種程式語言語法轉換的服務，並且針對轉換完成的程式碼補上適當的繁體中文註解。");
        requestModel.Temperature = 0.2f;
        requestModel.Max_Tokens = 1000; //限制 token 上限數
        requestModel.AddUserMessages(@"using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirQualityAPI
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            string apiKey = ""e8dd42e6-9b8b-43f8-991e-b3dee723a52d"";
            string url = $""https://data.epa.gov.tw/api/v2/aqx_p_432?api_key={apiKey}&limit=1000&sort=ImportDate%20desc&format=JSON"";

            try
            {
                string response = await GetApiResponseAsync(url);
                JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response);
                Console.WriteLine(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($""Error: {ex.Message}"");
            }
        }

        public static async Task<string> GetApiResponseAsync(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
");

        var json = JsonConvert.SerializeObject(requestModel);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("api-key", api_Key);
        var response = await client.PostAsync(api_Endpoint, data);
        var responseContent = await response.Content.ReadAsStringAsync();

        var completion = JsonConvert.DeserializeObject<Completion>(responseContent);

        Console.WriteLine(completion.Choices[0].Message.Content);

    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
