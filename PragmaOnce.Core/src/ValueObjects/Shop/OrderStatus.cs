using System.Text.Json.Serialization;

namespace PragmaOnce.Core.src.ValueObjects.Shop
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending,
        Completed,
        Shipped,
        Cancelled,
        Processing,

    }
}