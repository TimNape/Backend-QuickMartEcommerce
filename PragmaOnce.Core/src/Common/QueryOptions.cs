using System.Text.Json.Serialization;
using PragmaOnce.Core.src.ValueObjects;

namespace PragmaOnce.Core.src.Common
{
    public class QueryOptions
    {
        public virtual int Page { get; set; }
        public virtual int PageSize { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortType SortBy { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortOrder SortOrder { get; set; }
    }
}