using GetLabel_LP;
using GetLabel_LP.Model;
using GetLabel_LP.Services;
using MySqlConnector;
using System;
using System.Configuration;
using System.Data;
using System.Threading;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Plataformas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var log = new clsLog();
            var cont = 0; 
            var cn = new clsConexion();

            try
            {
                var query = ConfigurationManager.AppSettings["SPConsulta"];
                var data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);

                log.EscribeLog(data.Rows.Count + " pedidos");
                foreach ( DataRow order in data.Rows )
                {
                    
                    log.EscribeLog("-----------------");
                    var so = order["sale_order_id"].ToString();
                    var sale_order = "so:" + order["sale_order_id"];
                    var sku = "sku: " + order["product_id"];
                    var sellerku = "sku: " + order["seller_sku"];
                    var qty = int.Parse(order["quantity"].ToString());
                    var product_id = int.Parse(order["product_id"].ToString());
                    var count_items = int.Parse(order["count_items"].ToString());
                    var reference = order["reference_order_number"].ToString().Substring(0, 12);
                    var count_items_2 = int.Parse(order["count_items_2"].ToString());
                    var shipping_id_number = order["shipping_id_number"].ToString();
                    var multi = int.Parse(order["multiguia"].ToString());
                    log.EscribeLog("soid: " + order["sale_order_id"]);
                    log.EscribeLog("reference_order_number: " + reference);
                    log.EscribeLog("qty: " + order["quantity"]);
                    log.EscribeLog("count_items: " + count_items);
                    log.EscribeLog("seller_sku: " + sellerku);
                    log.EscribeLog("product_id: " + product_id); 
                    var modelLiver = new mdlLiverpool();
                    modelLiver.Login();
                    var conteo= 0;
                    var type_documents = string.Empty;
                    var Type_Document = modelLiver.GetDocuments(reference, so);
                    var counts = Type_Document.order_documents.Count;
                    var count_product = modelLiver.CountProduct(product_id, count_items, qty);
                    var GetOrders = modelLiver.GetOrder(reference);
                    try
                    {
                        if (GetOrders != null)
                        {
                            if(GetOrders.orders[0].shipping_company != null && GetOrders.orders[0].shipping_tracking != null)
                            {
                                var name_carrier = GetOrders.orders[0].shipping_company.ToString();
                                var tracking_number_get = GetOrders.orders[0].shipping_tracking.ToString();
                                for (var i = 0; i < counts; i++)
                                {
                                    if (Type_Document.order_documents[i].type != "SYSTEM_DELIVERY_BILL")
                                    {
                                        conteo++;
                                    }
                                }
                                if (count_items > 1)
                                {
                                    if (conteo > qty)
                                    {
                                        modelLiver.GetLabelLiverpoolCarritos(Type_Document, count_product, so, qty, multi, name_carrier, count_items, tracking_number_get);
                                    }
                                }
                                else
                                if (count_items == 1)
                                {
                                    if (conteo >= int.Parse(count_product))
                                    {
                                        modelLiver.GetLabelLiverpool(Type_Document, count_product, so, qty, multi, name_carrier, count_items, tracking_number_get);
                                    }
                                }
                            }
                            else
                            {
                                log.EscribeLog("El Tracking esta nulo");
                            }
                        }
                        else
                        {
                            log.EscribeLog("aun no tiene guias");
                        }
                    }
                    catch (Exception e)
                    {
                        log.EscribeLog("Error al consumir el GetOrders");

                    }
                }
            }
            catch (Exception ex) 
            {
                log.EscribeLog("error en: " + ex.Message);
            }
        }
    }
}
