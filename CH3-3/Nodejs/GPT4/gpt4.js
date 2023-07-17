const axios = require('axios');

const api_Key = '{aoai_key}';
const aoai_Service_Name = '{aoai_service_name}';
const deployment_Name = '{aoai_deploy_name}';
const api_Version = '2023-03-15-preview';

//使用 Chat Completions API 搭配 GPT-4 模型
const api_Endpoint = `https://${aoai_Service_Name}.openai.azure.com/openai/deployments/${deployment_Name}/chat/completions?api-version=${api_Version}`;

const requestModel={};


requestModel.temperature = 0.5;
requestModel.max_tokens = 1000;
requestModel.messages = [
  { role: 'system', content: '現在開始你是一位專欄作家。' },
  { role: 'user', content: '請你以 ### ChatGPT在教學領域的應用 ### 為主題，並使用繁體中文創作一篇文章' }
];


//村上春樹風格-內容改寫優化
// requestModel.temperature = 0.9;
// requestModel.max_tokens = 2000;
// requestModel.messages = [
//   { role: 'system', content: '現在開始你是一位專欄作家。' },
//   { role: 'user', content: '請你閱讀###內的文章的寫作風格  ###  你要做一個不動聲色的大人了。不准情緒化，不准偷偷想念，不准回頭看。去過自己另外的生活。  就經驗性來說，人強烈追求什麼的時候，那東西基本上是不來的，而當你極力迴避它的時候，它卻自然找到頭上。  不管全世界所有人怎麼說，我都認為自己的感受才是正確的。 無論別人怎麼看，我絕不打亂自己的節奏。 喜歡的事自然可以堅持，不喜歡怎麼也長久不了。  哪裡有人喜歡孤獨，只不過不亂交朋友罷了，那樣只能落得失望。  剛剛好，看見你幸福的樣子，於是幸福著你的幸福。  我們的人生，在那之間有所謂陰影的中間地帶。能夠認識那陰影的層次，並去理解它，才是健全的知性。  我認為我的工作是觀察人和世界，而不是去評判他們。我一直希望自己遠離所謂的結論，我想讓世界一切都敞開懷抱  歸根結底，一個人是否有可能對另一個人有完美的理解？我們可以花費大量時間和精力去結識另一個人，但最後，我們可以接近那個人的本質嗎？我們說服自己，我們很了解對方，但是我們真的了解任何人的重要本質嗎？  ###    請你使用相同的寫作風格，對下面的文章進行改寫，並且產生500個字以內的內容，使用繁體中文    ###  隨著科技的進步，人工智能（AI）在各個領域中的應用越來越廣泛，教育領域也不例外。近年來，ChatGPT（生成式對話機器人）已經在教學領域中發揮了重要作用，從而為教師和學生提供了更多的學習機會和資源。本文將探討ChatGPT在教育領域的幾個主要應用，以及它如何改變了當代教育的面貌。    1. 輔助教學    ChatGPT可作為教師的助手，協助他們解答學生的疑問。這樣一來，教師便能專注於教授課程內容，同時保證每位學生都能得到足夠的關注。此外，ChatGPT具有自然語言處理（NLP）功能，能夠理解並回答各種問題，有助於學生在課堂以外的時間獲得即時反饋。    2. 個性化學習    ChatGPT具有強大的學習能力，能夠根據每個學生的需求和興趣提供個性化的學習計劃。這意味著學生可以在自己的節奏下學習，避免了因跟不上課程進度而感到沮喪的情況。此外，ChatGPT還能夠根據學生的學習情況給出建議，幫助他們在學習過程中取得更好的成果。    3. 語言學習助手    對於正在學習外語的學生，ChatGPT可以作為一個出色的語言學習助手。它可以與學生進行即時對話，幫助他們練習口語和聽力技能。此外，ChatGPT還  能提供寫作建議，協助學生改進他們的寫作技巧。這樣的互動式學習方式對於提高學生的語言水平具有很大的幫助。    4. 在線評估與測試    ChatGPT可以自動生成各種題型的試題，為教師提供了一個簡單而有效的評估工具。這不僅可以節省教師編制試題的時間，還能確保試題的多樣性和客觀性。此外，ChatGPT還能夠進行自動評分，為教師提供及時的學生表現反饋。  ###      ' }
// ];

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
