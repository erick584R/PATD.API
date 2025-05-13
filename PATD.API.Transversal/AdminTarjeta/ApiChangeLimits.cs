using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class ApiChangeLimits
    {
        public string Sender { get; set; }
        public string Pan { get; set; }
        public bool LimiteRetiroActivo { get; set; }
        public int LimiteRetiroNumeroDiario { get; set; }
        public double LimiteRetiroMontoDiario { get; set; }
        public double LimiteRetiroMontoTxn { get; set; } = 0;
        public bool LimiteCompraActivo { get; set; }
        public int LimiteCompraNumeroDiario { get; set; }
        public double LimiteCompraMontoDiario { get; set; }
        public double LimiteCompraMontoTxn { get; set; }

    }
}
