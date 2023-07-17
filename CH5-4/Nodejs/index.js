'use strict';
// 使用express框架來作為Node程式碼基本架構，方便未來改寫成網站或Chatbot
const express = require('express');
// 使用config來管理密碼、API Key
const config = require('config');
// 使用axios來介接Azure AI的API
const axios = require('axios').default;
// 隨機產生一組識別碼，在介接Azure翻譯服務時需要用到
const { v4: uuidv4 } = require('uuid');

//Azure Key - 建立config資料夾，在裡面新增default.json檔案
//裡面寫上{ "Translator_Key":"XXXXXXX","Translator_Endpoint":"XXXXXX"}
//填上在Azure申請到的API Key，與Web API文字翻譯的端點網址
let key = config.get("Translator_Key");
let endpoint = config.get("Translator_Endpoint");
//請確認是否與申請的翻譯服務區域相同
let location = "eastus";

//開始建立Express應用程式
const app = express();

//決定伺服器的Port號碼，為了相容於本地端測試與未來雲端部署，判斷是否有環境變數的設定，若沒有則給予固定值
const port = process.env.port || process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`listening on ${port}`);
    //伺服器啟動之後，就跳進這個函數開始準備呼叫翻譯服務
    MS_Translator()
        .catch((err) => {
            console.error("Error:", err);
        });
});

async function MS_Translator() {
    console.log("[MS_Translator] in");
    //準備三句翻譯測試句子，一句中文，一句英文，一句其他國家語言
    // let translator_input = "Ryan, you are so handsome!";
    // let translator_input = "萊恩, 你太帥了！";
    let translator_input = "ライアン、あなたはとてもハンサムです！";
    let resultString = "";
    axios({
        baseURL: endpoint,
        url: '/translate',
        method: 'post',
        headers: {
            'Ocp-Apim-Subscription-Key': key,
            'Ocp-Apim-Subscription-Region': location,
            'Content-type': 'application/json',
            'X-ClientTraceId': uuidv4().toString()
        },
        params: {
            'api-version': '3.0',
            'to': ['en','zh-Hant']
        },
        data: [{
            'text': translator_input
        }],
        responseType: 'json'
    }).then(function (response) {
        //取得Azure翻譯服務偵測到的語言是哪一種，對應提供翻譯結果
        //中文->英文、英文->中文、其他語言->中文與英文
        if (response.data[0].detectedLanguage.language == "zh-Hans" || 
            response.data[0].detectedLanguage.language == "zh-Hant") {
            resultString += response.data[0].translations[0].text;
        } else if (response.data[0].detectedLanguage.language == "en") {
            resultString += response.data[0].translations[1].text;
        } else {
            resultString += response.data[0].translations[0].text;
            resultString += "\n";
            resultString += response.data[0].translations[1].text;
        }
        // 在console輸出結果
        console.log(resultString);
    })
}