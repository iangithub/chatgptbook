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
    MS_NamedEntityRecognition()
        .catch((err) => {
            console.error("Error:", err);
        });
});

async function MS_NamedEntityRecognition() {
    console.log("[MS_NamedEntityRecognition] in");
    //使用axios套件來呼叫文字摘要API服務
    //data那裡放上想要摘要的文字並指定語言
    axios({
        baseURL: endpoint,
        url: 'language/:analyze-text?api-version=2022-05-01',
        method: 'post',
        headers: {
            'Ocp-Apim-Subscription-Key': apiKey,
            'Content-type': 'application/json',
        },
        params: {
        },
        data: {
            "kind": "EntityRecognition",
            "parameters": {
                "modelVersion": "latest"
            },
            "analysisInput": {
                "documents": [
                    {
                        "id": "1",
                        "language": "zh-Hans",
                        "text":"萊恩在4/17~4/22在美國微軟總部參加MVP Summit大會。"
                    }
                ]
            }
        },
        responseType: 'json'
    }).then(async function (response) {
        let entitiesArray = response.data.results.documents[0].entities;
        for(let x=0;x<entitiesArray.length;x++){
            console.log(`類別：${entitiesArray[x].category}、次類別：${entitiesArray[x].subcategory == undefined ? "無" : entitiesArray[x].subcategory}、內容：${entitiesArray[x].text}`);
        }
    })
}
