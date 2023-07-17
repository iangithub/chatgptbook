// See https://aka.ms/new-console-template for more information
using Azure;
using Azure.AI.Language.QuestionAnswering;
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

//Azure Question Ansering相關參數值
const string az_QuestionAnsering_Endpoint = "https://{service_name}.cognitiveservices.azure.com";
const string az_QuestionAnsering_Credential = "{qa_api_key}";
const string az_QuestionAnsering_ProjectName = "{qa_project_name}}";
const string az_QuestionAnsering_DeploymentName = "{qa_deploy_name}";
        

//使用 Chat Completions API 搭配 GPT-4 模型
const string api_Endpoint = $"https://{aoai_Service_Name}.openai.azure.com/openai/deployments/{deployment_Name}/chat/completions?api-version={api_Version}";

try
{
    var userMsg ="停車場在哪裡？怎麼收費？";

    using (HttpClient client = new HttpClient())
    {
        //使用Azure.AI.Language.QuestionAnswering套件呼叫QuestionAnswering服務
        Uri endpoint = new Uri(az_QuestionAnsering_Endpoint);
        AzureKeyCredential credential = new AzureKeyCredential(az_QuestionAnsering_Credential);
        QuestionAnsweringClient qa_client = new QuestionAnsweringClient(endpoint, credential);
        QuestionAnsweringProject project = new QuestionAnsweringProject(az_QuestionAnsering_ProjectName, az_QuestionAnsering_DeploymentName);
        Response<AnswersResult> qa_response = await qa_client.GetAnswersAsync(userMsg, project);
        
        //取得QA智慧搜尋的答案
        var qa_Response = qa_response.Value.Answers[0] != null ? qa_response.Value.Answers[0].Answer : "很抱歉,無法回答您的問題";


        var requestModel = new ApiRequestModelGpt4(@"你是一位客服人員，我會提供給你要回答客戶的答案，
請你進行內容文字的修飾調整並以客服語氣產生回答內容");
        requestModel.Temperature = 0.5f;
        requestModel.Max_Tokens = 500; //限制 token 上限數

        //將QA智慧搜尋的答案轉交GPT模型進行修飾
        requestModel.AddUserMessages(qa_Response);

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
