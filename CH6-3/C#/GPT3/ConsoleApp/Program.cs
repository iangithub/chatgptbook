// See https://aka.ms/new-console-template for more information
using ConsoleApp;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

//AOAI API授權金鑰
const string api_Key = "{aoai_key}";
const string aoai_Service_Name = "{aoai_service_name}";
const string api_Version = "2022-12-01";
const string deployment_Name = "{aoai_deploy_name}";

//Speech API授權金鑰
const string speech_Key = "{speech_key}";
const string speech_Region = "{speech_region}";


//使用 Completions API 搭配 GPT3 模型
const string api_Endpoint = $"https://{aoai_Service_Name}.openai.azure.com/openai/deployments/{deployment_Name}/completions?api-version={api_Version}";

//模擬故事關鍵字
var userText = "bear";
//prompt 開始加工使用者的輸入
string prompt_Template = $"Can yo make a story about {userText} Keep it short, around 100 words. \nAI：";

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

        //GPT生成的故事內容
        var gptStory = completion.Choices[0].Text;

        await Text2SpeechAsync(gptStory);
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

async Task Text2SpeechAsync(string userInput)
{
    //設定稍後新增的聲音檔案名稱
    var audioFile = "YourAudioFile.wav";
    //準備Speech Service所需資訊
    var speechConfig = SpeechConfig.FromSubscription(speech_Key, speech_Region);
    var audioConfig = AudioConfig.FromWavFileOutput(audioFile);
    //選擇語音角色
    speechConfig.SpeechSynthesisVoiceName = "en-US-AmberNeural";
    //開始文字轉語音
    using (var speechSynthesizer = new SpeechSynthesizer(speechConfig, audioConfig))
    {
        //把GPT生成的故事內容放入轉語音的參數
        var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(userInput);

        switch (speechSynthesisResult.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                Console.WriteLine($"Speech synthesized finished");
                break;
            case ResultReason.Canceled:
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
            default:
                break;
        }
    }
}


