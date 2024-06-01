using System.Text.Json.Serialization;

namespace Justpharm.Library.Models.Medical
{
    public class Documento
    {
        [JsonPropertyName("tipo")]
        public int Tipo { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("urlHtml")]
        public string UrlHtml { get; set; }
        [JsonPropertyName("secc")]
        public bool Secc { get; set; }
        [JsonPropertyName("fecha")]
        public long Fecha { get; set; }
    }
}