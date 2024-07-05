using Google.Cloud.AIPlatform.V1;

namespace KOLAffiliate.API.Models.GeminiModel
{
    public class GeminiRequest
    {
        public GeminiContent[] Contents { get; set; }
        public GenerationConfig GenerationConfig { get; set; }
        public SafetySettingsCustom[] SafetySettings { get; set; }
    }

    public class GeminiContent
    {
        public string Role { get; set; }
        public GeminiPart[] Parts { get; set; }
    }

    public class GeminiPart
    {
        // This one interests us the most
        public string Text { get; set; }
    }

    // Two models used for configuration
    public class GenerationConfig
    {
        public int Temperature { get; set; }
        public int TopK { get; set; }
        public int TopP { get; set; }
        public int MaxOutputTokens { get; set; }
        public List<object> StopSequences { get; set; }
    }

    public class SafetySettingsCustom
    {
        public string Category { get; set; }
        public string Threshold { get; set; }
    }
}
