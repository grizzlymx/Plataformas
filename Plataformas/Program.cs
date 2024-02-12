using GetLabel_LP;
using GetLabel_LP.Model;
using MySqlConnector;
using System;
using System.Configuration;
using System.Data;

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
                    log.EscribeLog("soid: " + order["sale_order_id"]);
                    log.EscribeLog("reference_order_number: " + reference);
                    log.EscribeLog("qty: " + order["quantity"]);
                    log.EscribeLog("count_items: " + count_items);
                    log.EscribeLog("seller_sku: " + sellerku);
                    log.EscribeLog("product_id: " + product_id);
                    
                    var tracking_number = string.Empty;
                    var pdf_base64 = string.Empty;
                    var modelLiver = new mdlLiverpool();
                    modelLiver.Login();
                    var type_documents = string.Empty;
                    var Type_Document = modelLiver.GetDocuments(reference, so);
                    type_documents = Type_Document.order_documents[0].type;
                    log.EscribeLog("type_document: "+ Type_Document.order_documents[0].type);

                    if (type_documents != "SYSTEM_DELIVERY_BILL")
                    {
                        pdf_base64 = modelLiver.GetLabelLiverpool(reference, Type_Document, product_id, so, qty, count_items);
                    }
                    else
                    {
                        log.EscribeLog("El documento aun no tiene la guia se encuentra diferente de DELIVERY " + so);
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
