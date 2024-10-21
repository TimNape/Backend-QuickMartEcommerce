using System.Text.Json.Serialization;

namespace PragmaOnce.Core.src.ValueObjects
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        Admin,
        Customer
    }
}