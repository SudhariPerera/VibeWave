using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VibeWave.Models
{
    public class ChatGptResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        [JsonPropertyName("message")]
        public ChatMessage Message { get; set; }
    }
}
