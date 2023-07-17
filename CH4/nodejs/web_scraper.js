const axios = require("axios");
const cheerio = require("cheerio");
const fs = require("fs");
// 請記得使用 npm install axios cheerio 來安裝套件


// 你可以把這個 URL 換成其他法律條文的網址
const url = "https://law.moj.gov.tw/LawClass/LawAll.aspx?pcode=D0060066";

axios
  .get(url)
  .then((response) => {
    const html = response.data;
    const $ = cheerio.load(html);
    const rows = $(".row");
    const data = [];

    rows.each((index, row) => {
      const colNo = $(row).find(".col-no");
      const lawArticle = $(row).find(".law-article");

      if (colNo && lawArticle) {
        const colNoText = colNo.text().trim();
        const articleLines = lawArticle.find("div");
        const lines = articleLines
          .map((index, line) => $(line).text())
          .get()
          .join(" ");

        data.push({ prompt: colNoText, completion: lines });
      }
    });

    // 將資料存成 JSON 檔
    fs.writeFileSync("raw_data.json", JSON.stringify(data, null, 2));
  })
  .catch((error) => {
    console.log(error);
  });
