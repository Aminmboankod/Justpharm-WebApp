using System.Text.Json.Serialization;

namespace Justpharm.Library.Models.Medical
{
    public class Foto
    {
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("fecha")]
        public long Fecha { get; set; }
    }
}