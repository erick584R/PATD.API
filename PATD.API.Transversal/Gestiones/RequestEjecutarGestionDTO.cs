using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class RequestEjecutarGestionDTO
    {
        //Para asignar tarjeta
        public int? IdTipoGestion { get; set; }
        public string? IdentidadCliente { get; set; }
        public TarjetasDTO? TarjetaCliente { get; set; }        
        public GestionDTO? Gestion { get; set; }        
    }
}
