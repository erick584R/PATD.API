using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class RequestActivarTarjetaDTO
    {
        public int? IDSolicitud { get; set; }
        public string? Tarjeta { get; set; }
        public string? MedioAcceso { get; set; }
        public string? TipoMedioAcceso { get; set; }
    }
}
