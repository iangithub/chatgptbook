'use strict';
//載入express建構網路應用程式
const express = require('express');
//載入config以利將密碼與金鑰放置於獨立檔案中
const config = require('config');
//載入axios來進行資源請求
const axios = require('axios');

//OpenAI需要的驗證資訊
const endpoint = config.get("ENDPOINT");
const apiKey = config.get("OpenAI_API_KEY");
const modelName = config.get("DeploymentName");

// create Express app
// about Express itself: https://expressjs.com/
const app = express();

// listen on port
const port = process.env.port || process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`listening on ${port}`);
    //在這裡輸入想要產生的故事關鍵字
    let userText = "bear";
    //將故事關鍵字送給OpenAI_Analysis函數
    OpenAI_Analysis(userText);
});

function OpenAI_Analysis(userText) {
    console.log("[OpenAI_Analysis] in");
    //開始加工使用者的輸入
    let addPrePrompt = "Can yo make a story about ";
    let addPostPrompt = " Keep it short, around 100 words."
    //準備送出請求
    let axios_setting_data = {
        method: "POST",
        url: endpoint + "/openai/deployments/" + modelName + "/completions?api-version=2022-12-01",
        headers: {
            "api-key": apiKey,
            "Content-Type": "application/json"
        },
        data: {
            "prompt": addPrePrompt + userText + "?" + addPostPrompt,
            "max_tokens": 300
        }
    };
    //發出請求
    axios(axios_setting_data)
        .then(function (response) {
            //成功的話，將生成文字送入Text2Speech函數
            let returnText = response.data.choices[0].text;
            console.log(returnText);
            Text2Speech(returnText);
        })
        .catch(function (error) {
            console.log("Error : ", error);
        });
}

function Text2Speech(userInput) {
    //載入Azure Speech Service
    var sdk = require("microsoft-cognitiveservices-speech-sdk");
    //設定稍後新增的聲音檔案名稱
    var audioFile = "YourAudioFile.wav";
    //準備Speech Service所需資訊
    const speechConfig = sdk.SpeechConfig.fromSubscription(config.get("SPEECH_KEY"), config.get("SPEECH_REGION"));
    const audioConfig = sdk.AudioConfig.fromAudioFileOutput(audioFile);
    //選擇語音角色
    speechConfig.speechSynthesisVoiceName = "en-US-AmberNeural";
    //開始文字轉語音
    var synthesizer = new sdk.SpeechSynthesizer(speechConfig, audioConfig);
    synthesizer.speakTextAsync(userInput,
        function (result) {
            if (result.reason === sdk.ResultReason.SynthesizingAudioCompleted) {
                //順利完成後，在console中顯示完成
                console.log("synthesis finished.");
            } else {
                //有問題會來這裡
                console.error("Speech synthesis canceled, " + result.errorDetails +
                    "\nDid you set the speech resource key and region values?");
            }
            synthesizer.close();
            synthesizer = null;
        },
        function (err) {
            console.trace("err - " + err);
            synthesizer.close();
            synthesizer = null;
        });
    console.log("Now synthesizing to: " + audioFile);
}
