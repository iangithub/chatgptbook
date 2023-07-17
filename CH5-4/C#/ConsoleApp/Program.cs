// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Net;
using System.Text;


//API授權金鑰
const string api_Key = "{api_key}";
const string api_Endpoint = "https://api.cognitive.microsofttranslator.com";
const string location = "{service_region}";

try
{
    string route = "/translate?api-version=3.0&to=en&to=zh-Hant";

    var translator_input = "Ryan, you are so handsome!";
    //var translator_input= "萊恩, 你太帥了！";
    //var translator_input= "ライアン、あなたはとてもハンサムです！";
    object[] body = new object[] { new { Text = translator_input } };
    var requestBody = JsonConvert.SerializeObject(body);

    using (var client = new HttpClient())
    using (var request = new HttpRequestMessage())
    {
        //準備API請求參數
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(api_Endpoint + route);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        request.Headers.Add("Ocp-Apim-Subscription-Key", api_Key);
        request.Headers.Add("Ocp-Apim-Subscription-Region", location);

        //發送請求並取得回應
        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
        string result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


