using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class LimitesGeneralesDTO
    {
        public string? Categoria { get; set; }
        public string? Descripcion { get; set; }
        public int LimiteRetiroNumeroDiario { get; set; }
        public double LimiteRetiroMontoDiario { get; set; }
        public double LimiteRetiroMontoTxn { get; set; }
        public int LimiteCompraNumeroDiario { get; set; }
        public double LimiteCompraMontoDiario { get; set; }
        public double LimiteCompraMontoTxn { get; set; }
    }
}
