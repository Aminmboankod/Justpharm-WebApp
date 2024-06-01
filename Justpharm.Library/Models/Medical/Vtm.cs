using System.Text.Json.Serialization;

namespace Justpharm.Library.Models.Medical
{
    public class Vtm
    {
        // [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
    }
}