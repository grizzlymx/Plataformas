using MySqlConnector;
using System;
using CreateTracking_WrongCS.Data;
using CreateTracking_WrongCS.Entities;
using CreateTracking_WrongCS.Model;

namespace CreateTracking_WrongCS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var consulta = cn.TraeDataTable("Call SP_CreatedTracking_WrongCS", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var proceso = new program();
            proceso.createTracking(consulta);
            log.EscribeLog("CreatedTracking_WrongCS termino");
        }
    }
}
