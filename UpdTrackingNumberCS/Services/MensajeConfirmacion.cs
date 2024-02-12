using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UpdTrackingNumberCS.Services
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class MensajeConfirmacion
    {
        [JsonPropertyName("estatus")]
        public string estatus { get; set; }

        [JsonPropertyName("mensaje")]
        public string mensaje { get; set; }

        [JsonPropertyName("nopedido")]
        public string nopedido { get; set; }

        [JsonPropertyName("tipoguia")]
        public string tipoguia { get; set; }

        [JsonPropertyName("mensajeria")]
        public string mensajeria { get; set; }

        [JsonPropertyName("guia")]
        public string guia { get; set; }

        [JsonPropertyName("productoid")]
        public string productoid { get; set; }

        [JsonPropertyName("idrelacionproducto")]
        public string idrelacionproducto { get; set; }

        [JsonPropertyName("canal")]
        public string canal { get; set; }

        [JsonPropertyName("versionConfig")]
        public string versionConfig { get; set; }

        [JsonPropertyName("versionAPP")]
        public string versionAPP { get; set; }

        [JsonPropertyName("tagManagerID")]
        public string tagManagerID { get; set; }

        [JsonPropertyName("tagManagerIDCS")]
        public string tagManagerIDCS { get; set; }

        [JsonPropertyName("visibleMenuCV")]
        public bool visibleMenuCV { get; set; }
    }
}
