using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdTrackingNumberCS.Data;
using UpdTrackingNumberCS.Entities;
using UpdTrackingNumberCS.Services;

namespace UpdTrackingNumberCS.Model
{
    public class process
    {
        public void ProcessTracking(DataTable data)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var point = new EndPoint();
            try
            {
                var count = data.Rows.Count;
                log.EscribeLog("Pedidos por procesar: "+ count);
                foreach(DataRow dataRow in data.Rows)
                {
                    string pedido = dataRow[2].ToString();
                    string idpedido = dataRow[3].ToString();
                    string sale_order_id = dataRow[0].ToString();
                    string customer = dataRow[4].ToString();

                    log.EscribeLog("cs_id: " + pedido);
                    log.EscribeLog("idpedido: " + idpedido);
                    log.EscribeLog("sale_order_id: " + sale_order_id);
                    log.EscribeLog("customer: " + customer);

                    var response = point.nopedido(pedido);
                    var cont = response.productos.Count;

                    for(int i = 0; i < cont; i++)
                    {
                        var tracking_number = response.productos[i].guia;
                        var carrier_name = string.Empty;
                        var carrier = string.Empty;
                        var UpdHeader = string.Empty;
                        var UpdCSOrders = string.Empty;
                        if (response.productos[i].idpedidorelacion == idpedido)
                        {
                           
                            var estatus = response.productos[i].estatus;

                            if(tracking_number == null)
                            {
                                var response2 = point.mensaje(pedido, idpedido);
                                if(response2.mensaje == "La guia fue asignada al pedido correctamente")
                                {
                                    var shipment_provider = "Fedex";
                                    var carrier_id = "1";
                                    var tracking_number2 = response2.guia;
                                    UpdHeader = point.UpdHeader(sale_order_id, tracking_number2, carrier_id, shipment_provider);
                                    var estatusguia = "Por embarcar con Proveedor";
                                    UpdCSOrders = point.UpdCSOrders(idpedido,tracking_number2,estatusguia);
                                    log.EscribeLog("Guia Creada");
                                }
                                else
                                {
                                    log.EscribeLog("La api no la puede generar "+ pedido + " id pedido: "+idpedido+ " solicitar a ClaroShop que la genere");
                                }
                            }
                            else
                            {
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
                                        carrier = "5"; break;
                                    case "Fedex":
                                        carrier = "1"; break;
                                    case "T1 Bigsmart":
                                        carrier = "25"; break;
                                }
                                #endregion
                                UpdHeader = point.UpdHeader(sale_order_id, tracking_number, carrier, carrier_name);
                                UpdCSOrders = point.UpdCSOrders(sale_order_id, tracking_number, estatus);
                                log.EscribeLog("Guia Actualizada");
                            }
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                log.EscribeLog("error en el proceso de Tracking Number CS");
            }
        }
    }
}
