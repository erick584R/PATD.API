using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class ApiAssignCard
    {
        public string Sender { get; set; }
        public string Cif { get; set; }
        public string Pan { get; set; }
        public short Agencia { get; set; }

    }
}
