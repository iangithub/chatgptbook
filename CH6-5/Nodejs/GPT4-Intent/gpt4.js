const axios = require('axios');

//API授權金鑰
const api_Key = "{aoai_key}";
//AOAI服務名稱
const aoai_Service_Name = "{aoai_service_name}";
//AOAI部署名稱
const deployment_Name = "{aoai_deploy_name}";
//API版本
const api_Version = "2023-03-15-preview";
//使用 Chat Completions API 搭配 GPT-4 模型
const api_Endpoint = `https://${aoai_Service_Name}.openai.azure.com/openai/deployments/${deployment_Name}/chat/completions?api-version=${api_Version}`;


//Azure Question Ansering相關參數值
const az_QuestionAnsering_Credential = "{qa_api_key}";
const az_QuestionAnsering_ServiceName = "{qa_service_name}";
const az_QuestionAnsering_ProjectName = "{qa_projecnt_name}";
const az_QuestionAnsering_DeploymentName = "{qa_deploy_name}";
const az_QuestionAnsering_Endpoint = `https://${az_QuestionAnsering_ServiceName}.cognitiveservices.azure.com/language/:query-knowledgebases?
projectName=${az_QuestionAnsering_ProjectName}&api-version=2021-10-01&deploymentName=${az_QuestionAnsering_DeploymentName}`;

let userMsg ="停車場在哪裡？怎麼收費？";
let qa_Response ="";

//用於Azure Question Answering API的請求內容
const questionAnsweringPayload = {
  question: userMsg, //要查詢知識庫的使用者問題
  top: 1, //要針對問題傳回的答案數目上限。
  confidenceScoreThreshold: 0.7, //答案的最小閾值分數，值範圍從 0 到 1。
  includeUnstructuredSources: true //啟用非結構化來源的查詢。
}

axios.post(az_QuestionAnsering_Endpoint, questionAnsweringPayload, {
  headers: {
    'Ocp-Apim-Subscription-Key': az_QuestionAnsering_Credential,
    'Content-Type': 'application/json'
  }
})
  .then(response => {
    const completion = response.data;
    if (completion.answers && completion.answers.length > 0) {
      qa_Response = completion.answers[0].answer;
      console.log("============== qa_Response ====================");
      console.log(qa_Response);
      //呼叫GPT-4模型
      getGpt4Response(qa_Response).catch(error => {
        console.error("Error in getGpt4Response:", error);
      });
    } else {
      console.log("No answers found");
    }
  })
  .catch(error => {
    console.error(error);
  });


  async function getGpt4Response(qa_Response) {
    const requestModel = {
      temperature: 0.5,
      max_tokens: 500,
      messages: [
        { role: 'system', content: '你是一位客服人員，我會提供給你要回答客戶的答案，請你進行內容文字的修飾調整並以客服語氣產生回答內容' },
        { role: 'user', content: qa_Response },
      ],
    };
  
    try {
      const response = await axios.post(api_Endpoint, requestModel, {
        headers: {
          'api-key': api_Key,
          'Content-Type': 'application/json'
        }
      });
  
      const completion = response.data;
      console.log("============== GPT-4_Response ====================");
      console.log(completion.choices[0].message.content);
    } catch (error) {
      console.error(error);
    }
  }
  

