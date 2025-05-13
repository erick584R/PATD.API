using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class ResponseReasignarTarjetaDTO
    {
        public string? IdRequest { get; set; }
        public string? OldCard { get; set; }
        public string? NewCard { get; set; }
        public string? PreviusAccessEntry { get; set; }
        public string? NewAccessEntry { get; set; }
    }
}
