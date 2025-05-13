using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Notificaciones
{
    public class ObtenerUsuarioPELDTO
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public int IdEstado { get; set; }
        public string Email { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaCambioClave { get; set; }
        public bool CambioClave { get; set; }
        public bool EsUsuarioEmpresa { get; set; }
        public string Telefono { get; set; }
    }
}
