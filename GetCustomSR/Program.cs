using GetCustomSR.Data;
using GetCustomSR.Entities;
using GetCustomSR.Model;
using MySqlConnector;
using System;

namespace GetCustomSR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_GetCustomSR", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var proceso = new Processor();
            proceso.GetCustom(consulta);
            log.EscribeLog("Termino proceso de GetCustomSR");
        }
    }
}
