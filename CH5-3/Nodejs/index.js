'use strict';
// 使用express框架來作為Node程式碼基本架構，方便未來改寫成網站或Chatbot
const express = require('express');
// 使用config來管理密碼、API Key
const config = require('config');
// 帶入Azure Text Analytics SDK
const { TextAnalyticsClient, AzureKeyCredential } = require("@azure/ai-text-analytics");

//Azure Text Sentiment 要用到的端點網址與API Key
const endpoint = config.get("ENDPOINT");
const apiKey = config.get("TEXT_ANALYTICS_API_KEY");

//開始建立Express應用程式
const app = express();

//決定伺服器的Port號碼，為了相容於本地端測試與未來雲端部署，判斷是否有環境變數的設定，若沒有則給予固定值
const port = process.env.port || process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`listening on ${port}`);
    //伺服器啟動之後，就跳進這個函數開始準備呼叫文字情緒分析服務
    MS_TextSentimentAnalysis()
    .catch((err) => {
        console.error("Error:", err);
    });
});

async function MS_TextSentimentAnalysis() {
    console.log("[MS_TextSentimentAnalysis] in");
    // 呼叫文字情緒分析服務
    const analyticsClient = new TextAnalyticsClient(endpoint, new AzureKeyCredential(apiKey));
    // 在此輸入要測試的評論留言，可指定語系(繁體中文zh-hant、英文en)
    const thisReview = {
        text: "熱水一點都不熱，洗到冷水澡！真令人生氣！",
        id: "0",
        language: "zh-hant"
    };
    // const thisReview2 = {
    //     text: "我喜歡這間飯店，櫃檯人員十分客氣！房間內也相當整潔、寬敞。",
    //     id: "1",
    //     language: "zh-hant"
    // };
    // const thisReview = {
    //     text: "The food and service were unacceptable. The concierge was nice, however.",
    //     id: "0",
    //     language: "en"
    // };
    let documents = [];
    documents.push(thisReview.text);
    // results會拿到文字情緒分析服務回傳的結果
    const results = await analyticsClient.analyzeSentiment(documents, documents[0].language, {
        includeOpinionMining: true,
    });
    console.log("[results] ", JSON.stringify(results));
    // 取出回傳結果中的sentiment(positive正向、neutral中性、negative負向)
    let resultString = "";
    if (results[0].sentiment == "positive") {
        resultString += "正向。";
        resultString += "分數：" + results[0].confidenceScores.positive;
    } else if (results[0].sentiment == "neutral") {
        resultString += "中性。";
        resultString += "分數：" + results[0].confidenceScores.neutral;
    } else {
        resultString += "負向。";
        resultString += "分數：" + results[0].confidenceScores.negative;
    }
    // 在console輸出結果
    console.log(resultString);
}
