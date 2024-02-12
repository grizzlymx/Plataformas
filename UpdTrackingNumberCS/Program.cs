using MySqlConnector;
using System;
using UpdTrackingNumberCS.Data;
using UpdTrackingNumberCS.Entities;
using UpdTrackingNumberCS.Model;

namespace UpdTrackingNumberCS
{
    public class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_UpdTrackingNumber_CS", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var progeso = new process();
            progeso.ProcessTracking(consulta);
            log.EscribeLog("Proceso terminado de UpdTrackingNumber CS");

        }
    }
}
