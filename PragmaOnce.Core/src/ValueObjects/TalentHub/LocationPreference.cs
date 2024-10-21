using System.Text.Json.Serialization;

namespace PragmaOnce.Core.src.ValueObjects.TalentHub
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LocationPreference
    {
        RemoteOnly,
        Hybrid,
        OfficeOnly
    }
}
