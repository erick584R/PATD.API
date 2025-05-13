using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Login
{
    public class LoginResponseDTO
    {
        public bool ValidacionActiveDirectory { get; set; } //Campo utilizado para validación de login
        public string? JwtSesion { get; set; }
        public string? IdUsuario { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public int IdRol { get; set; }
        public int IdAgenciaAsignada { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? Estado { get; set; }
        public string? Correo { get; set; }
    }
}
