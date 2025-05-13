using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class ResponseTrxTdDto
    {
        public string? Cuenta { get; set; }
        public string? ComercioAtm { get; set; }
        public decimal Monto { get; set; }
        public decimal Comision { get; set; }
        public string? MonedaMonto { get; set; }
        public string? FechaTrx { get; set; }
        public string? HoraTx { get; set; }
        public string? Internacional { get; set; }
        public string? CodigoEstado { get; set; }
        public string? DetalleEstado { get; set; }
    }
}
