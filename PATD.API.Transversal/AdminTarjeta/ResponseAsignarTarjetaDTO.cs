using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class ResponseAsignarTarjetaDTO
    {
        public string? Pan { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
