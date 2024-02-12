using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ConfirmTrackingFallidos.Data;
using ConfirmTrackingFallidos.Entities;
using ConfirmTrackingFallidos.Services;
using CreatedTrackingCS.Services;
using MySqlConnector;
using Newtonsoft.Json;
using RestSharp;

namespace ConfirmTrackingFallidos.Model
{
    public class Endpoint
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

        public Pendientes pedidos()// api pendientes general de todos 
        {
            var log = new clsLog();
            var cn = new clsConexion();
            var count = 0;
            Endpoint point = new Endpoint();
            var ENDPOINT = point.endpoint();
            try
            {
                var client = new RestClient(ENDPOINT + "pedidos?action=pendientes&page=" + count);
                var request = new RestRequest("", Method.GET, DataFormat.Json);
                var response = client.Execute(request);
                Pendientes respuesta = JsonConvert.DeserializeObject<Pendientes>(response.Content);
                return respuesta;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en la api de GetPendientes " + e.Message);
                return null;
            }
        }

        public MensajeConfirmacion confirmar(string nopedido, string pedidorelacion, string guia, string canal)
        {
            var log = new clsLog();
            var cn = new clsConexion();
            Endpoint point = new Endpoint();
            var ENDPOINT = point.endpoint();

            try
            {
                var enviar = new
                {
                    nopedido = nopedido,
                    guia = "manual",
                    mensajeria = "manual",
                    idpedidorelacion = pedidorelacion,
                    rastreomanual = guia,
                    canal = canal
                };
                var url = ENDPOINT + "embarque";
                var client = new RestClient(url);
                var request = new RestRequest("", Method.POST, DataFormat.Json);
                request.AddJsonBody(enviar);
                var respose = client.Execute(request);
                MensajeConfirmacion respuesta = JsonConvert.DeserializeObject<MensajeConfirmacion>(respose.Content);
                return respuesta;
            }
            catch(Exception e)
            {
                log.EscribeLog("error en la confirmacion");
                return null;
            }
        }
    }
}
