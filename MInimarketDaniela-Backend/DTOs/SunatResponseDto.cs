using System.Text.Json.Serialization;

namespace MInimarketDaniela_Backend.DTOs
{
    public class SunatResponseDto
    {
        [JsonPropertyName("razon_social")]
        public string RazonSocial { get; set; } = string.Empty;

        [JsonPropertyName("numero_documento")]
        public string Ruc { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;

        [JsonPropertyName("condicion")]
        public string Condicion { get; set; } = string.Empty;

        [JsonPropertyName("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [JsonPropertyName("ubigeo")]
        public string Ubigeo { get; set; } = string.Empty;

        [JsonPropertyName("distrito")]
        public string Distrito { get; set; } = string.Empty;

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; } = string.Empty;

        [JsonPropertyName("departamento")]
        public string Departamento { get; set; } = string.Empty;
    }
}
