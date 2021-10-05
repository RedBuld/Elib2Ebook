using System.Text.Json.Serialization;

namespace Author.Today.Epub.Converter.Types.Litmarket {
    public class Mod {
        [JsonPropertyName("type")] 
        public string Type { get; set; }

        [JsonPropertyName("text")] 
        public string Text { get; set; }
        
        [JsonPropertyName("data")]
        public ModData Data { get; set; }
        
        [JsonPropertyName("mods")]
        public Mod[] Mods { get; set; }
    }
}