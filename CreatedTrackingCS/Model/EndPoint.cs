using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CreatedTrackingCS.Data;
using CreatedTrackingCS.Entities;
using CreatedTrackingCS.Services;
using MySqlConnector;
using Newtonsoft.Json;
using RestSharp;

namespace CreatedTrackingCS.Model
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

        public MensajeConfirmacion PostTracking (string nopedido, string idpedido, string sku)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            EndPoint point = new EndPoint();
            var ENDPOINT = point.endpoint();
            try
            {
                var enviar = new
                {
                    nopedido = nopedido,
                    guia = "automatica",
                    mensajeria = "fedex",
                    idpedidorelacion = idpedido,
                    canal = "CS"
                };
                var url = ENDPOINT + "embarque";
                var client = new RestClient(url);
                var request = new RestRequest("", Method.POST, DataFormat.Json);
                var request1 = request.AddJsonBody(enviar);
                var respose = client.Execute(request1);
                MensajeConfirmacion respuesta = JsonConvert.DeserializeObject<MensajeConfirmacion>(respose.Content);

                var estatus = respuesta.estatus;
                var mensaje = respuesta.mensaje;
                var json = JsonConvert.SerializeObject(enviar);

                cn.EjecutaConsulta("INSERT INTO m_createdTrackingLogCS (cs_id, idpedidorelacion, seller_sku, estatus, mensaje, endpoint,request_data ,response_data, add_date, `server`) " +
                    "VALUES(@nopedido,@idpedido, @sku, @estatus, @mensaje, @ENDPOINT,@json, @respose, NOW(),'netcore Created Tracking CS')", new[]
                    {
                        new MySqlParameter("@nopedido", nopedido),
                        new MySqlParameter("@idpedido", idpedido),
                        new MySqlParameter("@sku", sku),
                        new MySqlParameter("@estatus", estatus),
                        new MySqlParameter("@mensaje", mensaje),
                        new MySqlParameter("@ENDPOINT", ENDPOINT),
                        new MySqlParameter("@json", json),
                        new MySqlParameter("@respose", respose.Content)

                    }, CommandType.Text);

                return respuesta;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en el bloque de PostTracking "+ e.Message);
                return null;
            }
        }
    }
}
