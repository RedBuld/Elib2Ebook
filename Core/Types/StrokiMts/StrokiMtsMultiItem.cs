using System.Text.Json.Serialization;

namespace Core.Types.StrokiMts;

public class StrokiMtsMultiItem {
    [JsonPropertyName("textBook")]
    public StrokiMtsBookItem TextBook { get; set; }
}