using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace LoginExample.Models {
public class User {
    [Required]
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    [Required]
    [NotNull]
    [JsonPropertyName("UserName")]
    public string UserName { get; set; }
    [Required]
    [NotNull]
    [JsonPropertyName("Domain")]
    public string Domain { get; set; }
    [Required]
    [NotNull]
    [JsonPropertyName("City")]
    public string City { get; set; }
    [Required]
    [JsonPropertyName("BirthYear")]
    public int BirthYear { get; set; }
    [Required, MaxLength(32)]
    [NotNull]
    [DefaultValue("Student")]
    [JsonPropertyName("Role")]
    public string Role { get; set; }
    [Required]
    [JsonPropertyName("SecurityLevel")]
    public int SecurityLevel { get; set; }
    [Required]
    [NotNull]
    [JsonPropertyName("Password")]
    public string Password { get; set; }
}
}