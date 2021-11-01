using System.Text.Json.Serialization;

namespace LoginExample.Models {
public class Adult : Person {
    [JsonPropertyName("JobTitle")]
    public Job JobTitle { get; set; }
}
}