using GetCustomCS.Data;
using GetCustomCS.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCustomCS.Model
{
    public class ConsultaAndUpd
    {
        public string getOrderCustomCS(string id_pedido, string idpedidorelacion)
        {
            var log = new clsLog();
            var cn = new clsConexion();
            try
            {
                var dt = cn.TraeDataTable("SELECT COUNT(*) as cont FROM m_claroshoporderitem WHERE cs_id =@id_pedido  and idpedidorelacion=@idpedidorelacion", new[]
                {
                    new MySqlParameter("@id_pedido", id_pedido),
                    new MySqlParameter("@idpedidorelacion", idpedidorelacion)
                },System.Data.CommandType.Text);
                var dr = dt.Rows[0];
                var reg = dr["cont"].ToString();
                return reg;

            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public string updOrderCustomCS(string id_pedido, string idpedidorelacion)
        {
            var log = new clsLog();
            var cn = new clsConexion();

            try
            {
                cn.EjecutaConsulta("UPDATE m_claroshoporderitem SET estatus ='Embarcado', mensajeria ='custom', guia ='custom' WHERE cs_id=@id_pedido AND idpedidorelacion =@idpedidorelacion", new[]
                {
                    new MySqlParameter("@id_pedido", id_pedido),
                    new MySqlParameter("@idpedidorelacion", idpedidorelacion)
                }, System.Data.CommandType.Text);
                return null;
            }
            catch(Exception e)
            {
                log.EscribeLog("error en el Update Custom");
                return null;
            }
        }
    }
}
