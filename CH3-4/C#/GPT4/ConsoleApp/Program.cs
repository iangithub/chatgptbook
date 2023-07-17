// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Text;


//API授權金鑰
const string api_Key = "{aoai_key}";

const string aoai_Service_Name = "{aoai_service_name}";
const string deployment_Name = "{aoai_deploy_name}";
const string api_Version = "2023-03-15-preview";


//使用 Chat Completions API 搭配 GPT-4 模型
const string api_Endpoint = $"https://{aoai_Service_Name}.openai.azure.com/openai/deployments/{deployment_Name}/chat/completions?api-version={api_Version}";

try
{
    using (HttpClient client = new HttpClient())
    {
        var requestModel = new ApiRequestModelGpt4("現在開始你是一位文字閱讀高手。");
        requestModel.Temperature = 0.7f;
        requestModel.Max_Tokens = 200; // 控制生成摘要的最大長度

        //抽象摘要
        //string prompt = "請為以下文章生成使用繁體中文輸出100個字以內的抽象摘要，摘要包含其主要觀點，";

        //提取摘要
        string prompt = "請為以下文章生成使用繁體中文輸出提取摘要，摘要包含3至5個重點句子，";
        string sourceContent = @"微軟日前已宣布將 GPT-4 導入全新 Bing 搜尋引擎和 Microsoft 365 Copilot，今日的發表將有助於各產業，同樣以先進模型為基礎，藉由 Azure OpenAI 服務發展自身的應用程式。 

透過生成式 AI 技術，我們將幫助各產業在業務運作上更有效率。例如：機器人開發人員使用 Azure OpenAI 服務中的 Power Virtual Agents Copilot 後，將可以在幾分鐘內透過自然語言創造語音助理。 

GPT-4 具備運用其廣泛的知識、問題解決的能力及領域專長，將使用體驗推升至全新層次。透過 Azure OpenAI 服務中的 GPT-4，企業可以利用具備資安與審核機制的的模型，降低有害輸出，以達到精簡組織內部溝通及與客戶應對的流程。 

各種規模的企業皆正在運用 Azure AI，許多企業亦使用 Azure OpenAI 服務在生產過程中部署語言模型，並充分瞭解這項服務是由 Azure 獨特的超級運算及企業等級能力支持。服務內容包含改善端對端客戶體驗、摘要長篇幅文章、協助撰寫軟體，以及透過預判正確的數據降低營運風險。 

顧客正在加速採用語言模型 

現階段，微軟對生成式 AI 技術的了解只是冰山一角，但我們正努力探索這項技術，並致力於讓我們的客戶負責地使用 Azure OpenAI Service，以產生真正的影響。隨著 GPT-4 的推出，Epic Healthcare、Coursera 和 Coca-Cola 計劃以獨特的方式運用這項新技術： 

「我們對 GPT-4 的研究顯示其在醫療保健應用上的巨大可能性。我們將利用 GPT-4 幫助醫師及護理師減少文書工作的時間，而在數據檢查上可以用更簡易且具對話性的方式進行。」—— Seth Hain, Senior Vice President of Research and Development at Epic 

「Coursera 正在利用 Azure OpenAI 服務，在平台上創造一個由 AI 驅動的全新學習體驗，讓學習者可以在學習過程中得到高質感與客製化的支援。Azure OpenAI 服務整合全新 GPT-4 模型，將幫助全球數百萬用戶更有效地於 Coursera 上學習。」 —— Mustafa Furniturewala, Senior Vice President of Engineering at Coursera 

「作為民生消費品企業，我們無法用文字形容我們對於 Azure OpenAI 帶來的無限機會的興奮與感謝。將 Azure Cognitive Services 運用在企業的核心數位服務架構，我們利用 OpenAI 文字與圖片生成模型的變革性力量，解決業務問題並建立知識中心。然而，OpenAI 帶來的 GPT-4 多模式功能的巨大潛力以及它發展可能性，才真正讓我們感到驚嘆與敬佩。GPT-4 在行銷、廣告、公共與客戶關係的應用上具有無限可能性，而我們也迫不及待體驗此革命性科技。我們瞭解成功並不只是因為科技發展，也需要擁有正確的企業輔助工具。這是我們成為 Microsoft Azure 長期合作夥伴的原因，以確保我們擁有向客戶傳達超群體驗的所有工具。Azure OpenAI 不只是一項尖端技術，它是真正可以改變局勢的角色，而我們很榮幸可以一同參與這不可思議的旅程。」——Lokesh Reddy Vangala, Senior Director of Engineering, Data and AI, The Coca-Cola Company 

我們對負責任的 AI 的承諾 
正如我之前在部落格上所描述的，微軟採用分層方法來管理生成式模型，並遵循微軟負責任的 AI 原則。在 Azure OpenAI 中，整合式安全系統將提供保護，防止有害的輸入和輸出，並監測濫用情況。在此基礎上，我們為客戶提供指導和最佳實踐，以負責任的方式使用這些模型，並期望客戶遵守 Azure OpenAI 的行為規範。透過 OpenAI 的新研究進展，GPT-4 模型將增加了一層保護。在人類反饋的指導下，GPT-4 將具備更高級別的安全性，使模型更有效地處理有害輸入，降低模型生成有害回應的可能性。 ";
        requestModel.AddUserMessages(prompt + $" #### {sourceContent} ####");

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

