'use strict';
const express = require('express'),
    configGet = require('config');
//載入axios來進行資源請求
const axios = require('axios');

//OpenAI需要的驗證資訊
const endpoint_openai = configGet.get("OPENAI_ENDPOINT");
const apiKey_openai = configGet.get("OpenAI_API_KEY");
const modelName = configGet.get("DeploymentName");
//載入情緒分析
const { TextAnalyticsClient, AzureKeyCredential } = require("@azure/ai-text-analytics");

//Azure 情緒分析需要的驗證資訊
const endpoint = configGet.get("ENDPOINT");
const apiKey = configGet.get("TEXT_ANALYTICS_API_KEY");

const app = express();

const port = process.env.PORT || process.env.port || 3000;

app.listen(port, () => {
    console.log(`listening on ${port}`);
    // 準備幾則測試留言
    // let userInput = "早餐不好吃";
    // let userInput = "櫃檯人員很親切";
    let userInput = "房間陳設普通";
    // 呼叫文字情緒分析服務
    MS_TextSentimentAnalysis(userInput);
});

async function MS_TextSentimentAnalysis(userInput) {
    console.log("[MS_TextSentimentAnalysis] in");
    const analyticsClient = new TextAnalyticsClient(endpoint, new AzureKeyCredential(apiKey));
    let documents = [];
    documents.push(userInput);
    const results = await analyticsClient.analyzeSentiment(documents, "zh-Hant", {
        includeOpinionMining: true //包含具體對象挖掘
    });
    //將Azure的情緒分析分類對應成中文
    let sentiments = ["negative","neutral","positive"];
    let sentimenst_tw = ["負向","中性","正向"];
    let resultString = "";
    for(var x=0; x<sentiments.length;x++){
        if(results[0].sentiment == sentiments[x]){
            resultString += sentimenst_tw[x];
        }
    }
    //如果有拿到目標(主詞)，就也記錄下來
    if(results[0].sentences[0].opinions.length!=0){
        resultString += "," + results[0].sentences[0].opinions[0].target.text;
    }

    console.log(resultString);
    //送往Azure OpenAI
    GPT_Analysis(resultString);
}

function GPT_Analysis(resultString){
    //取出情緒分析分類與目標(主詞)
    let sentiment = resultString.split(",")[0];
    let target = "";
    if (resultString.split(",").length > 1){
        target = resultString.split(",")[1];
    }
    console.log("[OpenAI_Analysis] in");
    //開始加工使用者的輸入
    let addPrePrompt = "你是一名旅館經理，現在顧客的情緒是"+sentiment;
    if(target != "" && target != undefined){
        addPrePrompt += ", 顧客這樣的情緒是來自於旅館的"+target;
    }
        addPrePrompt += "。請具體針對顧客提出的主題("+target+")，說一段話來回應顧客，不用太長 : ";
    console.log("[addPrePrompt] ", addPrePrompt);
    //準備送出請求
    let axios_setting_data = {
        method: "POST",
        url: endpoint_openai + "/openai/deployments/" + modelName + "/completions?api-version=2022-12-01",
        headers: {
            "api-key": apiKey_openai,
            "Content-Type": "application/json"
        },
        data: {
            "prompt": addPrePrompt,
            "max_tokens": 800,
            // "stop":4
        }
    };
    //發出請求
    axios(axios_setting_data)
        .then(function (response) {
            let returnText = response.data.choices[0].text;
            //去除回應內容前面的空行
            console.log(returnText.replace(/\n\n/, ''));
        })
        .catch(function (error) {
            console.log("Error : ", error);
        });
}