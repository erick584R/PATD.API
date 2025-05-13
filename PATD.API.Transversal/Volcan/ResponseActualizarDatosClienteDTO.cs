using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class ResponseActualizarDatosClienteDTO
    {
        public string? IdRequest { get; set; }
        public int? ResponseCode { get; set; }
        public string? ResponseDescription { get; set; }
    }
}
