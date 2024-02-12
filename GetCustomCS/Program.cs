using GetCustomCS.Data;
using GetCustomCS.Entities;
using GetCustomCS.Model;
using MySqlConnector;
using System;

namespace GetCustomCS
{
    public class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_GetCustomCS", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var proceso = new Processor();
            proceso.GetCustom(consulta);
            log.EscribeLog("Termino proceso de GetCustomCS");
        }
    }
}
