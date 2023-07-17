const axios = require('axios');

const api_Key = '{aoai_key}';
const aoai_Service_Name = '{aoai_service_name}';
const deployment_Name = '{aoai_deploy_name}';
const api_Version = '2023-03-15-preview';

//使用 Chat Completions API 搭配 GPT-4 模型
const api_Endpoint = `https://${aoai_Service_Name}.openai.azure.com/openai/deployments/${deployment_Name}/chat/completions?api-version=${api_Version}`;

const requestModel={};


requestModel.temperature = 0.2;
requestModel.max_tokens = 1000; //限制 token 上限數
requestModel.messages = [
  { role: 'system', content: '現在開始你是一位C#與node.js專家，將會提供這2種程式語言語法轉換的服務，並且針對轉換完成的程式碼補上適當的繁體中文註解。' },
  { role: 'user', content: `using System;
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
  }` }
];

axios.post(api_Endpoint, requestModel, {
  headers: {
    'api-key': api_Key,
    'Content-Type': 'application/json'
  }
})
  .then(response => {
    const completion = response.data;
    console.log(completion.choices[0].message.content);
  })
  .catch(error => {
    console.error(error);
  });
