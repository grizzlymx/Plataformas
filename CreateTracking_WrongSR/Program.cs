using System;
using CreateTracking_WrongSR.Data;
using CreateTracking_WrongSR.Entities;
using CreateTracking_WrongSR.Model;
using MySqlConnector;

namespace CreateTracking_WrongSR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_CreatedTracking_WrongSR", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var proceso = new program();
            proceso.createTracking(consulta);
            log.EscribeLog("CreatedTracking_WrongSR termino");
        }
    }
}
