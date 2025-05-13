using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class RequestReasignarTarjetaDTO
    {
        public string? IdRequest { get; set; }
        public string? OldCard { get; set; }
        public string? NewCard { get; set; }
        public string? PreviusAccessEntryType { get; set; }
        public string? PreviusAccessEntry { get; set; }
        public string? NewAccessEntryType { get; set; }
        public string? NewAccessEntry { get; set; }
    }
}
