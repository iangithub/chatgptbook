// See https://aka.ms/new-console-template for more information
using Azure;
using Azure.AI.TextAnalytics;


//API授權金鑰
const string api_Key = "{api_key}";
const string api_Endpoint = "https://{your_service_name}.cognitiveservices.azure.com/";

AzureKeyCredential credentials = new AzureKeyCredential(api_Key);
Uri endpoint = new Uri(api_Endpoint);


try
{
    var client = new TextAnalyticsClient(endpoint, credentials);
    var userMsg = "熱水一點都不熱，洗到冷水澡！真令人生氣！";

    //評論留言，及指定語系(繁體中文zh-hant、英文en)
    var reviews = await client.AnalyzeSentimentAsync(userMsg, language: "zh-hant", options: new AnalyzeSentimentOptions()
    {
        IncludeOpinionMining = true
    });

    Console.WriteLine(reviews.Value.Sentiment);

}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


