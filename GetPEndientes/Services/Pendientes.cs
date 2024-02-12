using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetPEndientes.Services
{
    public class Listapendiente
    {
        [JsonPropertyName("nopedido")]
        public string nopedido { get; set; }

        [JsonPropertyName("estatus")]
        public string estatus { get; set; }

        [JsonPropertyName("fechacolocacion")]
        public string fechacolocacion { get; set; }

        [JsonPropertyName("fechaautorizacion")]
        public string fechaautorizacion { get; set; }

        [JsonPropertyName("sku")]
        public string sku { get; set; }

        [JsonPropertyName("articulo")]
        public string articulo { get; set; }

        [JsonPropertyName("claroid")]
        public string claroid { get; set; }

        [JsonPropertyName("idpedidorelacion")]
        public string idpedidorelacion { get; set; }

        [JsonPropertyName("fulfillment")]
        public object fulfillment { get; set; }

        [JsonPropertyName("sla")]
        public string sla { get; set; }

        [JsonPropertyName("comision")]
        public string comision { get; set; }

        [JsonPropertyName("totalproducto")]
        public string totalproducto { get; set; }

        [JsonPropertyName("totalpedido")]
        public string totalpedido { get; set; }

        [JsonPropertyName("skuhijo")]
        public string skuhijo { get; set; }

        [JsonPropertyName("channel")]
        public string channel { get; set; }

        [JsonPropertyName("transactionid")]
        public object transactionid { get; set; }
    }

    public class Pendientes
    {
        [JsonPropertyName("totalpaginas")]
        public int totalpaginas { get; set; }

        [JsonPropertyName("totalpendientes")]
        public int totalpendientes { get; set; }

        [JsonPropertyName("totalregistros")]
        public string totalregistros { get; set; }

        [JsonPropertyName("listapendientes")]
        public List<Listapendiente> listapendientes { get; set; }

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
