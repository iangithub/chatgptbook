const axios = require('axios');

const api_Key = '{aoai_key}';
const aoai_Service_Name = '{aoai_service_name}';
const deployment_Name = '{aoai_deploy_name}';
const api_Version = '2022-12-01';

//使用 Completions API 搭配 code-davinci-002 模型
const api_Endpoint = `https://${aoai_Service_Name}.openai.azure.com/openai/deployments/${deployment_Name}/completions?api-version=${api_Version}`;


const prompt_Template = '/* 使用C#程式語言建立一個以亂數產生的整數List，具有10個item，並且由小到大排序 */ using ';

const req_model = {
  prompt: prompt_Template,
  temperature: 0.2,
  best_of: 1,
  max_tokens: 1000
};

axios.post(api_Endpoint, req_model, {
  headers: {
    'api-key': api_Key,
    'Content-Type': 'application/json'
  }
})
  .then(response => {
    const completion = response.data;
    console.log(completion.choices[0].text);
  })
  .catch(error => {
    console.error(error);
  });
