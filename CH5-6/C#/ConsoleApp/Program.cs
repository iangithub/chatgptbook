// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Net;
using System.Text;


//API授權金鑰
const string api_Key = "{api_key}";
const string api_Endpoint = "https://{your_service_name}.cognitiveservices.azure.com/";

try
{
    string route = "language/:analyze-text?api-version=2022-05-01";

    var txt = @"萊恩在4/17~4/22在美國微軟總部參加MVP Summit大會。";

    var reqModel = new RequestModel();
    reqModel.analysisInput.documents.Add(new Document()
    {
        id = "1",
        language = "zh-Hans",
        text = txt
    });

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

                foreach (var item in rspModel.results.documents[0].entities)
                {
                    Console.WriteLine($"類別：{item.category},關鍵資訊：{item.text}");
                }
            }

        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


