using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class InformacionClienteDTO
    {
        public string? Cif { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Nacionalidad { get; set; }
        public string? PaisNacimiento { get; set; }
        public int? DiaNacimiento { get; set; }
        public int? MesNacimiento { get; set; }
        public int? AnoNacimiento { get; set; }
        public string? Genero { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? DireccionTrabajo { get; set; }
        public string? Celular { get; set; }
        public string? TelefonoResidencia { get; set; }
        public string? telefonoTrabajo { get; set; }
        public string? Profesion { get; set; }
        public IEnumerable<CuentasDTO>? CuentasActivas { get; set; }
    }
}
