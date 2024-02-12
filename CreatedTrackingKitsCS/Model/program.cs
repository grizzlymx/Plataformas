using CreatedTrackingKitsCS.Data;
using CreatedTrackingKitsCS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatedTrackingKitsCS.Model
{
    public class program
    {
        public void createTracking(DataTable dataTable) 
        {
            var log = new clsLog();
            var cn = new clsConexion();
            var point = new EndPoint();
            try
            {
                var count = dataTable.Rows.Count;
                log.EscribeLog("pedidos: "+ count);
                foreach(DataRow row in dataTable.Rows)
                {
                    var cont = 1;
                    var nopedido = row[0].ToString();
                    var idpedido = row[2].ToString();
                    var sku = row[4].ToString();
                    log.EscribeLog("producto: " + nopedido + " id relacion: " + idpedido);
                    point.PostTracking(nopedido, idpedido, sku);
                    cont++;
                    log.EscribeLog("pedido: " + cont++);
                }
            }
            catch(Exception ex)
            {
                log.EscribeLog("error en el proceso de tracking kits CS");
            }
        }
    }
}
