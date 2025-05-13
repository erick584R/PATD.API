using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class RequestCrearTarjetasDTO
    {
        public string? NumTarjeta { get; set; }
        public string? FechaVencimiento { get; set; }
    }
}
