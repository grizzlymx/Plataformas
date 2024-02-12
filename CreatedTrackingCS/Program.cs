using CreatedTrackingCS.Data;
using CreatedTrackingCS.Entities;
using CreatedTrackingCS.Model;
using MySqlConnector;
using System;
using System.Diagnostics;

namespace CreatedTrackingCS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_CreatedTrackingCS", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var proceso = new program();
            proceso.createTracking(consulta);
            log.EscribeLog("CreatedTrackingCS termino");
            
            
        }
    }
}
