using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class ActualizarDatosClientePELDTO
    {
        public string? Cif { get; set; }
        public string? EstadoCivil { get; set; }
        public string? DireccionPersonal { get; set; }
        public string? BloqueSectorPersonal { get; set; }
        public string? PuntoReferenciaPersonal { get; set; }
        public string? DireccionTrabajo { get; set; }
        public string? BloqueSectorTrabajo { get; set; }
        public string? PuntoReferenciaTrabajo { get; set; }
        public string? Celular { get; set; }
        public string? TelFijoPer { get; set; }
        public string? TelFijoLab { get; set; }
        public string? Correo { get; set; }
        public string? Usuario { get; set; }
    }
}
