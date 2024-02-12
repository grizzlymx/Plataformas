using GetPEndientes.Data;
using GetPEndientes.Entities;
using GetPEndientes.Services;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPEndientes.Model
{
    public class Claro_Sears
    {
        public void proceso()
        {
            var log = new clsLog();
            var cn = new clsConexion();
            var pendiente = new Endpoint();
            try
            {
                var resp = pendiente.pedidos();
                var count = resp.listapendientes.Count;
                var cont = 0;
                log.EscribeLog("Pedidos: "+ count);
                for (int i=0; i < count; i++)
                {
                    log.EscribeLog("*************************");
                    var insert = string.Empty;
                    var insert2 = string.Empty;
                    var Update = string.Empty;
                    var reg = string.Empty;
                    var reg2 = string.Empty;
                    var sku_nuevo = string.Empty;
                    #region
                    var Nopedido = string.Empty;
                    var status = string.Empty;
                    var fechColocacion = string.Empty;
                    var fechAutorizacion = string.Empty;
                    var sku = string.Empty;
                    var articulo= string.Empty;
                    var id = string.Empty;
                    var idpedidorelacion = string.Empty;
                    var fulfillment = string.Empty;
                    var sla = string.Empty;
                    var comision = string.Empty;
                    var totalproducto = string.Empty;
                    var totalpedido = string.Empty;
                    var skuhijo = string.Empty;
                    var channel = string.Empty;
                    var transactionid = string.Empty;
                    #endregion

                    Nopedido = resp.listapendientes[i].nopedido;
                    status = resp.listapendientes[i].estatus;
                    fechColocacion = resp.listapendientes[i].fechacolocacion;
                    fechAutorizacion = resp.listapendientes[i].fechaautorizacion;
                    sku = resp.listapendientes[i].sku;
                    articulo = resp.listapendientes[i].articulo;
                    id = resp.listapendientes[i].claroid;
                    idpedidorelacion = resp.listapendientes[i].idpedidorelacion;
                    fulfillment = resp.listapendientes[i].fulfillment.ToString();
                    if(fulfillment.ToLower() == "true")
                    {
                        fulfillment = "1";
                    }
                    else
                    {
                        fulfillment = "0";
                    }
                    sla = resp.listapendientes[i].sla;
                    comision = resp.listapendientes[i].comision;
                    totalproducto = resp.listapendientes[i].totalproducto;
                    totalpedido = resp.listapendientes[i].totalpedido;
                    //skuhijo = resp.listapendientes[i].skuhijo;
                    channel = resp.listapendientes[i].channel;
                    if (resp.listapendientes[i].transactionid != null)
                    {
                        transactionid = resp.listapendientes[i].transactionid.ToString();
                    }
                    else
                    {
                        transactionid = "";
                    }
                    log.EscribeLog("Nopedido: " + Nopedido);
                    log.EscribeLog("Plataforma: " + channel);
                    log.EscribeLog("SKU: " + sku);

                    if (sku == "mkz-mancuhexaneg")
                    {
                        sku = "mkz-mancu15lbneg";
                    }
                    else if (sku == "mkz-sillagravneg")
                    {
                        sku = "mkz-sillagravnegx2";
                    }

                    var respu = pendiente.nopedido(Nopedido);//api por pedido

                    if (respu != null)
                    {
                        #region
                        var entregara = string.Empty;
                        var direccion = string.Empty;
                        var entrecalles = string.Empty;
                        var colonia = string.Empty;
                        var del_municipio = string.Empty;
                        var cp = string.Empty;
                        var ciudad = string.Empty;
                        var estado = string.Empty;
                        var observaciones = string.Empty;
                        var telefono = "5555555555";
                        var idpedidorelaciondetail = string.Empty;
                        var idChannel = string.Empty;
                        var sku_hijo = string.Empty;
                        #endregion

                        var countproduc = respu.productos.Count(); //cuantos pedidos hay en caso de que sea un carrito 
                        for (int j = 0; j < countproduc; j++)
                        {
                            entregara = respu.datosenvio.entregara;
                            direccion = respu.datosenvio.direccion;
                            entrecalles = respu.datosenvio.entrecalles;
                            colonia = respu.datosenvio.colonia;
                            del_municipio = respu.datosenvio.delmunicipio;
                            cp = respu.datosenvio.cp;
                            ciudad = respu.datosenvio.ciudad;
                            estado = respu.datosenvio.estado;
                            observaciones = respu.datosenvio.observaciones;
                            idpedidorelaciondetail = respu.productos[j].idpedidorelacion;
                            idChannel = respu.productos[j].idchannel;
                            skuhijo = respu.productos[j].skuhijo;

                            log.EscribeLog("sku en bloque en detail: " + respu.productos[j].sku);
                            if (idChannel == "CS")
                            {
                                reg2 = pendiente.getCSOrderDetailCount(Nopedido);
                                if (int.Parse(reg2) < 1)
                                {
                                    var No = Nopedido + "-69-2738";
                                    //insert2 = pendiente.InsertClaroDetail(Nopedido,entregara, direccion, entrecalles, colonia, del_municipio, cp, ciudad, estado, observaciones, telefono, idpedidorelaciondetail);
                                    log.EscribeLog("Se inserto nuevo producto en Detail CS " + No);
                                }
                            }
                            else //SEARS
                            {
                                reg2 = pendiente.getSROrdersDetailCount(Nopedido);
                                if (int.Parse(reg2) < 1)
                                {
                                    var No = Nopedido + "-69-2738";
                                    //insert2 = pendiente.InsertSRDetail(Nopedido, entregara, direccion, entrecalles, colonia, del_municipio, cp, ciudad, estado, observaciones, telefono, idpedidorelaciondetail);
                                    log.EscribeLog("Se inserto nuevo producto en Detail SR " + No);
                                }
                            }

                            if (channel == "CS")
                            {
                                reg = pendiente.getCSOrdersCount(idpedidorelacion);
                                if (int.Parse(reg) < 1)
                                {
                                    var No = Nopedido + "-69-2738";
                                    //insert = pendiente.InsertClaro(Nopedido,status,fechColocacion,fechAutorizacion, sku, articulo, id, idpedidorelacion,fulfillment, sla,comision, totalproducto, totalpedido, skuhijo, channel,transactionid,No);
                                    log.EscribeLog("Se inserto nuevo producto CS " + No);
                                }
                                else
                                {
                                    var No = Nopedido + "-69-2738";
                                    //Update = pendiente.UpdateCS(transactionid, Nopedido);
                                    log.EscribeLog("Se actualizo " + Nopedido);
                                }
                            }
                            else // SEARS
                            {
                                reg = pendiente.getSROrdersCount(idpedidorelacion);
                                if (int.Parse(reg) < 1)
                                {
                                    var No = Nopedido + "-69-2738";
                                    //insert = pendiente.InsertSears(Nopedido, status, fechColocacion, fechAutorizacion, sku, articulo, id, idpedidorelacion, fulfillment, sla, comision, totalproducto, totalpedido, skuhijo, channel, transactionid, No);
                                    log.EscribeLog("Se inserto nuevo producto SR " + No);

                                }
                                else
                                {
                                    var No = Nopedido + "-69-2738";
                                    //Update = pendiente.UpdateSR(transactionid, Nopedido);
                                    log.EscribeLog("Se actualizo " + Nopedido);
                                }
                            }
                        }
                    }
                    else
                    {
                        log.EscribeLog("el pedido " + Nopedido + "no pudo entrar con la api de detallepedido&nopedido ");
                    }
                    cont++;
                    log.EscribeLog("Pedidos procesados: " + cont);
                }
            }
            catch (Exception ex)
            {
                log.EscribeLog("error para procesar los pedidos "+ ex.Message); 
                return;
            }
        }
    }
}
