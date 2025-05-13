using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class RequestCancelarTarjetaDTO
    {
        public string? IdSolicitud { get; set; }
        public string? Tarjeta { get; set; }
        public string? MotivoCancelacion { get; set; }
        public string? TipoMedioAcceso { get; set; }
        public string? MedioAcceso { get; set; }
    }
}
