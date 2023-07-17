using Newtonsoft.Json;


public class ApiRequestModelGpt4
{
    [JsonProperty(PropertyName = "messages")]
    public List<Message> Messages { get; private set; }

    [JsonProperty(PropertyName = "temperature")]
    public float Temperature { get; set; }

    [JsonProperty(PropertyName = "top_p")]
    public float Top_p { get; set; }

    [JsonProperty(PropertyName = "frequency_penalty")]
    public int Frequency_Penalty { get; set; }

    [JsonProperty(PropertyName = "presence_penalty")]
    public int Presence_Penalty { get; set; }
    [JsonProperty(PropertyName = "max_tokens")]
    public int Max_Tokens { get; set; }

    public ApiRequestModelGpt4(string sysContent)
    {
        /*
         * sysContent Sample : 
         * "現在開始你是一位專欄作家"
         */

        Messages = new List<Message>
            {
                new Message() { Role = "system", Content = sysContent }
            };
        Temperature = 0.8f;
        Top_p = 0.95f;
        Frequency_Penalty = 0;
        Presence_Penalty = 0;
        Max_Tokens = 1000;
    }

    public void AddUserMessages(string message)
    {
        this.Messages.Add(new Message() { Role = "user", Content = message });
    }
    public void AddGptMessages(string message)
    {
        this.Messages.Add(new Message() { Role = "assistant", Content = message });
    }
}

public class Message
{
    /// <summary>
    /// system/assistant/user
    /// </summary>
    [JsonProperty(PropertyName = "role")]
    public string Role { get; set; }
    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }
}

