const axios = require('axios');

const api_Key = '{aoai_key}';
const aoai_Service_Name = '{aoai_service_name}';
const deployment_Name = '{aoai_deploy_name}';
const api_Version = '2023-03-15-preview';

//使用 Chat Completions API 搭配 GPT-4 模型
const api_Endpoint = `https://${aoai_Service_Name}.openai.azure.com/openai/deployments/${deployment_Name}/chat/completions?api-version=${api_Version}`;

const requestModel={};


requestModel.temperature = 0.2;
requestModel.max_tokens = 1000;
requestModel.messages = [
  { role: 'system', content: `現在開始你是一位 C# 程式語言專家。
  當我指示code review：時，請針對我所提供的程式碼進行分析，並指出有問題的程式碼，
  以json格式提供相關繁體中文說明及修正建議` },
  { role: 'user', content: `code review:
  namespace ConsoleApplication
  {
      public class Program
      {
          public static void Main(string[] args)
          {
              for(int i = 0; i<10; i++)
              {
          using(var client = new HttpClient())
          {
            var result = client.GetAsync(""https://learn.microsoft.com/"").Result;
            Console.WriteLine(result.StatusCode);
          }
              }
              Console.ReadLine();
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
