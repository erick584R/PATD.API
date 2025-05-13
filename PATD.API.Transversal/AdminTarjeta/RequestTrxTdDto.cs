using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class RequestTrxTdDto
    {
        public string? IdentidadCliente { get; set; }
        public string? UltimosDigitosTarjeta { get; set; }
    }
}
