// See https://aka.ms/new-console-template for more information
using ConsoleApp;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

var openai_API_KEY = "{oai_key}";
var openai_Endpoint = "https://api.openai.com/v1/images/generations";

var resp = new OenpAiResponseModel();

var reqMode = new
{
    prompt = "A cute cat looking at the camera, begging for food.",
    n = 1,
    size = "1024x1024"
};


using (var client = new HttpClient())
{
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openai_API_KEY);
    var Message = await client.
          PostAsync(openai_Endpoint,
          new StringContent(JsonConvert.SerializeObject(reqMode),
          Encoding.UTF8, "application/json"));

    if (Message.IsSuccessStatusCode)
    {
        var content = await Message.Content.ReadAsStringAsync();
        resp = JsonConvert.DeserializeObject<OenpAiResponseModel>(content);

        Console.WriteLine(resp.Data[0].Url);
    }
}
