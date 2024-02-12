using ConfirmTrackingFallidos.Data;
using ConfirmTrackingFallidos.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfirmTrackingFallidos.Model
{
    public class process
    {
        public void embarque()
        {
            try
            {
                var cn = new clsConexion();
                var log = new clsLog();
                var pedidos = new Endpoint();

                var resp = pedidos.pedidos();
                var count = resp.listapendientes.Count();

                var cont = 0;

                log.EscribeLog("pedidos por procesar: "+ count);
                for (int i = 0; i < count; i++)
                {
                    
                    var nopedido = string.Empty;
                    var norelacion = string.Empty;
                    var channel = string.Empty; 
                    nopedido = resp.listapendientes[i].nopedido;
                    norelacion = resp.listapendientes[i].idpedidorelacion;
                    channel = resp.listapendientes[i].channel;

                    var query = cn.TraeDataTable("SELECT reference_order_number AS id,shipping_id_number AS pedidorelacion,tracking_number AS guia  FROM  sale_order_header soh  WHERE shipping_id_number = '" + norelacion + "' AND (customer_id = '80' OR customer_id = '103') AND (tracking_number <> '' AND tracking_number <> 'custom')", new MySqlParameter[] { }, System.Data.CommandType.Text);

                    if(query.Rows.Count >= 1 )
                    {
                        var pedidorelacion = query.Rows[1].ToString();
                        var tracking = query.Rows[2].ToString();

                        pedidos.confirmar(nopedido, pedidorelacion, tracking, channel);

                    }
                    else
                    {
                        log.EscribeLog("el pedido aun no esta en merch "+nopedido);
                    }
                    cont++;
                    log.EscribeLog("pedido por procesar: "+  cont++); 
                }

            }
            catch(Exception ex)
            {

            }
        }
    }
}
