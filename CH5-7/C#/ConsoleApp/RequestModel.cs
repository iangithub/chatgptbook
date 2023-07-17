
public class RequestModel
{
    public string kind { get; set; }
    public Analysisinput analysisInput { get; set; }
    public Parameters parameters { get; set; }

    public RequestModel()
    {
        kind = "Conversation";
        analysisInput = new Analysisinput();
        parameters = new Parameters() { stringIndexType = "TextElement_V8", verbose = true };
    }
}

public class Analysisinput
{
    public Conversationitem conversationItem { get; set; }
    public Analysisinput()
    {
        conversationItem = new Conversationitem();
    }
}

public class Conversationitem
{
    public string id { get; set; }
    public string participantId { get; set; }
    public string text { get; set; }
}

public class Parameters
{
    public string projectName { get; set; }
    public string deploymentName { get; set; }
    public string stringIndexType { get; set; }
    public bool verbose { get; set; }
}
