using MySqlConnector;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace GetLabel_LP
{
    public class clsLog
    {

        //este método genera la carpeta de log dentro del proyecto, se crean los archivos de log.txt
        // colocando las anomalias presentadas, en el flujo del proceso.
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

        public void AgregarComents(string sale_order_id, int type_comment, string comments, DateTime date_created, int id_user)
        {
            var cn = new clsConexion();
            cn.EjecutaConsulta("INSERT INTO sale_order_comments(sale_order_id,comment_type_id,comments,user_id,date_created) VALUES(@sale_order_id,@comment_type_id,@comments,@id_user,@fecha_create)",
                new[]
                {
                    new MySqlParameter("@sale_order_id", sale_order_id),
                    new MySqlParameter("@comment_type_id", type_comment),
                    new MySqlParameter("@comments", comments),
                    new MySqlParameter("@id_user", id_user),
                    new MySqlParameter("@fecha_create", date_created)
                }, CommandType.Text);
        }

        public void AgregarTrackingLog(string sale_order_id, string carrier_name, string tracking_number, string status_name, DateTime date_created, int cutomer_id, int type_comment)
        {
            var cn = new clsConexion();
            cn.EjecutaConsulta("insert into sale_order_tracking_Log(sale_order_id,carrier_name,tracking_number,status_name,date_created,Customer_id, id_type_comment)VALUES(@sale_order_id,@carrier_name,@tracking_number,@status_name,@date_created,@customer_id, @type_comment)",
                new[]
                {
                    new MySqlParameter("@sale_order_id", sale_order_id),
                    new MySqlParameter("@carrier_name", carrier_name),
                    new MySqlParameter("@tracking_number", tracking_number),
                    new MySqlParameter("@status_name", status_name),
                    new MySqlParameter("@date_created", date_created),
                    new MySqlParameter("@customer_id", cutomer_id),
                    new MySqlParameter("@type_comment", type_comment)
                }, CommandType.Text);
        }

        public void AgregarLog(DateTime fechaInicial, DateTime fechaFinal, string name, string clave, int count)
        {
            var cn = new clsConexion();
            cn.EjecutaConsulta("insert into tracking_Log(name_netcore,date_inicial,date_final,clave,count)VALUES(@name_netcore,@date_inicial,@date_final,@clave,@count)",
                new[]
                {
                    new MySqlParameter("@name_netcore", name),
                    new MySqlParameter("@date_inicial", fechaInicial),
                    new MySqlParameter("@date_final", fechaFinal),
                    new MySqlParameter("@clave",clave ),
                    new MySqlParameter("@count", count)

                }, CommandType.Text);
        }


        public void UpdateLog(DateTime fechaFinal, string clave, int cout_asignados)
        {
            var cn = new clsConexion();
            cn.EjecutaConsulta("Update tracking_Log set date_final = @date_final,count2=@count2  where clave =@clave ",
                new[]
                {

                    new MySqlParameter("@date_final", fechaFinal),
                    new MySqlParameter("@clave", clave),
                    new MySqlParameter("@count2", cout_asignados)

                }, CommandType.Text);
        }
    }
}