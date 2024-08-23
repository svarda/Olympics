using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Olympics.Core.Entities {
    public class MedalData {
        [JsonConverter(typeof(StringEnumConverter))]
        public Season Season { get; set; }
        public int? Year { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Rank Rank { get; set; }
        public IList<string> Names { get; set; } = new List<string>();
        public string? Sport { get; set; }
        public string? Competition { get; set; }
        public string? Date { get; set; }
    }
}
