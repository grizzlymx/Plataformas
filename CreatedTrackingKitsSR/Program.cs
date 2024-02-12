using System;
using CreatedTrackingKitsSR.Data;
using CreatedTrackingKitsSR.Entities;
using CreatedTrackingKitsSR.Model;
using MySqlConnector;

namespace CreatedTrackingKitsSR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_CreatedTrackingKits_SR", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var proceso = new program();
            proceso.createTracking(consulta);
            log.EscribeLog("CreatedTrackingSR termino");
        }
    }
}
