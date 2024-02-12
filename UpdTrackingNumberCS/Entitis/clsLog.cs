using UpdTrackingNumberCS.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdTrackingNumberCS.Entities
{
    public class clsLog
    {
        public void EscribeLog(string mensaje)
        {
            var texto = DateTime.Now.ToString("s") + " " + mensaje;
            Console.WriteLine(texto);
            var ruta = ConfigurationManager.AppSettings["CarpetaLog"];
            ruta = Path.Combine(ruta, "Log");
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
            ruta = Path.Combine(ruta, DateTime.Now.Year.ToString());
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
            ruta = Path.Combine(ruta, DateTime.Now.Month.ToString());
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
            ruta = Path.Combine(ruta, "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            var sw = new StreamWriter(ruta, true, Encoding.UTF8);
            sw.WriteLine(texto);
            sw.Close();
        }
        public void AgregaPcgRequest(string request_str, string response_str, string purchase_order_id, string sale_order_id, string source)
        {
            var cn = new clsConexion();
            cn.EjecutaConsulta("INSERT INTO pcg_request_platform  (request_str, response_str, purchase_order_id, sale_order_id, date_created, source, user_id) VALUES (@request_str, @response_str, @purchase_order_id, @sale_order_id, NOW(), @source, 1)",
               new[]
               {
               new MySqlParameter("@request_str",request_str),
               new MySqlParameter("@response_str",response_str),
               new MySqlParameter("@purchase_order_id",purchase_order_id),
               new MySqlParameter("@sale_order_id",sale_order_id),
               new MySqlParameter("@source",source)
               }, CommandType.Text);
        }
    }
}
