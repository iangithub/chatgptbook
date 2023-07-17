
public class ResponseModel
{
    public string kind { get; set; }
    public Result result { get; set; }
}

public class Result
{
    public string query { get; set; }
    public Prediction prediction { get; set; }
}

public class Prediction
{
    public string topIntent { get; set; }
    public string projectKind { get; set; }
    public Intent[] intents { get; set; }
    public Entity[] entities { get; set; }
}

public class Intent
{
    public string category { get; set; }
    public int confidenceScore { get; set; }
}

public class Entity
{
    public string category { get; set; }
    public string text { get; set; }
    public int offset { get; set; }
    public int length { get; set; }
    public int confidenceScore { get; set; }
    public List<ExtraInformation> extraInformation { get; set; }

}

public class ExtraInformation
{
    public string extraInformationKind { get; set; }
    public string key { get; set; }
}