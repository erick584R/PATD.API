using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class ResponseCrearTarjetasDTO
    {
        public List<string>? Created { get; set; }
        public List<string>? NotCreated { get; set; }
    }
}
