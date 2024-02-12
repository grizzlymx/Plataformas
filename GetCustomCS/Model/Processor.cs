using GetCustomCS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCustomCS.Model
{
    public class Processor
    {
        public void GetCustom(DataTable date)
        {
            var log = new clsLog();
            var consulta = new ConsultaAndUpd();
            try
            {
                var count = date.Rows.Count;
                log.EscribeLog("pedidos por procesar: " + count);
                foreach (DataRow row in date.Rows)
                {
                    var cont = 0;
                    var id_pedido = row[1].ToString();
                    var idpedidorelacion = row[5].ToString();
                    var estatus = row[6].ToString();

                    log.EscribeLog("id pedido: " + id_pedido);
                    log.EscribeLog("id pedido relacion: " + idpedidorelacion);
                    log.EscribeLog("estatus: " + estatus);

                    var contador = consulta.getOrderCustomCS(id_pedido, idpedidorelacion);
                    if(int.Parse(contador) >=1)
                    {
                        consulta.updOrderCustomCS(id_pedido, idpedidorelacion);
                    }
                    cont++;
                    log.EscribeLog("pedido: " + cont++);
                }
            }
            catch(Exception ex)
            {
                log.EscribeLog("error en el proceso de Custom CS");
            }
        }
    }
}
