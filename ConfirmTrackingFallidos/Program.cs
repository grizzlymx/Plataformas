using ConfirmTrackingFallidos.Data;
using ConfirmTrackingFallidos.Entities;
using ConfirmTrackingFallidos.Model;
using System;

namespace ConfirmTrackingFallidos
{
    public class Program
    {
        static void Main(string[] args)
        {
            var cn = new clsConexion();
            var log = new clsLog();
            var proceso = new process();
            proceso.embarque();
            
            
        }
    }
}
