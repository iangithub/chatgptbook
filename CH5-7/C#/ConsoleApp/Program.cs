// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Net;
using System.Text;


//API授權金鑰
const string api_Key = "{api_key}";
const string api_Endpoint = "https://{your_service_name}.cognitiveservices.azure.com/";

const string projectName = "SmartHome-YYYYMMDD";
const string deploymentName = "clu-yyyymmdd-deploy";

try
{
    string route = "language/:analyze-conversations?api-version=2022-10-01-preview";

    var txt = @"我想要關燈";

    var reqModel = new RequestModel();
    reqModel.parameters.projectName = projectName;
    reqModel.parameters.deploymentName = deploymentName;
    reqModel.analysisInput.conversationItem.id = "1";
    reqModel.analysisInput.conversationItem.participantId = "1";
    reqModel.analysisInput.conversationItem.text = txt;

    var requestBody = JsonConvert.SerializeObject(reqModel);

    using (var client = new HttpClient())
    {
        using (var request = new HttpRequestMessage())
        {
            //準備API請求參數
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(api_Endpoint + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", api_Key);

            //發送請求並取得回應
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var rsp = await response.Content.ReadAsStringAsync();
                var rspModel = JsonConvert.DeserializeObject<ResponseModel>(rsp);

                Console.WriteLine($"Intent :{rspModel.result.prediction.topIntent}");
                Console.WriteLine($"category :{rspModel.result.prediction.entities[0].category}");
               Console.WriteLine($"entitie :{rspModel.result.prediction.entities[0].extraInformation[0].key}");
            }
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


