using MySqlConnector;
using System;
using UpdStatusPendienteCS.Data;
using UpdStatusPendienteCS.Entities;
using UpdStatusPendienteCS.Model;

namespace UpdStatusPendienteCS
{
    public class Program
    {
        static void Main(string[] args)
        {
            var cn = new  clsConexion();
            var log = new clsLog();
            var Consulta= cn.TraeDataTable("Call SP_UpdStatusPendiente_CS()", new MySqlParameter[] { }, System.Data.CommandType.Text);
            var pedido = new process();
            pedido.proceso(Consulta);
            log.EscribeLog("Termino proceso UpdStatusPendientesCS");
        }
    }
}
