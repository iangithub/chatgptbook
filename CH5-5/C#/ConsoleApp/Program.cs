// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Net;
using System.Text;


//API授權金鑰
const string api_Key = "{api_key}";
const string api_Endpoint = "https://{your_service_name}.cognitiveservices.azure.com/";

try
{
    string route = "language/analyze-text/jobs?api-version=2022-10-01-preview";

    var doc = @"（中央社紐約4日綜合外電報導）美國財經資訊公司彭博（Bloomberg）近日宣布研發自有聊天機器人BloombergGPT，盼推出專於金融領域的人工智慧（AI）資訊處理應用，
以提供客戶和記者更好的功能與服務。美國新聞業網站尼曼實驗室（Nieman Lab）報導，彭博3月31日發表研究論文詳述BloombergGPT的開發。據彭博介紹，BloombergGPT是「一個新的大規模生成式AI模型
。這個大型語言模型（LLM）專門鎖定範圍廣泛的金融資料來訓練生成，目的為了支援多元化的金融產業自然語言處理（NLP）任務集」。彭博表示，近期以大型語言模型為基礎的人工智慧發展，
已在許多領域展示出令人振奮的新應用；但金融領域因其複雜性及具有專門術語，有必要有專屬模型。因此BloombergGPT的推出，代表將聊天機器人這項新科技開發應用到金融產業的第一步。
彭博指出，BloombergGPT將協助其改善現有金融相關自然語言處理的任務，例如文本情感分析、命名實體辨識（NER）、新聞分類、回答問題和其他功能。此外，它也創造新機會來排列可從彭博終端機取得的巨量資料
，以提供客戶更好的協助。至於BloombergGPT的訓練規模，彭博表示它的語料庫有7000億餘個token（字詞碎片）。相較之下，熱門聊天機器人ChatGPT的開發公司OpenAI在2020年推出的模型GPT-3
，訓練的語料庫則約有5000億個token。根據彭博說法，BloombergGPT的語料庫中，有3630億個token取自彭博自有金融資料，也就是來自彭博終端機的資料庫，
彭博號稱這是「至今最大的特定領域資料集（dataset）」；其餘3450億個token則是取自其他來源的通用資料集。彭博還說，訓練資料分為財金類FinPile和一般The Pile兩類。其中FinPile包括彭博檔案庫中的各類英文金融文件
，如新聞文章、公告、新聞稿、網頁內容和社群媒體資料，以及彭博記者撰寫的新聞以外所有的新聞來源。至於ThePile則是龐雜的語料庫，
來源從YouTube的畫面擷取、文藝數位化的古騰堡計畫（Project Gutenberg）到AI訓練常見的安隆公司（Enron）電郵快取。究竟BloombergGPT能夠如何應用？尼曼實驗室的文章表示，
按照其訓練原理，它應該具有像ChatGPT的功能，但此外也能處理與彭博需求更相關的任務，例如將自然語言指令翻譯成彭博查詢語言（Bloomberg Query Language）終端機的使用者偏好功能。
BloombergGPT也能為新聞文章提議具有彭博新聞風格的標題。彭博還說，BloombergGPT更能勝任回答與商業有關的提問，無論是有關文本情感分析、分類、資料擷取或任何其他任務。
（譯者：張正芊/ 核稿：林治平）1120405";

    
    var reqModel = new RequestModel();
    reqModel.analysisInput.documents.Add(new Document() { id = "1", language = "zh-Hans", text = doc });

    var summary_Endpoint = string.Empty;
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

            // 檢查是否存在 Content-Type 標頭
            if (response.Headers.Contains("operation-location"))
            {
                summary_Endpoint = response.Headers.GetValues("operation-location").First();

                Console.WriteLine($"summary_Endpoint: {summary_Endpoint}");
            }
        }

        //由於摘要服務有可能需要一些時間處理(依輸入文字長度而定)，稍後一會才向Azure取回結果
        //可自行修改以輪詢方式處理
        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(15));

        //取得摘要結果
        using (var request = new HttpRequestMessage())
        {
            //以job_id取回抽象摘要結果
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(summary_Endpoint);
            request.Headers.Add("Ocp-Apim-Subscription-Key", api_Key);

            //發送請求並取得回應
            var response = await client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            var summary_Result = JsonConvert.DeserializeObject<ResponseModel>(result);

            Console.WriteLine($"============= Summary ==================");

            foreach (var item in summary_Result.tasks.items[0].results.documents[0].sentences)
            {
                //這邊可視判斷結果調整，目前設定為，只顯示分數高於0.8的句子
                if (item.rankScore >= 0.8)
                {
                    Console.WriteLine($"txt:{item.text},Score:{item.rankScore}");
                }
            }
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


