
public class ResponseModel
{
    public string kind { get; set; }
    public Results results { get; set; }
}

public class Results
{
    public List<ResultDocument> documents { get; set; }
    public object[] errors { get; set; }
    public string modelVersion { get; set; }
}

public class ResultDocument
{
    public List<Entity> entities { get; set; }
    public string id { get; set; }
    public object[] warnings { get; set; }
}

public class Entity
{
    public string category { get; set; }
    public float confidenceScore { get; set; }
    public int length { get; set; }
    public int offset { get; set; }
    public string text { get; set; }
    public string subcategory { get; set; }
}
