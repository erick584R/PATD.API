using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class CuentasDTO
    {
        public decimal? tipoproducto { get; set; }
        public string? descripcionProducto { get; set; }
        public decimal? subtipoProducto { get; set; }
        public string? descripcionSubProducto { get; set; }
        public string? cuenta { get; set; }
        public string? fecha { get; set; }
        public string? saldo { get; set; }
        public string? moneda { get; set; }
        public decimal? tasa { get; set; }
        public IEnumerable<TarjetasDTO>? Tarjetas { get; set; }
    }
}
