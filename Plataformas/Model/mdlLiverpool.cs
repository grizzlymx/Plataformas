using GetLabel_LP.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySqlConnector;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Net;
using PdfReader = iText.Kernel.Pdf.PdfReader;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;

namespace GetLabel_LP.Model
{
    public class mdlLiverpool
    {
        public string toke { get; set; }

        public string Login()
        {
            try
            {
                var cn = new clsConexion();
                var dt = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENPOINT_ACCESS_TOKEN_LIVERPOOL'", new MySqlParameter[] { }, CommandType.Text);
                var drtoken = dt.Rows[0];
                toke = drtoken["value"].ToString();

            }
            catch (Exception ex)
            {

                toke = null;
            }
            return toke;
        }

        public clsGetDocuments GetDocuments(string reference_order_number, string sale_order_id)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var url = string.Empty;
            var reques = string.Empty;
            var val_request = string.Empty;
            try
            {
                var dt = cn.TraeDataTable("SELECT value FROM m_configuration  WHERE name = 'ENDPOINT_LP_PANAGEA_GETDOCUMENTS'", new MySqlParameter[] { }, CommandType.Text);
                var dr = dt.Rows[0];
                url = dr["value"].ToString();

                var client = new RestClient(url+ reference_order_number);
                var request = new RestRequest("", Method.GET, DataFormat.Json);
                request.AddHeader("Authorization", toke);
                var response = client.Execute(request);
                var respuesta = JsonConvert.DeserializeObject<clsGetDocuments>(response.Content);

                try
                {
                    var dt_pcg = cn.TraeDataTable("SELECT request_str from pcg_request where sale_order_id =@sale_order_id and request_str =@request_str",
                    new[]
                    {
                    new MySqlParameter("@sale_order_id", sale_order_id),
                    new MySqlParameter("@request_str", response.Content)
                    }, CommandType.Text);
                    var dr_pcg = dt_pcg.Rows[0];
                    val_request = dr_pcg["request_str"].ToString();
                }
                catch (Exception ex)
                {
                    val_request = null;
                }

                if (val_request == null)
                {
                    cn.EjecutaConsulta("INSERT INTO pcg_request (request_str, response_str, purchase_order_id, sale_order_id, date_created, source,user_id) VALUES (@request_str, @response_str, @purchase_order_id, @sale_order_id, @date_created, @source, @user_id)", new[]
                    {
                        new MySqlParameter("@request_str",client.BaseUrl.AbsoluteUri),
                        new MySqlParameter("@response_str",response.Content),
                        new MySqlParameter("@purchase_order_id",reference_order_number),
                        new MySqlParameter("@sale_order_id",sale_order_id),
                        new MySqlParameter("@date_created", DateTime.Now),
                        new MySqlParameter("@source","Coppel-GetDocuments"),
                        new MySqlParameter("@user_id", 1)
                        }, CommandType.Text);

                }
                else
                {
                    log.EscribeLog("Ya exite un registro en la tabla pgc_request con la misma información");
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                log.EscribeLog("error en la api GETDOCUMENTS");
                return null;
            }
        }

        public string GetLabelLiverpool(string reference_order_number, clsGetDocuments getdocuments, int product_id, string sale_order_id, int qty, int count_items)
        {
            try
            {
                var url = string.Empty;
                var pdf = string.Empty;
                var count_label = string.Empty;
                var trackin_number = string.Empty;
                var package_quantity = string.Empty;
                var numguias = 0;
                var cant_packquantity = 0;
                var count = 0;
                var countlabel = string.Empty;
                var cn = new clsConexion();
                var log = new clsLog();

                var dt = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name ='ENDPOINT_LP_PANAGEA_GETLABEL_DOWNS'", new MySqlParameter[] { }, CommandType.Text);
                var dr = dt.Rows[0];
                url = dr["value"].ToString();

                try
                {//product
                    var dt2 = cn.TraeDataTable("select product_count from product_count_label where product_id=@product_id",
                    new[] { new MySqlParameter("@product_id", product_id) }, CommandType.Text);
                    var dr2 = dt2.Rows[0];
                    count_label = dr2["product_count"].ToString();
                }
                catch (Exception)
                {
                    count_label = "0";
                }
                log.EscribeLog("valor count_label:" + count_label);

                var type = getdocuments.order_documents[0].type;
                if(type == "Guia")
                {
                    var client = new RestClient(url + reference_order_number + "&document_codes=" + type);
                    var request = new RestRequest("", Method.GET, DataFormat.Json);
                    request.AddHeader("Authorization", toke);
                    var response = client.Execute(request);
                    var zplandpdf = response.Content;

                    string base64 = ConvertToBase64(zplandpdf);

                    static string ConvertToBase64(string input)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(input);
                        return Convert.ToBase64String(bytes);
                    }



                    log.EscribeLog("convertidor");
                    //var archive = new ZipArchive(new MemoryStream(response.RawBytes));

                    //log.EscribeLog("valor Entries: " + archive.Entries.Count);
                    //var labelCount = archive.Entries.Count; //compararlo
                    //var cont = 0;
                    //var insert = false;
                    //var countInsert = 0;
                    //var valcount_reg = 0;

                    //if (count_items > 0)
                    //{
                    //    if (int.Parse(count_label) == 0)
                    //    {
                    //        foreach (var entry in archive.Entries)
                    //        {
                    //            if (entry.FullName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    //            {
                    //                var valtracking = entry.Name.Length;
                    //                if (valtracking == 26 || valtracking == 16 || valtracking == 22)
                    //                {
                    //                    log.EscribeLog("num.Tracking: " + entry.Name);
                    //                    var ms = entry.Open();
                    //                    //descarga .zip
                    //                    var bytes = new byte[entry.Length];
                    //                    ms.Read(bytes, 0, bytes.Length);
                    //                    pdf = Convert.ToBase64String(bytes);

                    //                    //var name = entry.Name.Split(@".pdf");
                    //                    //var tracking = name[0].Trim();

                    //                    //aqui va un insert  para el numero de guias que le faltan


                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (int.Parse(countlabel) > 0)
                    //        {

                    //        }
                    //    }
                    //}


                    return null;

                }
            }
               
            catch (Exception ex)
            {
                
                return null;
            }
        }
    }
}
