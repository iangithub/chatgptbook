// See https://aka.ms/new-console-template for more information


namespace SemanticKernelTutorial;


using Microsoft.SemanticKernel; // 0.14.547.1-preview

using Microsoft.SemanticKernel.Connectors.Memory.Qdrant;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;



class Program
{
    //Microsoft.SemanticKernel 的版本為 0.14.547.1-preview
    
    private const string MemoryCollectionName = "qdrant-test";
    private const string TextCompletionDeploymentName = "gpt-35-turbo";
    private const string TextEmbeddingDeploymentName = "text-embedding-ada-002";
    private const string AzureOpenAIEndpoint = "https://your-endpoint.openai.azure.com/";
    private const string AzureOpenAIKey = "your-key";
    private const string QdrantEndpoint = "http://localhost";

    
    static async Task Main(string[] args)
    {
        Console.WriteLine("== Hello, Embedding! ==");
        
        // 建立向量資料庫 Qdrant 的 Memory Store
        QdrantMemoryStore memoryStore = new QdrantMemoryStore(QdrantEndpoint, 6333, vectorSize: 5566);
        
        //建立 Kernel，並把ChatCompletionService 與 TextEmbeddingGenerationService 加入
        IKernel kernel = Kernel.Builder
            .Configure(c =>
            {
                c.AddAzureChatCompletionService(deploymentName: TextCompletionDeploymentName, endpoint: AzureOpenAIEndpoint, apiKey: AzureOpenAIKey);
                c.AddAzureTextEmbeddingGenerationService(deploymentName: TextEmbeddingDeploymentName, endpoint: AzureOpenAIEndpoint, apiKey: AzureOpenAIKey);
            })
            .WithMemoryStorage(memoryStore)
            .Build();
        
        
        Console.WriteLine("== 印出 Vector DB 中有哪些 collections ==");
        var collections = memoryStore.GetCollectionsAsync();
        await foreach (var collection in collections)
        {
            Console.WriteLine(collection);
        }

        Console.WriteLine("== 把法條加入 Qdrant Memory 中 ==");

        var key1 = await kernel.Memory.SaveInformationAsync(MemoryCollectionName, id: "第1條", 
            text: "為管理不動產經紀業（以下簡稱經紀業），建立不動產交易秩序，保障交易者權益，促進不動產交易市場健全發展，特制定本條例。");
        var key2 = await kernel.Memory.SaveInformationAsync(MemoryCollectionName, id: "第2條", 
            text: "經紀業之管理，依本條例之規定；本條例未規定者，適用其他有關法律之規定。");
        var key3 = await kernel.Memory.SaveInformationAsync(MemoryCollectionName, id: "第4條", 
            text: "本條例用辭定義如下︰ 一、不動產︰指土地、土地定著物或房屋及其可移轉之權利；房屋指成屋、預售屋及其可移轉之權利。 二、成屋︰指領有使用執照，或於實施建築管理前建造完成之建築物。 三、預售屋︰指領有建造執照尚未建造完成而以將來完成之建築物為交易標的之物。 四、經紀業︰指依本條例規定經營仲介或代銷業務之公司或商號。 五、仲介業務︰指從事不動產買賣、互易、租賃之居間或代理業務。 六、代銷業務︰指受起造人或建築業之委託，負責企劃並代理銷售不動產之業務。 七、經紀人員︰指經紀人或經紀營業員。經紀人之職務為執行仲介或代銷業務；經紀營業員之職務為協助經紀人執行仲介或代銷業務。 八、加盟經營者︰經紀業之一方以契約約定使用他方所發展之服務、營運方式、商標或服務標章等，並受其規範或監督。 九、差價︰係指實際買賣交易價格與委託銷售價格之差額。 十、營業處所︰指經紀業經營仲介或代銷業務之店面、辦公室或非常態之固定場所。 ");
        var key4 = await kernel.Memory.SaveInformationAsync(MemoryCollectionName, id: "第13條",
            text: "前條第一項經紀人證書有效期限為四年，期滿時，經紀人應檢附其於四年內在中央主管機關認可之機構、團體完成專業訓練三十個小時以上之證明文件，向直轄市或縣（市）政府辦理換證。");

        // 查詢已經存進去的資料
        // Console.WriteLine("== 取得已經存入 Qdrant 的資料 ==");
        // MemoryQueryResult? lookup = await kernel.Memory.GetAsync(MemoryCollectionName, "cat1");
        // Console.WriteLine(lookup != null ? lookup.Metadata.Text : "ERROR: memory not found");
        
        
        var question = "請問不動產的定義是什麼？";
        Console.WriteLine("== 開始問不動產經紀業法規的問題： {0}==", question);
        
        // 相關度和答案數目的參數都可以調整。
        var searchResults = kernel.Memory.SearchAsync(MemoryCollectionName, question, limit: 1, minRelevanceScore: 0.7);

        IChatCompletion chatGPT = kernel.GetService<IChatCompletion>();
        var chatHistory = (OpenAIChatHistory)chatGPT.CreateNewChat(question);

        
        await foreach (var item in searchResults)
        {
            Console.WriteLine("== 印出透過 Embedding 查詢到的資料，與相關程度 ==");
            Console.WriteLine(item.Metadata.Text + " : " + item.Relevance);

            
            //把查詢到的資料加入到 chatHistory 中
            chatHistory.AddSystemMessage(item.Metadata.Text);
            string reply = await chatGPT.GenerateMessageAsync(chatHistory);
            Console.WriteLine("== 印出透過 ChatGPT 修飾過後的答案 ==");

            Console.WriteLine(reply);
        }

        
        // Console.WriteLine("== 移除 Collection {0} ==", MemoryCollectionName);
        // await memoryStore.DeleteCollectionAsync(MemoryCollectionName);

    }
}