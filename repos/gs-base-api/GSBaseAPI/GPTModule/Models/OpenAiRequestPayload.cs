namespace GSBaseAPI.GPTModule.Models
{
    public sealed class OpenAiRequestPayload
    {
        public string model { get; set; } = "gpt-5";
        public object[] input { get; set; } = Array.Empty<object>();
        
    }

}
