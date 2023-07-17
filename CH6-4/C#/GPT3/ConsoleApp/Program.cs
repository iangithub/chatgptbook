// See https://aka.ms/new-console-template for more information
using Azure.AI.TextAnalytics;
using Azure;
using ConsoleApp;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

//AOAI API授權金鑰
const string api_Key = "{aoai_key}";
const string aoai_Service_Name = "{aoai_service_name}";
const string deployment_Name = "{aoai_deploy_name}";
const string api_Version = "2022-12-01";

//Sentiment API授權金鑰
const string sentimentAnalysis_Key = "{sentiment_key}";
const string sentimentAnalysis_Endpoint = "https://{service_name}.cognitiveservices.azure.com";

//使用 Completions API 搭配 GPT3 模型
const string api_Endpoint = $"https://{aoai_Service_Name}.openai.azure.com/openai/deployments/{deployment_Name}/completions?api-version={api_Version}";

//模擬客戶留言
//var userText = "早餐不好吃";
//var userText = "櫃檯人員很親切";
var userText = "房間陳設普通";

var sentiments = string.Empty;
var sentimentAnalysis = string.Empty;
var opinions = new List<string>();

var client = new TextAnalyticsClient(new Uri(sentimentAnalysis_Endpoint), new AzureKeyCredential(sentimentAnalysis_Key));
var documents = new List<string> { userText };
AnalyzeSentimentResultCollection reviews = client.AnalyzeSentimentBatch(documents, options: new AnalyzeSentimentOptions()
{
    IncludeOpinionMining = true //包含具體對象挖掘
});

if (reviews != null && reviews.Any())
{
    sentimentAnalysis = reviews[0].DocumentSentiment.Sentiment.ToString();
    Console.WriteLine("========= sentimentAnalysis ================== ");
    Console.WriteLine(sentimentAnalysis);

    //如果有拿到目標(主詞)，就也記錄下來
    foreach (SentenceSentiment sentence in reviews[0].DocumentSentiment.Sentences)
    {
        opinions.Add(sentence.Text);
    }

    //送往Azure OpenAI
    await GPT_AnalysisAsync(sentimentAnalysis, opinions);
}

async Task GPT_AnalysisAsync(string sentimentAnalysis, List<string> opinions)
{
    //prompt 開始加工使用者的輸入
    string prompt_Template = $"你是一名旅館經理，現在顧客的情緒是{sentimentAnalysis}，顧客這樣的情緒是來自於旅館的";
    foreach (var item in opinions)
    {
        prompt_Template += item + ",";
    }
    prompt_Template += "。請具體針對顧客提出的主題，說一段話來回應顧客，不用太長。\nAI：";

    try
    {
        using (HttpClient client = new HttpClient())
        {
            //建立API請求參數，並將prompt_Template參數值帶入
            var req_model = new ApiRequestModelGpt3()
            {
                Prompt = prompt_Template,
                Temperature = 0.8f,
                Max_Tokens = 300 //限制總 token數，依模型有不同上限
            };

            //將API授權金鑰加入Http請求標頭
            client.DefaultRequestHeaders.Add("api-key", api_Key);
            var data = new StringContent(JsonConvert.SerializeObject(req_model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(api_Endpoint, data);
            var responseContent = await response.Content.ReadAsStringAsync();

            var completion = JsonConvert.DeserializeObject<Completion>(responseContent);

            //GPT 生成結果
            var result = completion.Choices[0].Text;

            Console.WriteLine("========= GPT 回應 ================== ");
            Console.WriteLine(result);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

}

