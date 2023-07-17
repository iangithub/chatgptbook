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
        var requestModel = new ApiRequestModelGpt4(@"你是一個服務自動預約系統，請協助自動分析客人的服務需求，包含 intent 及 entities ，並且使用JSON格式輸出回應，JSON屬性用英文
JSON格式為
{
""intent"":""xxxx"",
""entities"":[{"""",""""},{"""",""""}]
}");
        requestModel.Temperature = 0.2f;
        requestModel.Max_Tokens = 500; //限制 token 上限數
        requestModel.AddUserMessages(@"預約接駁車，從火車站到飯店，有3個大人，2個小孩，5件行李");

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
