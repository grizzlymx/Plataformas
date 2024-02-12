using MySqlConnector;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UpdStatusPendienteCS.Data;
using UpdStatusPendienteCS.Entities;
using UpdStatusPendienteCS.Services;

namespace UpdStatusPendienteCS.Model
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
                var client = new RestClient(ENDPOINT + "pedidos?action=detallepedido&nopedido=" + Nopedido);
                var request = new RestRequest("", Method.GET, DataFormat.Json);
                var response = client.Execute(request);
                GetPendientes respuesta = JsonConvert.DeserializeObject<GetPendientes>(response.Content);
                return respuesta;
            }
            catch(Exception e )
            {
                log.EscribeLog("error en la llamada a la api ");
                return null;
            }
        }

        public string UpdateCS (string idpedido, string estatus, string mensajeria, string guia,string estatus_guia)
        {
            var log = new clsLog();
            var cn = new clsConexion();
            try
            {
                cn.EjecutaConsulta("UPDATE m_claroshoporderitem SET estatus = @estatus , mensajeria =@mensajeria , guia = @guia, estatus_guia = @estatus_guia WHERE idpedidorelacion= @idpedido", new[]
                {
                    new MySqlParameter("@idpedido", idpedido),
                    new MySqlParameter("@estatus", estatus),
                    new MySqlParameter("@mensajeria", mensajeria),
                    new MySqlParameter("guia", guia),
                    new MySqlParameter("@estatus_guia", estatus_guia)
                }, CommandType.Text);
                return null;
            }
            catch( Exception e )
            {
                log.EscribeLog("error en el Update");
                return null;
            }
        }
    }
}
