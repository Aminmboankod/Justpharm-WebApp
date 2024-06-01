using System.Text.Json.Serialization;

namespace Justpharm.Library.Models.Medical
{
    public class CimaResponse
    {
        [JsonPropertyName("nregistro")]
        public string? NRegistro { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("labtitular")]
        public string? LabTitular { get; set; }

        [JsonPropertyName("cpresc")]
        public string? CPresc { get; set; }

        [JsonPropertyName("estado")]
        public Estado? Estado { get; set; }

        [JsonPropertyName("comerc")]
        public bool Comerc { get; set; }

        [JsonPropertyName("receta")]
        public bool Receta { get; set; }

        [JsonPropertyName("generico")]
        public bool Generico { get; set; }

        [JsonPropertyName("conduc")]
        public bool Conduc { get; set; }

        [JsonPropertyName("triangulo")]
        public bool Triangulo { get; set; }

        [JsonPropertyName("huerfano")]
        public bool Huerfano { get; set; }

        [JsonPropertyName("biosimilar")]
        public bool Biosimilar { get; set; }

        [JsonPropertyName("nosustituible")]
        public NoSustituible? NoSustituible { get; set; }

        [JsonPropertyName("psum")]
        public bool PSum { get; set; }

        [JsonPropertyName("notas")]
        public bool Notas { get; set; }

        [JsonPropertyName("materialesInf")]
        public bool MaterialesInf { get; set; }

        [JsonPropertyName("ema")]
        public bool Ema { get; set; }

        [JsonPropertyName("docs")]
        public List<Documento>? Docs { get; set; }

        [JsonPropertyName("fotos")]
        public List<Foto>? Fotos { get; set; }

        [JsonPropertyName("viasAdministracion")]
        public List<ViaAdministracion>? ViasAdministracion { get; set; }

        [JsonPropertyName("formaFarmaceutica")]
        public FormaFarmaceutica? FormaFarmaceutica { get; set; }

        [JsonPropertyName("formaFarmaceuticaSimplificada")]
        public FormaFarmaceuticaSimplificada? FormaFarmaceuticaSimplificada { get; set; }

        [JsonPropertyName("vtm")]
        public Vtm? Vtm { get; set; }

        [JsonPropertyName("dosis")]
        public string? Dosis { get; set; }
    }
}
