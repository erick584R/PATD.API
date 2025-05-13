using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class RequestAsignarTarjetaAgenciaDTO
    {
        public string? Usuario { get; set; }
        public int? IdAgencia { get; set; }
        public int? IdAgenciaEnviar { get; set; }
        public int? CantidadTarjetas { get; set; }
    }
}
