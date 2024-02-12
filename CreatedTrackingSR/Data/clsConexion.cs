using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatedTrackingSR.Data
{
    public class clsConexion
    {
        private string conexion;
        public clsConexion()
        {
            conexion = ConfigurationManager.ConnectionStrings["db_conn"].ConnectionString;
        }

        public int EjecutaConsulta(string consulta, MySqlParameter[] parametros, CommandType tipo)
        {
            try
            {
                var cn = new MySqlConnection(conexion);
                var valor = 0;
                cn.Open();
                var cmd = new MySqlCommand(consulta, cn)
                {
                    CommandTimeout = 0,
                    CommandType = tipo
                };
                cmd.Parameters.AddRange(parametros);
                valor = cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public DataTable TraeDataTable(string consulta, MySqlParameter[] parametros, CommandType tipo)
        {
            var cn = new MySqlConnection(conexion);
            var dt = new DataTable();
            cn.Open();
            var cmd = new MySqlCommand(consulta, cn)
            {
                CommandTimeout = 0,
                CommandType = tipo
            };
            cmd.Parameters.AddRange(parametros);
            var rd = cmd.ExecuteReader();
            dt.Load(rd);
            rd.Close();
            cn.Close();
            cn.Dispose();
            return dt;
        }
        public DataSet TraeDataSet(string consulta, MySqlParameter[] parametros, CommandType tipo)
        {
            var cn = new MySqlConnection(conexion);
            var dt = new DataSet();
            cn.Open();
            var adap = new MySqlDataAdapter(consulta, cn)
            {
                SelectCommand =
               {
                  CommandTimeout = 0,
                  CommandText = consulta
               }
            };
            adap.SelectCommand.Parameters.AddRange(parametros);
            adap.Fill(dt);
            cn.Close();
            cn.Dispose();
            return dt;
        }
    }
}
