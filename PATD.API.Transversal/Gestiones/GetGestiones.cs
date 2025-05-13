using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class GetGestiones
    {
        public string IdGestion { get; set; }
        public int IdTarjeta { get; set; }
        public int IdTipoGestion { get; set; }
        public string IdUsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int IdEstadoGestion { get; set; }
        public int IdAgenciaAsignada { get; set; }
    }
}
