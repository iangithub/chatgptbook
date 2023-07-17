
    public class RequestModel
    {
        public string displayName { get; set; }
        public Analysisinput analysisInput { get; set; }
        public List<Task> tasks { get; set; }

        public RequestModel()
        {
            displayName = "Document ext Summarization Task Example";
            analysisInput = new Analysisinput();
            tasks = new List<Task> { new Task()
        {
            kind = "ExtractiveSummarization",
            taskName = "Document Extractive Summarization Task 1",
            parameters = new Parameters() { sentenceCount = 5 }
        }};
        }
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

    public class Task
    {
        public string kind { get; set; }
        public string taskName { get; set; }
        public Parameters parameters { get; set; }
    }

    public class Parameters
    {
        public int sentenceCount { get; set; }
    }
