using MySqlConnector;
using System;
using UpdTrackingNumber_SR.Model;
using UpdTrackingNumber_SR.Data;
using UpdTrackingNumber_SR.Entities;

namespace UpdTrackingNumber_SR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_UpdTrackingNumber_SR", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var progeso = new process();
            progeso.ProcessTracking(consulta);
            log.EscribeLog("Proceso terminado de UpdTrackingNumber SR");
        }
    }
}
