using CreatedTrackingCS.Model;
using CreatedTrackingSR.Data;
using CreatedTrackingSR.Entities;
using MySqlConnector;
using System;

namespace CreatedTrackingSR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_CreatedTrackingSR", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var proceso = new program();
            proceso.createTracking(consulta);
            log.EscribeLog("CreatedTrackingSR termino");


        }
    }
}
