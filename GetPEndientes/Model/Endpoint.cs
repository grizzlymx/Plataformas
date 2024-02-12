using GetPEndientes.Data;
using GetPEndientes.Entities;
using GetPEndientes.Services;
using MySqlConnector;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GetPEndientes.Model
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
                var client = new RestClient(ENDPOINT + "pedidos?action=pendientes&page="+ count);
                var request = new RestRequest("", Method.GET, DataFormat.Json);
                var response = client.Execute(request);
                Pendientes respuesta = JsonConvert.DeserializeObject<Pendientes>(response.Content);
                return respuesta; 
            }
            catch(Exception e) 
            {
                log.EscribeLog("error en la api de GetPendientes "+ e.Message);
                return null;
            }
        }

        public pedido nopedido (string Nopedido) //Api pedido uno por uno 
        {
            var log = new clsLog();
            var cn = new clsConexion();
            Endpoint point = new Endpoint();
            var ENPOINT = point.endpoint();

            try
            {
                var client = new RestClient(ENPOINT + "pedidos?action=detallepedido&nopedido=" + Nopedido);
                var request = new RestRequest("", Method.GET,DataFormat.Json);
                var response = client.Execute(request);
                pedido respuesta = JsonConvert.DeserializeObject<pedido>(response.Content);
                if (response.StatusCode.ToString() == "OK")
                {
                    return respuesta;
                }
                else
                {
                    log.EscribeLog("no respondio la api");
                    return null;
                }
                
            }
            catch( Exception e )
            {
                log.EscribeLog("error en el proceso del Nopedido "+ e.Message);
                return null;
            }
        }

        public string getCSOrdersCount (string idpedidorelacion) //validacion de CS a orders
        {
            var cn = new clsConexion ();
            var log = new clsLog();
            try
            {
                var dt = cn.TraeDataTable("SELECT COUNT(*) as cont FROM m_claroshoporderitem WHERE idpedidorelacion = @idpedidorelacion", new[]
                        {
                            new MySqlParameter("@idpedidorelacion", idpedidorelacion)
                        }, CommandType.Text);
                var dr = dt.Rows[0];
                var reg = dr["cont"].ToString();
                return reg;
            }
            catch ( Exception e )
            {
                log.EscribeLog("error en la consulta para validar "+ e.Message);
                return null;
            }
        }

        public string getCSOrderDetailCount (string idpedidorelacion) // validacion de CS a detail
        {
            var cn = new clsConexion();
            var log = new clsLog();
            try
            {
                var dt = cn.TraeDataTable("SELECT COUNT(*) as cont FROM m_claroshopordedetail WHERE cs_id = @idpedidorelacion", new[]
                        {
                            new MySqlParameter("@idpedidorelacion", idpedidorelacion)
                        }, CommandType.Text);
                var dr = dt.Rows[0];
                var reg = dr["cont"].ToString();
                return reg;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en la consulta para validar " + e.Message);
                return null;
            }
        }

        public string getSROrdersCount(string idpedidorelacion) //validacion de SR 
        {
            var cn = new clsConexion ();
            var log = new clsLog();
            try
            {
                var dt = cn.TraeDataTable("SELECT COUNT(*) as cont FROM m_searsorderitem WHERE idpedidorelacion = @idpedidorelacion", new[]
                {
                    new MySqlParameter("@idpedidorelacion", idpedidorelacion)
                }, CommandType.Text);
                var dr = dt.Rows[0];
                var reg = dr["cont"].ToString();
                return reg;
            }
            catch(Exception e  )
            {
                log.EscribeLog("error en la consuta si existe "+ e.Message);
                return null;
            }
        }

        public string getSROrdersDetailCount (string idpedidorelacion)//validacion de SR a detail
        {
            var cn = new clsConexion();
            var log = new clsLog();
            try
            {
                var dt = cn.TraeDataTable("SELECT COUNT(*) as cont FROM m_searsordedetail WHERE sr_id = @idpedidorelacion", new[]
                {
                    new MySqlParameter("@idpedidorelacion", idpedidorelacion)
                }, CommandType.Text);
                var dr = dt.Rows[0];
                var reg = dr["cont"].ToString();
                return reg;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en la consuta si existe " + e.Message);
                return null;
            }
        }

        public string InsertClaro(string nopedido, string estatus, string fechaC , string fechA, string sku, string articulo, string claroid, string idpedidoRe, string ful, string sla, string comision, string totalPro, string totalPe, string hijo, string channel, string transactionid,string Nom)
        {
            var cn = new clsConexion ();
            var log = new clsLog();
            try
            {
                var dt = cn.EjecutaConsulta("INSERT INTO m_claroshoporderitem(date_add,cs_id,idpedidorelacion,claroshop_order_number,estatus,fecha_de_colocacion,fecha_autorizacion,claroid,sku,sku_hijo,articulo,sla,comission,precio,cantidad,totalpedido,comision,channel,fullfillment,transactionid)" +
                    "VALUES(NOW(),@nopedido, @idpedidoRe, @Nom, @estatus, @fechaC, @fechA,@claroid, @sku, @hijo, @articulo, @sla, @comision, @totalPro, 1 , @totalPe, 0.14,@channel, @ful,@transactionid)", new[]
                    {
                        new MySqlParameter("@nopedido", nopedido),
                        new MySqlParameter("@idpedidoRe", idpedidoRe),
                        new MySqlParameter("@Nom", Nom),
                        new MySqlParameter("@estatus", estatus),
                        new MySqlParameter("@fechaC", fechaC),
                        new MySqlParameter("@fechA", fechA),
                        new MySqlParameter("@claroid", claroid),
                        new MySqlParameter("@sku",sku),
                        new MySqlParameter("@hijo", hijo),
                        new MySqlParameter("@articulo", articulo),
                        new MySqlParameter("@sla", sla),
                        new MySqlParameter("@comision", comision),
                        new MySqlParameter("@totalPro",totalPro),
                        new MySqlParameter("@totalPe",totalPe),
                        new MySqlParameter("@channel", channel),
                        new MySqlParameter("@ful",ful),
                        new MySqlParameter("@transactionid",transactionid)

                }, CommandType.Text);
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string InsertSears(string nopedido, string estatus, string fechaC, string fechA, string sku, string articulo, string claroid, string idpedidoRe, string ful, string sla, string comision, string totalPro, string totalPe, string hijo, string channel, string transactionid, string Nom)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            try
            {
                var dt = cn.EjecutaConsulta("INSERT INTO m_searsorderitem(date_add,sr_id,idpedidorelacion,sears_order_number,estatus,fecha_de_colocacion,fecha_autorizacion,searsid,sku,sku_hijo,articulo,sla,comission,precio,cantidad,totalpedido,comision,channel,fullfillment,transactionid)" +
                    "VALUES(NOW(), @nopedido, @idpedidoRe, @Nom, @estatus, @fechaC, @fechA,@claroid, @sku, @hijo, @articulo, @sla, @comision, @totalPro, 1 , @totalPe, 0.14,@channel, @ful,@transactionid)", new[]
                    {
                        new MySqlParameter("@nopedido", nopedido),
                        new MySqlParameter("@idpedidoRe", idpedidoRe),
                        new MySqlParameter("@Nom", Nom),
                        new MySqlParameter("@estatus", estatus),
                        new MySqlParameter("@fechaC", fechaC),
                        new MySqlParameter("@fechA", fechA),
                        new MySqlParameter("@claroid", claroid),
                        new MySqlParameter("@sku",sku),
                        new MySqlParameter("@hijo", hijo),
                        new MySqlParameter("@articulo", articulo),
                        new MySqlParameter("@sla", sla),
                        new MySqlParameter("@comision", comision),
                        new MySqlParameter("@totalPro",totalPro),
                        new MySqlParameter("@totalPe",totalPe),
                        new MySqlParameter("@channel", channel),
                        new MySqlParameter("@ful",ful),
                        new MySqlParameter("@transactionid",transactionid)

                }, CommandType.Text);
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public string InsertClaroDetail(string No, string entregara, string direccion, string entrecalles,string colonia, string del_municipio, string cp, string ciudad, string estado, string observaciones,string telefono ,string idpedidorelacion)
        {
            var cn = new clsConexion();
            var log = new clsLog();

            try
            {
                var dt = cn.EjecutaConsulta("INSERT INTO m_claroshopordedetail(cs_id, entregara, direccion, entrecalles, colonia, delegacion_municipio, cp, ciudad, estado, observaciones,telefono, idpedidorelacion, date_created)" +
                    "VALUE(@cs_id, @entregara, @direccion, @entrecalles, @colonia, @del_municipio, @cp, @ciudad, @estado, @observaciones, @telefono, @idpedidorelacion,NOW());", new[]
                    {
                        new MySqlParameter("@cs_id", No),
                        new MySqlParameter("@entregara", entregara),
                        new MySqlParameter("@direccion", direccion),
                        new MySqlParameter("@entrecalles", entrecalles),
                        new MySqlParameter("@colonia", colonia),
                        new MySqlParameter("@del_municipio", del_municipio),
                        new MySqlParameter("@cp", cp),
                        new MySqlParameter("@ciudad", ciudad),
                        new MySqlParameter("@estado", estado),
                        new MySqlParameter("@observaciones", observaciones),
                        new MySqlParameter("@telefono", telefono),
                        new MySqlParameter("@idpedidorelacion",idpedidorelacion)

                    }, CommandType.Text);
                return null;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en el InsertClaroDetail");
                return null;
            }
        }

        public string InsertSRDetail(string No, string entregara, string direccion, string entrecalles, string colonia, string del_municipio, string cp, string ciudad, string estado, string observaciones, string telefono, string idpedidorelacion)
        {
            var cn = new clsConexion();
            var log = new clsLog();

            try
            {
                var dt = cn.EjecutaConsulta("INSERT INTO m_searsordedetail(sr_id, entregara, direccion, entrecalles, colonia, delegacion_municipio, cp, ciudad, estado, observaciones,telefono, idpedidorelacion, date_created)" +
                    "VALUE(@sr_id, @entregara, @direccion, @entrecalles, @colonia, @del_municipio, @cp, @ciudad, @estado, @observaciones, @telefono, @idpedidorelacion,NOW())", new[]
                    {
                        new MySqlParameter("@sr_id", No),
                        new MySqlParameter("@entregara", entregara),
                        new MySqlParameter("@direccion", direccion),
                        new MySqlParameter("@entrecalles", entrecalles),
                        new MySqlParameter("@colonia", colonia),
                        new MySqlParameter("@del_municipio", del_municipio),
                        new MySqlParameter("@cp", cp),
                        new MySqlParameter("@ciudad", ciudad),
                        new MySqlParameter("@estado", estado),
                        new MySqlParameter("@observaciones", observaciones),
                        new MySqlParameter("@telefono", telefono),
                        new MySqlParameter("@idpedidorelacion",idpedidorelacion)

                    }, CommandType.Text);
                return null;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en el InsertClaroDetail");
                return null;
            }
        }
        public string UpdateCS(string transactionid, string Nopedido)
        {
            var cn = new clsConexion();
            var log = new clsLog();

            try
            {
                if(transactionid != null)
                {
                    var dt = cn.EjecutaConsulta("UPDATE m_claroshoporderitem SET date_add = NOW() , transactionid = @transactionid WHERE cs_id = @Nopedido ", new[]
                    {
                    new MySqlParameter("@transactionid", transactionid),
                    new MySqlParameter("@Nopedido", Nopedido)
                     }, CommandType.Text);
                    log.EscribeLog("Se Actualizo el pedido " + Nopedido);
                }
                else
                {
                    log.EscribeLog("Aun viene vacio" + Nopedido);
                }
                return null;
            }
            catch ( Exception e )
            {
                log.EscribeLog("error en el proceso para actualizar");
                return null;
            }
        }

        public string UpdateSR(string transactionid, string Nopedido)
        {
            var cn = new clsConexion();
            var log = new clsLog();

            try
            {
                if (transactionid != null)
                {
                    var dt = cn.EjecutaConsulta("UPDATE m_searsorderitem SET date_add = NOW() , transactionid = @transactionid WHERE cs_id = @Nopedido ", new[]
                    {
                    new MySqlParameter("@transactionid", transactionid)
                     }, CommandType.Text);
                    log.EscribeLog("Se Actualizo el pedido " + Nopedido);
                }
                else
                {
                    log.EscribeLog("Aun viene vacio" + Nopedido);
                }
                return null;
            }
            catch (Exception e)
            {
                log.EscribeLog("error en el proceso para actualizar");
                return null;
            }
        }
    }
}
