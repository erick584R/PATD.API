using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class RequestAsignarPinDTO
    {
        public int IdSolicitud { get; set; }
        public string? Tarjeta { get; set; }
        public string? MedioAcceso { get; set; }
        public string? TipoMedioAccesoOrigen { get; set; }
        public string? NipNuevo { get; set; }
    }
}
