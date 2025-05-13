using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class ResponseMovimientosTarjetaDTO
    {
        public string? NumeroReferencia { get; set; }
        public decimal Monto { get; set; }
        public decimal ComisionATM { get; set; }
        public string? FechaProceso { get; set; }
        public string? HoraProceso { get; set; }
        public string? TransInternacional { get; set; }
        public string? Estado { get; set; }
        public string? CodRespuesta { get; set; }
        public string? IdAutorizacion { get; set; }
        public string? TipoTransaccion { get; set; }
        public string? RefBloqueoPOS { get; set; }
        public string? Reversada { get; set; }
        public string? FechaReversada { get; set; }
        public string? HoraReversada { get; set; }
    }
}