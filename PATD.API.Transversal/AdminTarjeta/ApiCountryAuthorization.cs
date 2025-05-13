using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class ApiCountryAuthorization
    {
        public string Sender { get; set; }
        public string Pan { get; set; }
        public short CodigoIsoNumerico { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }

    }
}
