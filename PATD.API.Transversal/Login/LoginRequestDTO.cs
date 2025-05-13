using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Login
{
    public class LoginRequestDTO
    {
        public string? IdUsuario { get; set; }
        public string? Contraseña { get; set; }
    }
}
