using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class ResponseLoginDTO
    {
        public string? UserID { get; set; }
        public string? NombreUsuario { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Token { get; set; }
        public string? CodRespuesta { get; set; }
        public string? DescRespuesta { get; set; }
    }
}
