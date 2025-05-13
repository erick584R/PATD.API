using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class RequestConsultarTarjetaDTO
    {
        public int? IdRequest { get; set; }
        public string? Card { get; set; }
        public string? AccessEntry { get; set; }
        public string? TypeAccessEntry { get; set; }
    }
}
