// See https://aka.ms/new-console-template for more information
using ConsoleApp;
using Newtonsoft.Json;
using System.Text;

//API授權金鑰
const string api_Key = "{aoai_key}";

const string aoai_Service_Name = "{aoai_service_name}";
const string api_Version = "2022-12-01";
const string deployment_Name = "{aoai_deploy_name}";

//使用 Completions API 搭配 text-davinci-003 模型
const string api_Endpoint = $"https://{aoai_Service_Name}.openai.azure.com/openai/deployments/{deployment_Name}/completions?api-version={api_Version}";

//prompt
const string prompt_Template = @"現在開始，你是一位專欄作家。請你以 ### ChatGPT在教學領域的應用 ### 為主題，並使用繁體中文創作一篇文章。\nAI：";

try
{
    using (HttpClient client = new HttpClient())
    {
        //建立API請求參數，並將prompt_Template參數值帶入
        var req_model = new ApiRequestModelGpt3()
        {
            Prompt = prompt_Template,
            Temperature = 0.5f,
            Best_of=1,
            Max_Tokens = 1000 //限制總 token數，依模型有不同上限
        };

        //將API授權金鑰加入Http請求標頭
        client.DefaultRequestHeaders.Add("api-key", api_Key);
        var data = new StringContent(JsonConvert.SerializeObject(req_model), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(api_Endpoint, data);
        var responseContent = await response.Content.ReadAsStringAsync();

        var completion = JsonConvert.DeserializeObject<Completion>(responseContent);
        Console.WriteLine(responseContent);
        Console.WriteLine("====================================");
        Console.WriteLine(completion.Choices[0].Text);
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

