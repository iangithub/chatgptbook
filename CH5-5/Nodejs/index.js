'use strict';
// 使用express框架來作為Node程式碼基本架構，方便未來改寫成網站或Chatbot
const express = require('express');
// 使用config來管理密碼、API Key
const config = require('config');
const axios = require('axios').default;

//Azure Key - 建立config資料夾，在裡面新增default.json檔案
//裡面寫上{ "LANGUAGE_API_KEY":"XXXXXXX","ENDPOINT":"XXXXXX"}
//填上在Azure申請到的API Key、語言服務端點網址
const endpoint = config.get("ENDPOINT");
const apiKey = config.get("LANGUAGE_API_KEY");

//開始建立Express應用程式
const app = express();

//決定伺服器的Port號碼，為了相容於本地端測試與未來雲端部署，判斷是否有環境變數的設定，若沒有則給予固定值
const port = process.env.port || process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`listening on ${port}`);
    //伺服器啟動之後，就跳進這個函數開始準備呼叫摘要服務
    MS_TextSummary()
        .catch((err) => {
            console.error("Error:", err);
        });
});

async function MS_TextSummary() {
    console.log("[MS_TextSummary] in");
    //使用axios套件來呼叫文字摘要API服務
    //data那裡放上想要摘要的文字並指定語言
    axios({
        baseURL: endpoint,
        url: 'language/analyze-text/jobs?api-version=2022-10-01-preview',
        method: 'post',
        headers: {
            'Ocp-Apim-Subscription-Key': apiKey,
            'Content-type': 'application/json',
        },
        params: {
        },
        data: {
            "displayName": "Document ext Summarization Task Example",
            "analysisInput": {
                "documents": [
                    {
                        "id": "1",
                        "language": "zh-Hans",
                        // "language": "en",
                        // "text": "At Microsoft, we have been on a quest to advance AI beyond existing techniques, by taking a more holistic, human-centric approach to learning and understanding. As Chief Technology Officer of Azure AI Cognitive Services, I have been working with a team of amazing scientists and engineers to turn this quest into a reality. In my role, I enjoy a unique perspective in viewing the relationship among three attributes of human cognition: monolingual text (X), audio or visual sensory signals, (Y) and multilingual (Z). At the intersection of all three, there’s magic—what we call XYZ-code as illustrated in Figure 1—a joint representation to create more powerful AI that can speak, hear, see, and understand humans better. We believe XYZ-code will enable us to fulfill our long-term vision: cross-domain transfer learning, spanning modalities and languages. The goal is to have pre-trained models that can jointly learn representations to support a broad range of downstream AI tasks, much in the way humans do today. Over the past five years, we have achieved human performance on benchmarks in conversational speech recognition, machine translation, conversational question answering, machine reading comprehension, and image captioning. These five breakthroughs provided us with strong signals toward our more ambitious aspiration to produce a leap in AI capabilities, achieving multi-sensory and multilingual learning that is closer in line with how humans learn and understand. I believe the joint XYZ-code is a foundational component of this aspiration, if grounded with external knowledge sources in the downstream AI tasks."
                        // "text": "（中央社紐約4日綜合外電報導）美國財經資訊公司彭博（Bloomberg）近日宣布研發自有聊天機器人BloombergGPT，盼推出專於金融領域的人工智慧（AI）資訊處理應用，以提供客戶和記者更好的功能與服務。美國新聞業網站尼曼實驗室（Nieman Lab）報導，彭博3月31日發表研究論文詳述BloombergGPT的開發。據彭博介紹，BloombergGPT是「一個新的大規模生成式AI模型。這個大型語言模型（LLM）專門鎖定範圍廣泛的金融資料來訓練生成，目的為了支援多元化的金融產業自然語言處理（NLP）任務集」。彭博表示，近期以大型語言模型為基礎的人工智慧發展，已在許多領域展示出令人振奮的新應用；但金融領域因其複雜性及具有專門術語，有必要有專屬模型。因此BloombergGPT的推出，代表將聊天機器人這項新科技開發應用到金融產業的第一步。彭博指出，BloombergGPT將協助其改善現有金融相關自然語言處理的任務，例如文本情感分析、命名實體辨識（NER）、新聞分類、回答問題和其他功能。此外，它也創造新機會來排列可從彭博終端機取得的巨量資料，以提供客戶更好的協助。至於BloombergGPT的訓練規模，彭博表示它的語料庫有7000億餘個token（字詞碎片）。相較之下，熱門聊天機器人ChatGPT的開發公司OpenAI在2020年推出的模型GPT-3，訓練的語料庫則約有5000億個token。根據彭博說法，BloombergGPT的語料庫中，有3630億個token取自彭博自有金融資料，也就是來自彭博終端機的資料庫，彭博號稱這是「至今最大的特定領域資料集（dataset）」；其餘3450億個token則是取自其他來源的通用資料集。彭博還說，訓練資料分為財金類FinPile和一般The Pile兩類。其中FinPile包括彭博檔案庫中的各類英文金融文件，如新聞文章、公告、新聞稿、網頁內容和社群媒體資料，以及彭博記者撰寫的新聞以外所有的新聞來源。至於ThePile則是龐雜的語料庫，來源從YouTube的畫面擷取、文藝數位化的古騰堡計畫（Project Gutenberg）到AI訓練常見的安隆公司（Enron）電郵快取。究竟BloombergGPT能夠如何應用？尼曼實驗室的文章表示，按照其訓練原理，它應該具有像ChatGPT的功能，但此外也能處理與彭博需求更相關的任務，例如將自然語言指令翻譯成彭博查詢語言（Bloomberg Query Language）終端機的使用者偏好功能。BloombergGPT也能為新聞文章提議具有彭博新聞風格的標題。彭博還說，BloombergGPT更能勝任回答與商業有關的提問，無論是有關文本情感分析、分類、資料擷取或任何其他任務。（譯者：張正芊/ 核稿：林治平）1120405"
                        "text": "美國新聞業網站尼曼實驗室（Nieman Lab）報導，彭博3月31日發表研究論文詳述BloombergGPT的開發。據彭博介紹，BloombergGPT是「一個新的大規模生成式AI模型。這個大型語言模型（LLM）專門鎖定範圍廣泛的金融資料來訓練生成，目的為了支援多元化的金融產業自然語言處理（NLP）任務集」。彭博表示，近期以大型語言模型為基礎的人工智慧發展，已在許多領域展示出令人振奮的新應用；但金融領域因其複雜性及具有專門術語，有必要有專屬模型。因此BloombergGPT的推出，代表將聊天機器人這項新科技開發應用到金融產業的第一步。彭博指出，BloombergGPT將協助其改善現有金融相關自然語言處理的任務，例如文本情感分析、命名實體辨識（NER）、新聞分類、回答問題和其他功能。此外，它也創造新機會來排列可從彭博終端機取得的巨量資料，以提供客戶更好的協助。至於BloombergGPT的訓練規模，彭博表示它的語料庫有7000億餘個token（字詞碎片）。相較之下，熱門聊天機器人ChatGPT的開發公司OpenAI在2020年推出的模型GPT-3，訓練的語料庫則約有5000億個token。根據彭博說法，BloombergGPT的語料庫中，有3630億個token取自彭博自有金融資料，也就是來自彭博終端機的資料庫，彭博號稱這是「至今最大的特定領域資料集（dataset）」；其餘3450億個token則是取自其他來源的通用資料集。彭博還說，訓練資料分為財金類FinPile和一般The Pile兩類。其中FinPile包括彭博檔案庫中的各類英文金融文件，如新聞文章、公告、新聞稿、網頁內容和社群媒體資料，以及彭博記者撰寫的新聞以外所有的新聞來源。至於ThePile則是龐雜的語料庫，來源從YouTube的畫面擷取、文藝數位化的古騰堡計畫（Project Gutenberg）到AI訓練常見的安隆公司（Enron）電郵快取。究竟BloombergGPT能夠如何應用？尼曼實驗室的文章表示，按照其訓練原理，它應該具有像ChatGPT的功能，但此外也能處理與彭博需求更相關的任務，例如將自然語言指令翻譯成彭博查詢語言（Bloomberg Query Language）終端機的使用者偏好功能。BloombergGPT也能為新聞文章提議具有彭博新聞風格的標題。彭博還說，BloombergGPT更能勝任回答與商業有關的提問，無論是有關文本情感分析、分類、資料擷取或任何其他任務。（譯者：張正芊/ 核稿：林治平）1120405"
                    }
                ]
            },
            "tasks": [
                {
                    "kind": "ExtractiveSummarization",
                    "taskName": "Document Extractive Summarization Task 1",
                    "parameters": {
                        "sentenceCount": 5
                    }
                }
            ]
        },
        responseType: 'json'
    }).then( async function (response) {
        //由於摘要服務有可能需要一些時間處理(依輸入文字長度而定)，稍後一會才向Azure取回結果
        await sleep(5000);
        getResult(response.headers["operation-location"]);
    })
}

//暫緩呼叫用
function sleep(ms) {
    return new Promise((resolve) => {
        setTimeout(resolve, ms);
    });
}

//取回結果
function getResult(endpoint){
    console.log(endpoint);
    axios({
        baseURL: endpoint,
        method: 'get',
        headers: {
            'Ocp-Apim-Subscription-Key': apiKey,
            'Content-type': 'application/json'
        },
        responseType: 'json'
    }).then(function (response) {
        let resultSentences = response.data.tasks.items[0].results.documents[0].sentences;
        for(let x=0; x<resultSentences.length;x++){
            //這邊可視判斷結果調整，目前設定為，只顯示分數高於0.8的句子
            if (resultSentences[x].rankScore >= 0.8){
                console.log(`${resultSentences[x].text} Score:${resultSentences[x].rankScore}`);
            }   
        }
    })
}