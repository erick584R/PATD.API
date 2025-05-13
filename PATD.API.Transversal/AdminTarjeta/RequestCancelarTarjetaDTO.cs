using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class RequestCancelarTarjetaDTO
    {
        public string? NumTarjeta { get; set; }
        public string? CodMotivo { get; set; }
    }
}
