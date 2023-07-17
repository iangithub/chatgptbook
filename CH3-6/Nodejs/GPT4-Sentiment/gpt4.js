const axios = require('axios');

const api_Key = '{aoai_key}';
const aoai_Service_Name = '{aoai_service_name}';
const deployment_Name = '{aoai_deploy_name}';
const api_Version = '2023-03-15-preview';

//使用 Chat Completions API 搭配 GPT-4 模型
const api_Endpoint = `https://${aoai_Service_Name}.openai.azure.com/openai/deployments/${deployment_Name}/chat/completions?api-version=${api_Version}`;

const requestModel={};


requestModel.temperature = 0.2;
requestModel.max_tokens = 1000;
requestModel.messages = [
  { role: 'system', content: `你是一個客服系統，我需要你自動分析客人反應問題的整體情緒，
  分析結果以正面及負面做為表示，只要給出最終整體情緒是偏正面或負面就好，不用逐句分析，
  並且能夠分析客人主要的訴求是什麼，做50個字以內的中文摘要整理，
  分析的結果以JSON格式輸出，{
  ""emotions"":"""",""summary"""":""""""""
  }。` },
  { role: 'user', content: `上個月入住你們飯店，結果房間都是煙味，
  反應給櫃檯也只是得到了會再加強清潔的回答，
  完全沒有幫我們解決問題，早餐的選擇也很少，份量又不足，食材都是冷掉的，
  下次不會再來了` }
];


//sample 2
// requestModel.messages = [
//   { role: 'system', content: `你是一個客服系統，我需要你自動分析客人反應問題的整體情緒，
//   分析結果以正面及負面做為表示，只要給出最終整體情緒是偏正面或負面就好，不用逐句分析，
//   並且能夠分析客人主要的訴求是什麼，做50個字以內的中文摘要整理，
//   分析的結果以JSON格式輸出，{
//   ""emotions"":"""",""summary"""":""""""""
//   }。` },
//   { role: 'user', content: `一次完美的住宿體驗
//   訂房處理快速有效率
//   特殊需求也安排的很好
//   一進大廳就感覺到前衛的設計感但整體又非常舒適
//   櫃檯人員服務親切，並且不是只有一兩位令人感受很好
//   大概五位工作人員讓人感到很舒服放鬆
//   非常值得全體加薪！
//   房間擺設也令人很滿意
//   浴室很大，毛巾很舒服，沒有噁心的化學味～
//   而且浴缸的水很快就滿了
//   洗了舒服的熱水澡！
//   跟櫃檯要了兩瓶礦泉水
//   不只送來的速度很快，送來的工作人員也是很親切
//   隔天因為還要帶朋友在高雄玩，所以行李寄放在櫃檯
//   工作人員很自然地跟外國朋友介紹門口的船啊船錨等等，還熱心的幫我們拍了照
//   如果有任何人要我推薦高雄住宿的話
//   絕對是以專業的角度首選Indigo
//   光是訓練有素的所有接待工作人員
//   就非常值得再訪` }
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
