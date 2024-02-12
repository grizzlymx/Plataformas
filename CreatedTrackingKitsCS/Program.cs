using System;
using CreatedTrackingKitsCS.Data;
using CreatedTrackingKitsCS.Entities;
using CreatedTrackingKitsCS.Model;
using MySqlConnector;

namespace CreatedTrackingKitsCS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_CreatedTrackingKitsCS", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var proceso = new program();
            proceso.createTracking(consulta);
            log.EscribeLog("CreatedTrackingCS termino");
        }
    }
}
