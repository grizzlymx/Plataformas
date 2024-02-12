using MySqlConnector;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UpdTrackingNumber_SR.Data;
using UpdTrackingNumber_SR.Entities;
using UpdTrackingNumber_SR.Services;

namespace UpdTrackingNumber_SR.Model
{
    public class EndPoint
    {
        public string endpoint()
        {
            var publicKey = string.Empty;
            var privatekey = string.Empty;
            var url = string.Empty;
            var cn = new clsConexion();
            var log = new clsLog();
            try
            {
                var dt = cn.TraeDataTable("SELECT `value` FROM m_configuration WHERE `name` = 'PUBLICKEY'", new MySqlParameter[] { }, CommandType.Text);
                var dr = dt.Rows[0];
                publicKey = dr["value"].ToString();

                var dt_id = cn.TraeDataTable("SELECT `value` FROM m_configuration WHERE `name` = 'PRIVATEKEY'", new MySqlParameter[] { }, CommandType.Text);
                var dr_id = dt_id.Rows[0];
                privatekey = dr_id["value"].ToString();

                var dt_url = cn.TraeDataTable("SELECT `value` FROM m_configuration WHERE `name` = 'URL_CLAROSHOP'", new MySqlParameter[] { }, CommandType.Text);
                var dr_url = dt_url.Rows[0];
                url = dr_url["value"].ToString();

                var dateD = DateTime.Now.ToString("yyyy-MM-dd");
                var dateH = DateTime.Now.ToString("HH:mm:ss");

                var concatenado = publicKey + dateD + "T" + dateH + privatekey;
                byte[] bytesToHash = Encoding.UTF8.GetBytes(concatenado);
                byte[] hashByte;

                using (SHA256 sha256 = SHA256.Create())
                {
                    hashByte = sha256.ComputeHash(bytesToHash);
                }
                var hashString = BitConverter.ToString(hashByte).Replace("-", "").ToLower();

                var endpoint = url + publicKey + '/' + hashString + '/' + dateD + 'T' + dateH + '/';
                return endpoint;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en el signature " + e.Message);
                return null;
            }
        }
        public GetPendientes nopedido(string Nopedido)
        {
            var log = new clsLog();
            var cn = new clsConexion();
            EndPoint point = new EndPoint();
            var ENDPOINT = point.endpoint();
            try
            {
                var client = new RestClient(ENDPOINT + "pedidos?action=detallepedido&nopedido=" + Nopedido + "&canal=SR");
                var request = new RestRequest("", Method.GET, DataFormat.Json);
                var response = client.Execute(request);
                GetPendientes respuesta = JsonConvert.DeserializeObject<GetPendientes>(response.Content);
                return respuesta;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en la llamada a la api ");
                return null;
            }
        }

        public MensajeConfirmacion mensaje (string pedido,  string idpedido)
        {
            var log = new clsLog();
            var cn = new clsConexion();
            EndPoint point = new EndPoint();
            var ENDPOINT = point.endpoint();
            try
            {
                var enviar = new
                {
                    nopedido = pedido,
                    guia = "automatica",
                    mensajeria = "fedex",
                    idpedidorelacion = idpedido,
                    canal = "SR"
                };
                var url = ENDPOINT + "embarque";
                var client = new RestClient(url);
                var request = new RestRequest("", Method.POST, DataFormat.Json);
                request.AddJsonBody(enviar);
                var respose = client.Execute(request);
                MensajeConfirmacion respuesta = JsonConvert.DeserializeObject<MensajeConfirmacion>(respose.Content);
                return respuesta;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en la llamada a la api");
                return null;
            }
        }

        public string UpdHeader (string sale_order_id ,string tracking, string carrier, string shipment_provider)
        {
            var log = new clsLog();
            var cn = new clsConexion ();
            try
            {
                cn.EjecutaConsulta("UPDATE sale_order_header SET tracking_number =@tracking , carrier_id =@carrier, shipment_provider = @shipment_provider WHERE sale_order_id = @sale_order_id", new[]
                {
                    new MySqlParameter("@sale_order_id", sale_order_id),
                    new MySqlParameter("@tracking", tracking),
                    new MySqlParameter("@carrier", carrier),
                    new MySqlParameter("@shipment_provider", shipment_provider)
                }, CommandType.Text);
                return null;
            }
            catch(Exception e) 
            {
                log.EscribeLog("error en el proceso para actualizar");
                return null;
            }
        }

        public string UpdSROrders(string idpedido, string tracking, string estatuspedido)
        {
            var log = new clsLog();
            var cn = new clsConexion ();
            try
            {
                cn.EjecutaConsulta("UPDATE m_searsorderitem SET estatus_guia = @estatuspedido, guia = @tracking, WHERE idpedidorelacion =@idpedido", new[]
                {
                    new MySqlParameter("@idpedido", idpedido),
                    new MySqlParameter("@estatuspedido", estatuspedido),
                    new MySqlParameter("@tracking", tracking)
                }, CommandType.Text);
                return null;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en actualizar UpdOrders "+ e.Message);
                return null;
            }
        }
    }
}
