using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
// dotnet add package HtmlAgilityPack
// dotnet add package Newtonsoft.Json


class Program
{
    static void Main()
    {
        var url = "https://law.moj.gov.tw/LawClass/LawAll.aspx?pcode=D0060066";
        var httpClient = new HttpClient();
        var html = httpClient.GetStringAsync(url).Result;
        
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var rows = htmlDocument.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "") == "row");
        var data = new List<object>();

        foreach (var row in rows)
        {
            var colNo = row.SelectSingleNode(".//div[@class='col-no']");
            var lawArticle = row.SelectSingleNode(".//div[@class='law-article']");

            if (colNo != null && lawArticle != null)
            {
                var colNoText = colNo.InnerText.Trim();
                var articleLines = lawArticle.Descendants("div");
                var lines = string.Join(" ", articleLines.Select(line => line.InnerText));

                // 遵照 Azure OpenAI 的 Fine-tuning 格式
                data.Add(new { prompt = colNoText, completion = lines });
            }
        }

        // 將資料存成 JSON 檔
        var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText("raw_data.json", jsonData);
    }
}