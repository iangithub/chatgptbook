'use strict';

const express = require('express'),
    axios = require('axios');

const app = express();

const port = process.env.PORT || process.env.port || 3000;

app.listen(port, () => {
    console.log(`listening on ${port}`);
    LuisAnalysis("我想要關燈");
});

function LuisAnalysis(userInput) {
    console.log("[LuisAnalysis] in");
    let endpoint = "https://{your_service_name}.cognitiveservices.azure.com";
    let axios_setting_data = {
        method: "POST",
        url: endpoint + "/language/:analyze-conversations?api-version=2022-10-01-preview",
        headers: {
            "Ocp-Apim-Subscription-Key": "{api_key}",
            "Apim-Request-Id": "{apim-request-id}",
            "Content-Type": "application/json"
        },
        data: {
            kind: "Conversation",
            analysisInput: {
                conversationItem: {
                    id: "1",
                    participantId: "1",
                    modality: "text",
                    text: userInput
                }
            },
            parameters: {
                projectName: "SmartHome-YYYYMMDD",
                verbose: true,
                deploymentName: "clu-yyyymmdd-deploy",
                stringIndexType: "TextElement_V8"
            }
        }
    };

    axios(axios_setting_data)
        .then(function (response) {
            console.log(JSON.stringify(response.data));
            let thisResult = response.data.result;
            //result handling
            let returnText = "";
            returnText += "Intent : ";
            returnText += thisResult.prediction.topIntent;
            returnText += "\n";
            returnText += thisResult.prediction.entities[0].category;
            returnText += " : ";
            returnText += thisResult.prediction.entities[0].extraInformation[0].key;

            console.log(returnText); 
        })
        .catch(function (error) {
            console.log("Error : ", error);
        });
}