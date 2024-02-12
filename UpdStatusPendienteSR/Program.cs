using MySqlConnector;
using System;
using UpdStatusPendienteSR.Data;
using UpdStatusPendienteSR.Entities;
using UpdStatusPendienteSR.Model;

namespace UpdStatusPendienteSR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var Consulta = cn.TraeDataTable("Call SP_UpdStatusPendiente_SR()", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var pedido = new process();
            pedido.proceso(Consulta);
            log.EscribeLog("Termino proceso UpdStatusPendientesSR");
        }
    }
}
