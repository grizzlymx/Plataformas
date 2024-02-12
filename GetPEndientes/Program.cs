using GetPEndientes.Data;
using GetPEndientes.Entities;
using GetPEndientes.Model;
using System;

namespace GetPEndientes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            Claro_Sears CS = new Claro_Sears();
            CS.proceso();
            log.EscribeLog("proceso terminado de GetPEndientes");
            
            
        }
    }
}
