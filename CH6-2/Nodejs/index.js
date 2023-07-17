// 載入OpenAI
const { Configuration, OpenAIApi } = require("openai");
// 載入config，用來存放密碼
const config = require("config");
// 準備OpenAI的連接設定
const configuration = new Configuration({
    apiKey: config.get("OPENAI_API_KEY")
});
const openai = new OpenAIApi(configuration);

// prompt : 輸入你想要產生圖像的文字
// n : 決定產生圖像的數量(1~10，預設值：1)
// size : 只有 256x256, 512x512, 1024x1024(預設值)這三種
async function getResult() {
    const response = await openai.createImage({
        prompt: "A cute cat looking at the camera, begging for food.",
        n:1,
        size: "1024x1024",
    });
    console.log(response.data);
}

getResult();