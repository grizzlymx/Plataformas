using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UpdStatusPendienteCS.Data;
using UpdStatusPendienteCS.Entities;

namespace UpdStatusPendienteCS.Model
{
    public class process
    {
        public void proceso(DataTable pedidos)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var pending = new EndPoint();
            try
            {
                var count = pedidos.Rows.Count;
                log.EscribeLog("Pedidos por procesar: "+ count);
                foreach(DataRow pedido in pedidos.Rows)
                {
                    string cs_id = pedido[0].ToString();
                    string idpedidorelacion = pedido[1].ToString();
                    string sku = pedido[2].ToString();
                    string seller_sku = pedido[3].ToString();

                    log.EscribeLog("cs_id: "+cs_id);
                    log.EscribeLog("idpedidorelacion: "+idpedidorelacion);
                    log.EscribeLog("sku: "+ sku);
                    log.EscribeLog("seller_sku: "+ seller_sku);

                    var answer = pending.nopedido(cs_id);
                    var count1 = answer.productos.Count;
                    var update = string.Empty;
                    for(int i = 0; i < count1; i++)
                    {
                        if (answer.productos[i].idpedidorelacion ==idpedidorelacion)
                        {
                            if (answer.productos[i].guia == null)
                            {
                                log.EscribeLog("aun no trae guia"+ idpedidorelacion);
                                var estatus = answer.estatuspedido.estatus;
                                var mensajeria = "Pendiente";
                                var guia = "";
                                var estatus_guia = answer.productos[i].estatus;
                                update = pending.UpdateCS(idpedidorelacion , estatus, mensajeria, guia, estatus_guia);
                                
                            }
                            else
                            {
                                var tracking_number = answer.productos[i].guia;
                                var estatus = answer.estatuspedido.estatus;
                                var estatusguia = answer.productos[i].estatus;
                                var carrier_name = string.Empty;
                                var carrier_id = 0;
                                #region validar_carrier
                                if (tracking_number.Length == 10)
                                {
                                    carrier_name = "DHL";
                                }
                                else
                                    if (tracking_number.Length == 12)
                                {
                                    carrier_name = "Fedex";
                                }
                                else
                                    if (tracking_number.Length == 16)
                                {
                                    carrier_name = "T1 Bigsmart";
                                }
                                else
                                {
                                    carrier_name = "Pendiente";
                                }
                                switch (carrier_name)
                                {
                                    case "DHL":
                                        carrier_id = 5; break;
                                    case "Fedex":
                                        carrier_id = 1; break;
                                    case "T1 Bigsmart":
                                        carrier_id = 25; break;
                                }
                                #endregion
                                log.EscribeLog("tracking_number: " + tracking_number);
                                log.EscribeLog("estatus guia: " + estatusguia);
                                log.EscribeLog("Carrier Name: " + carrier_name);
                                log.EscribeLog("Carrier id: "+  carrier_id);
                                update = pending.UpdateCS(idpedidorelacion,estatus ,carrier_name, tracking_number, estatusguia);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.EscribeLog("error en el proceso "+ ex.Message);
            }
            
        }
    }
}
