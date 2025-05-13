using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class ResponseAsignarPinDTO
    {
        public int IdSolicitud { get; set; }
        public string? Tarjeta { get; set; }
        public string? MedioAcceso { get; set; }
        public int CodRespuesta { get; set; }
        public string? DescRespuesta { get; set; }
    }
}
