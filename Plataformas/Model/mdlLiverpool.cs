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
using PdfSharp.Drawing;
using System.Text.RegularExpressions;
using System.Threading;

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

                var client = new RestClient(url + reference_order_number);
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
        public string CountProduct(int product_id, int count_items, int qty)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var quantity_label = string.Empty;
            var quantity_product_label = string.Empty;
            var count_label = string.Empty;
            try
            {
                var dt2 = cn.TraeDataTable("SELECT product_id, quantity_label,quantity_product_label FROM product WHERE product_id = @product_id",
                   new[] { new MySqlParameter("@product_id", product_id) }, CommandType.Text);
                var dr2 = dt2.Rows[0];
                quantity_label = dr2["quantity_label"].ToString();
                quantity_product_label = dr2["quantity_product_label"].ToString();
                if (quantity_label == null || quantity_label == "" || quantity_product_label == null || quantity_product_label == "")
                {
                   if (quantity_label == null && quantity_product_label == null || quantity_label == "" && quantity_product_label == "")
                    {
                        quantity_label = "1";
                        quantity_product_label = "1";
                    }
                   else
                        if(quantity_product_label == null || quantity_product_label == "")
                        {
                            quantity_product_label= "1";
                        }
                        else
                        {
                            if(quantity_label == null || quantity_label == "")
                            {
                                quantity_label = "1";
                            }
                        }

                }
                var qty_laber = int.Parse(quantity_label);
                var qty_product_laber = int.Parse(quantity_product_label);

                //modificar la validacion
                if (count_items == 0)
                {
                    count_label = "1";
                }
                else
                    if (qty == 1 && qty_laber == 1)
                {
                    count_label = "1";
                }
                else
                 if (qty_laber >= qty && qty_product_laber == qty)
                {
                    count_label = qty_laber.ToString();
                }
                else
                    if (qty > 1 && qty_laber == 1)
                {
                    decimal total = qty / qty_product_laber;
                    if (total == 0)
                    {
                        count_label = "1";
                    }
                    else
                    {
                        count_label = total.ToString();
                    }
                }
                else
                {
                    decimal total = qty / qty_product_laber;
                    if (total == 0)
                    {
                        count_label = "1";
                    }
                    else
                    {
                        count_label = total.ToString();
                    }
                }
                log.EscribeLog("valor count_label:" + count_label);

                return count_label;
            }
            catch (Exception)
            {
                count_label = "1";
                log.EscribeLog("valor count_label:" + count_label);
                return count_label;
            }

        }
        public string GetLabelLiverpoolCarritos(clsGetDocuments respons, string count_product, string so, int qty, int multi, string carrier, int count_items, string resultado)
        {
            try
            {
                var url = string.Empty;
                var count = 1;
                var count_2 = 1;
                var countlabel = string.Empty;
                var cn = new clsConexion();
                var log = new clsLog();
                var UandI = new UpandInsert();
                var insert = false;
                var countInser = 0;
                var counts = respons.order_documents.Count;

                for (var i = 0; i < counts; i++)
                {
                    if (respons.order_documents[i].type != "SYSTEM_DELIVERY_BILL")
                    {
                        var id = respons.order_documents[i].id;
                        string file_name = respons.order_documents[i].file_name;
                        string result = file_name.Replace(".pdf", "");
                        if (count_2 <= int.Parse(count_product))
                        {
                            //casteo
                            var carrier_Casteo = UandI.val_tracking(result, so,1);
                            if(carrier_Casteo == carrier)
                            {
                                log.EscribeLog("**************************");
                                log.EscribeLog("Id del pedido: " + id);
                                var zplandpdf = DownsZPL(id);
                                //Bloque para sacar el tracking
                                if (count_items > 1 || multi == 1)
                                {
                                    switch (carrier)
                                    {
                                        case "FEDEX":
                                            /*int startIndex = zplandpdf.IndexOf(">;") + 24;
                                            resultado = zplandpdf.Substring(startIndex, 12);*/
                                            string caracteres = @"\>;(.*?)\^FS";
                                            Match mat = Regex.Match(zplandpdf, caracteres);
                                            var Tracking = mat.Groups[1].Value;
                                            resultado = Tracking.Substring(22, 12);
                                            break;
                                        case "ESTAFETA":
                                            /*int InitIndex = zplandpdf.IndexOf("^BY3,3") + 24;
                                            resultado = zplandpdf.Substring(InitIndex, 12);*/
                                            //string patron = @"\^BY3,3(.*?)\^FS";
                                            //Match match = Regex.Match(zplandpdf, patron);
                                            //var cadena = match.Groups[1].Value;
                                            //resultado = cadena.Substring(26, 22);
                                            string patron = @"FD\w{22}";
                                            Match match = Regex.Match(zplandpdf, patron);
                                            if (match.Success)
                                            {
                                                resultado = match.Value.Substring(2);
                                            }
                                            else
                                            {
                                                log.EscribeLog("no se encontro el patron");
                                            }
                                            break;
                                        case "UPS":
                                            string caracter = @"\^FO66(.*?)\^FS";
                                            Match matc = Regex.Match(zplandpdf, caracter);
                                            var track = matc.Groups[1].Value;
                                            resultado = track.Substring(27, 18);
                                            break;
                                    }
                                }
                                log.EscribeLog("Carrier name: " + carrier);
                                log.EscribeLog("tracking: " + resultado);
                                //compara si existe el tracking en la tabla
                                var reg = UandI.Tracking(resultado);
                                if (reg == "0")
                                {
                                    var Base64 = string.Empty;
                                    if (multi == 1)
                                    {
                                        if (count <= qty)
                                        {
                                            Base64 = LabelPDF(zplandpdf);
                                            if (Base64 != null)
                                            {
                                                UandI.InsertTrackingNumber(so, resultado, Base64, carrier);
                                                countInser = countInser + 1;
                                                countlabel = countInser + "/" + count_product;
                                                log.EscribeLog("Núm_guias: " + countlabel);
                                                UandI.updatecountlabel(so, countlabel);
                                                count++;
                                            }
                                            else
                                            {
                                                log.EscribeLog("No bajo la guia");
                                            }
                                        }
                                        UandI.updatemulti(so, qty);
                                    }
                                    else
                                    {
                                        Base64 = LabelPDF(zplandpdf);
                                        if (Base64 != null)
                                        {
                                            UandI.InsertTrackingNumber(so, resultado, Base64, carrier);
                                            countInser = countInser + 1;
                                            countlabel = countInser + "/" + count_product;
                                            log.EscribeLog("Núm_guias: " + countlabel);
                                            UandI.updatecountlabel(so, countlabel);
                                            count++;
                                        }
                                        else
                                        {
                                            log.EscribeLog("No bajo la guia");
                                        }
                                        if (int.Parse(count_product) > 1)
                                        {
                                            UandI.updatemulti(so, countInser);
                                        }
                                    }
                                    count_2++;
                                }
                                else
                                {
                                    log.EscribeLog("Existe el tracking en la tabla " + resultado);
                                }
                            }
                        }
                    }
                }
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetLabelLiverpool(clsGetDocuments respons, string count_product, string so, int qty, int multi, string carrier, int count_items, string resultado)
        {
            try
            {
                var url = string.Empty;
                var count = 1;
                var count_2 = 1;
                var countlabel = string.Empty;
                var cn = new clsConexion();
                var log = new clsLog();
                var UandI = new UpandInsert();
                var insert = false;
                var countInser = 0;
                
                var counts = respons.order_documents.Count ;

                for (var i = 0; i < counts;i++)
                {
                    if (respons.order_documents[i].type != "SYSTEM_DELIVERY_BILL")
                    {
                        var id = respons.order_documents[i].id;
                        string file_name = respons.order_documents[i].file_name;
                        string result = file_name.Replace(".pdf", "");
                        var carrier_Casteo = UandI.val_tracking(result, so, 1);
                        if (carrier_Casteo == carrier)
                        {
                            log.EscribeLog("**************************");
                            log.EscribeLog("Id del pedido: " + id);
                            var zplandpdf = DownsZPL(id);
                            if (count_items > 1 || multi == 1 || int.Parse(count_product) >= 2)
                            {

                                switch (carrier)
                                {
                                    case "FEDEX":
                                        /*int startIndex = zplandpdf.IndexOf(">;") + 24;
                                        resultado = zplandpdf.Substring(startIndex, 12);*/
                                        string caracteres = @"\>;(.*?)\^FS";
                                        Match mat = Regex.Match(zplandpdf, caracteres);
                                        var Tracking = mat.Groups[1].Value;
                                        resultado = Tracking.Substring(22, 12);
                                        break;
                                    case "ESTAFETA":
                                        /*int InitIndex = zplandpdf.IndexOf("^BY3,3") + 24;
                                        resultado = zplandpdf.Substring(InitIndex, 12);*/
                                        //string patron = @"\^BY3,3(.*?)\^FS";
                                        //Match match = Regex.Match(zplandpdf, patron);
                                        //var cadena = match.Groups[1].Value;
                                        //resultado = cadena.Substring(26, 22);
                                        string patron = @"FD\w{22}";
                                        Match match = Regex.Match(zplandpdf, patron);
                                        if (match.Success)
                                        {
                                            resultado = match.Value.Substring(2);
                                        }
                                        else
                                        {
                                            log.EscribeLog("no se encontro el patron");
                                        }
                                        break;
                                    case "UPS":
                                        string caracter = @"\^FO66(.*?)\^FS";
                                        Match matc = Regex.Match(zplandpdf, caracter);
                                        var track = matc.Groups[1].Value;
                                        resultado = track.Substring(27, 18);
                                        break;
                                }
                            }
                            //Bloque para sacar el tracking
                            log.EscribeLog("Carrier name: " + carrier);
                            log.EscribeLog("tracking: " + resultado);
                            //compara si existe el tracking en la tabla
                            var reg = UandI.Tracking(resultado);
                            if (reg == "0")
                            {
                                var Base64 = string.Empty;
                                if (multi == 1)
                                {
                                    if (count <= qty)
                                    {
                                        Base64 = LabelPDF(zplandpdf);
                                        if (Base64 != null)
                                        {
                                            UandI.InsertTrackingNumber(so, resultado, Base64, carrier);
                                            countInser = countInser + 1;
                                            countlabel = countInser + "/" + count_product;
                                            log.EscribeLog("Núm_guias: " + countlabel);
                                            UandI.updatecountlabel(so, countlabel);
                                            count++;
                                        }
                                        else
                                        {
                                            log.EscribeLog("No bajo la guia");
                                        }
                                    }
                                    UandI.updatemulti(so, qty);
                                }
                                else
                                {
                                    Base64 = LabelPDF(zplandpdf);
                                    if (Base64 != null)
                                    {
                                        UandI.InsertTrackingNumber(so, resultado, Base64, carrier);
                                        countInser = countInser + 1;
                                        countlabel = countInser + "/" + count_product;
                                        log.EscribeLog("Núm_guias: " + countlabel);
                                        UandI.updatecountlabel(so, countlabel);
                                        count++;
                                    }
                                    else
                                    {
                                        log.EscribeLog("No bajo la guia");
                                    }
                                    if (int.Parse(count_product) > 1)
                                    {
                                        UandI.updatemulti(so, countInser);
                                    }
                                }
                                count_2++;
                            }
                            else
                            {
                                log.EscribeLog("Existe el tracking en la tabla " + resultado);
                            }
                        }
                            
                    }
                }
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        public string DownsZPL(int id)
        {
            var cn =new clsConexion();
            var log = new clsLog();
            var url = string.Empty;
            try
            {
                var init = 0;
                var stop = 5;
                while (init < stop)
                {
                    init++;
                    var dt = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name ='ENDPOINT_LP_PANAGEA_GETLABEL_DOWNS'", new MySqlParameter[] { }, CommandType.Text);
                    var dr = dt.Rows[0];
                    url = dr["value"].ToString();

                    var client = new RestClient(url + id);
                    var request = new RestRequest("", Method.GET, DataFormat.Json);
                    request.AddHeader("Authorization", toke);
                    var response = client.Execute(request);
                    var zplandpdf = response.Content;
                    if(zplandpdf != null)
                    {
                        return zplandpdf;
                    }
                    else
                    {
                        log.EscribeLog("intento " + init);
                        Thread.Sleep(10000);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string LabelPDF(string ZPL)
        {
            var log = new clsLog();
            try
            {
                var init = 0;
                var stop = 5;
                while(init < stop)
                {
                    init++;
                    byte[] zpl = Encoding.UTF8.GetBytes(ZPL);

                    var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/5x8/0/");
                    request.Method = "POST";
                    request.Accept = "application/pdf";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = zpl.Length;

                    var requestStream = request.GetRequestStream();
                    requestStream.Write(zpl, 0, zpl.Length);
                    requestStream.Close();

                    var response = (HttpWebResponse)request.GetResponse();
                    var responseStream = response.GetResponseStream();
                    byte[] bytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        responseStream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    string Base64 = Convert.ToBase64String(bytes);
                    responseStream.Close();
                    if(Base64 != null)
                    {
                        return Base64;
                    }
                    else
                    {
                        log.EscribeLog("La api de LabelPDF no respondio intento "+ init);
                        Thread.Sleep(10000);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public GetOrders GetOrder(string reference)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var url = string.Empty;

            try
            {
                var init = 0;
                var stop = 3;
                while(init < stop)
                {
                    init++;
                    Thread.Sleep(1000);
                    var dt = cn.TraeDataTable("SELECT `value` FROM m_configuration WHERE `name` ='ENDPOINT_ORDERS_LP'", new MySqlParameter[] { }, CommandType.Text);
                    var dr = dt.Rows[0];
                    url = dr["value"].ToString();

                    var client = new RestClient(url +"?order_ids="+reference);
                    var request = new RestRequest("", Method.GET, DataFormat.Json);
                    request.AddHeader("Authorization", toke);
                    var response = client.Execute(request);
                    
                    if (response != null )
                    {
                        var respuesta = JsonConvert.DeserializeObject<GetOrders>(response.Content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        return respuesta;
                    }
                    log.EscribeLog("intento: " + init);
                }
                return null;
            }
            catch (Exception ex)
            {
                log.EscribeLog("GetOrders no respondio");
                return null;
            }

        }

        public int Check(clsGetDocuments respons,string carrier,string so)
        {
            var cn = new clsLog();
            var log = new clsLog();
            var UandI = new UpandInsert();
            var count = 0;
            try
            {
                var counts = respons.order_documents.Count;
                log.EscribeLog("~~~~~~~~~~~~~~~~~~~~");
                log.EscribeLog("Conteo de guias");
                var resultado = string.Empty;
                List<string> variables = new List<string>();
                for (var i = 0; i < counts; i++)
                {
                    string tracking_compare = resultado;
                    if (respons.order_documents[i].type != "SYSTEM_DELIVERY_BILL")
                    {
                        var id = respons.order_documents[i].id;
                        string file_name = respons.order_documents[i].file_name;
                        string result = file_name.Replace(".pdf", "");
                        var carrier_Casteo = UandI.val_tracking(result, so, 1);
                        if (carrier_Casteo == carrier)
                        {
                            var zplandpdf = DownsZPL(id);
                            switch (carrier)
                            {
                                case "FEDEX":
                                    /*int startIndex = zplandpdf.IndexOf(">;") + 24;
                                    resultado = zplandpdf.Substring(startIndex, 12);*/
                                    string caracteres = @"\>;(.*?)\^FS";
                                    Match mat = Regex.Match(zplandpdf, caracteres);
                                    var Tracking = mat.Groups[1].Value;
                                    resultado = Tracking.Substring(22, 12);
                                    break;
                                case "ESTAFETA":
                                    /*int InitIndex = zplandpdf.IndexOf("^BY3,3") + 24;
                                    resultado = zplandpdf.Substring(InitIndex, 12);*/
                                    //string patron = @"\^BY3,3(.*?)\^FS";
                                    //Match match = Regex.Match(zplandpdf, patron);
                                    //var cadena = match.Groups[1].Value;
                                    //resultado = cadena.Substring(26, 22);
                                    string patron = @"FD\w{22}";
                                    Match match = Regex.Match(zplandpdf, patron);
                                    resultado = match.Value.Substring(2);
                                    break;
                                case "UPS":
                                    string caracter = @"\^FO66(.*?)\^FS";
                                    Match matc = Regex.Match(zplandpdf, caracter);
                                    var track = matc.Groups[1].Value;
                                    resultado = track.Substring(27, 18);
                                    break;
                            }
                            variables.Add(resultado);
                        }
                        
                    }
                }
                //Generamos una lista y la vamos a compara para ver la info 

                HashSet<string> set = new HashSet<string>();//Usar un HashSet para verificar duplicados
                List<string> duplicador = new List<string>();
                List<string> noduplicados = new List<string>();
                foreach(var item in variables)
                {
                    if (!set.Add(item))
                    {
                        duplicador.Add(item);
                    }
                    else
                    {
                        noduplicados.Add(item);
                    }
                }
                return noduplicados.Count;
                //if(duplicador.Count > 0)
                //{
                //    log.EscribeLog("Se encontraron duplicados");
                //    return false;
                //}
                //else
                //{
                //    log.EscribeLog("no hay duplicados");
                //    var pedido = variables.Count;
                //    return true;
                //}
            }
            catch (Exception ex)
            {
                log.EscribeLog("Error al checar el conteo de las guias que hay");
                return 0;
            }
        }
    }
}
