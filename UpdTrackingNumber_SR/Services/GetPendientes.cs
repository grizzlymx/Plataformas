using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UpdTrackingNumber_SR.Services
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Datosenvio
    {
        [JsonPropertyName("entregara")]
        public string entregara { get; set; }

        [JsonPropertyName("direccion")]
        public string direccion { get; set; }

        [JsonPropertyName("entrecalles")]
        public string entrecalles { get; set; }

        [JsonPropertyName("colonia")]
        public string colonia { get; set; }

        [JsonPropertyName("del/municipio")]
        public string delmunicipio { get; set; }

        [JsonPropertyName("cp")]
        public string cp { get; set; }

        [JsonPropertyName("ciudad")]
        public string ciudad { get; set; }

        [JsonPropertyName("estado")]
        public string estado { get; set; }

        [JsonPropertyName("observaciones")]
        public string observaciones { get; set; }
    }

    public class Estatuspedido
    {
        [JsonPropertyName("estatus")]
        public string estatus { get; set; }

        [JsonPropertyName("fechacolocado")]
        public string fechacolocado { get; set; }
    }

    public class Producto
    {
        [JsonPropertyName("idchannel")]
        public string idchannel { get; set; }

        [JsonPropertyName("fechaasignacion")]
        public string fechaasignacion { get; set; }

        [JsonPropertyName("fechaenvio")]
        public object fechaenvio { get; set; }

        [JsonPropertyName("producto")]
        public string producto { get; set; }

        [JsonPropertyName("importe")]
        public string importe { get; set; }

        [JsonPropertyName("envio")]
        public string envio { get; set; }

        [JsonPropertyName("estatus")]
        public string estatus { get; set; }

        [JsonPropertyName("asignado")]
        public string asignado { get; set; }

        [JsonPropertyName("guia")]
        public string guia { get; set; }

        [JsonPropertyName("claroid")]
        public string claroid { get; set; }

        [JsonPropertyName("idpedidorelacion")]
        public string idpedidorelacion { get; set; }

        [JsonPropertyName("skuhijo")]
        public string skuhijo { get; set; }

        [JsonPropertyName("sku")]
        public string sku { get; set; }

        [JsonPropertyName("skufullfilment")]
        public bool skufullfilment { get; set; }

        [JsonPropertyName("transactionid")]
        public object transactionid { get; set; }
    }

    public class GetPendientes
    {
        [JsonPropertyName("estatuspedido")]
        public Estatuspedido estatuspedido { get; set; }

        [JsonPropertyName("datosenvio")]
        public Datosenvio datosenvio { get; set; }

        [JsonPropertyName("comentarios")]
        public List<object> comentarios { get; set; }

        [JsonPropertyName("productos")]
        public List<Producto> productos { get; set; }

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
        public string Content { get; internal set; }
    }

}
