const axios = require('axios');

const api_Key = '{aoai_key}';
const aoai_Service_Name = '{aoai_service_name}';
const deployment_Name = '{aoai_deploy_name}';
const api_Version = '2023-03-15-preview';

//使用 Chat Completions API 搭配 GPT-4 模型
const api_Endpoint = `https://${aoai_Service_Name}.openai.azure.com/openai/deployments/${deployment_Name}/chat/completions?api-version=${api_Version}`;

const requestModel={};


requestModel.temperature = 0.2;
requestModel.max_tokens = 1000; //限制 token 上限數
requestModel.messages = [
  { role: 'system', content: `你是一個服務自動預約系統，請協助自動分析客人的服務需求，包含 intent 及 entities ，並且使用JSON格式輸出回應，JSON屬性用英文
  JSON格式為
  {
  ""intent"":""xxxx"",
  ""entities"":[{"""",""""},{"""",""""}]
  }` },
  { role: 'user', content: '預約接駁車，從火車站到飯店，有3個大人，2個小孩，5件行李'}
];

axios.post(api_Endpoint, requestModel, {
  headers: {
    'api-key': api_Key,
    'Content-Type': 'application/json'
  }
})
  .then(response => {
    const completion = response.data;
    console.log(completion.choices[0].message.content);
  })
  .catch(error => {
    console.error(error);
  });
