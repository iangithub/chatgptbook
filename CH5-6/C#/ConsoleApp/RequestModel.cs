
public class RequestModel
{
    public string kind { get; set; }
    public Parameters parameters { get; set; }
    public Analysisinput analysisInput { get; set; }

    public RequestModel()
    {
        kind = "EntityRecognition";
        parameters=new Parameters() { modelVersion = "latest" };
        analysisInput = new Analysisinput();
    }
}

public class Parameters
{
    public string modelVersion { get; set; }
}

public class Analysisinput
{
    public List<Document> documents { get; set; }

    public Analysisinput()
    {
        documents = new List<Document>();
    }
}

public class Document
{
    public string id { get; set; }
    public string language { get; set; }
    public string text { get; set; }
}
