using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetLabel_LP.Model
{
    public class UpandInsert
    {
        public bool updatemulti(string sale_order_id, int numguias)
        {
            var cn = new clsConexion();
            try
            {
                cn.EjecutaConsulta("UPDATE sale_order_header SET multiguia_flag = " + numguias + " WHERE sale_order_id = " + sale_order_id,
                    new MySqlParameter[] { }, CommandType.Text);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool updatecountlabel(string sale_order_id, string numguias)
        {
            var cn = new clsConexion();
            try
            {
                cn.EjecutaConsulta("UPDATE sale_order_header SET count_label = '" + numguias + "' WHERE sale_order_id = " + sale_order_id,
                    new MySqlParameter[] { }, CommandType.Text);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void InsertTrackingNumber(string sale_order_id, string tracking_number, string base64String, string carrier)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            try
            {
                cn.EjecutaConsulta("INSERT INTO sale_order_tracking_number (sale_order_id,tracking_number,tracking_base64,date_created) VALUES(@sale_order_id, @tracking_number, @base64String, @data_created)",
                          new[]
                          {
                             new MySqlParameter("@sale_order_id", sale_order_id),
                             new MySqlParameter("@tracking_number", tracking_number),
                             new MySqlParameter("@base64String", base64String),
                             new MySqlParameter("@data_created", DateTime.Now)

                          }, CommandType.Text);

                cn.EjecutaConsulta("INSERT INTO shipping_label_info (sale_order_id, tracking_number,carrier_name,service_type,shipment_cost ,shipment_type, user_id, date_created) VALUES(@sale_order_id, @tracking_number,@carrier_name,@service_type ,@shipment_cost, @shipment_type, @user_id, @date_created)", new[]
                   {
                                        new MySqlParameter("@sale_order_id",sale_order_id),
                                           new MySqlParameter("@tracking_number",tracking_number),
                                           new MySqlParameter("@carrier_name", carrier),
                                           new MySqlParameter("@service_type","standard"),
                                           new MySqlParameter("@shipment_cost","0"),
                                           new MySqlParameter("@shipment_type",1),
                                           new MySqlParameter("@user_id",1),
                                           new MySqlParameter("@date_created", DateTime.Now)
                                    }, CommandType.Text);

                cn.EjecutaConsulta("UPDATE sale_order_header SET tracking_number = @tracking_number WHERE sale_order_id = @sale_order_id",
             new[] {
                                   new MySqlParameter("@tracking_number", tracking_number),
                                   new MySqlParameter("@sale_order_id", sale_order_id)

            }, CommandType.Text);
            }
            catch (Exception ex)
            {
                log.EscribeLog("Error al insertar ");
            }
        }

        public string Tracking(string tracking)
        {
            var cn = new clsConexion();
            try
            {
                var dt = cn.TraeDataTable("SELECT count(*) AS reg,sale_order_id FROM sale_order_tracking_number WHERE tracking_number =@tracking",
                new[]
                {
                    new MySqlParameter("@tracking", tracking)
                }, CommandType.Text);
                var dr = dt.Rows[0];
                var reg = dr["reg"].ToString();
                return reg;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
