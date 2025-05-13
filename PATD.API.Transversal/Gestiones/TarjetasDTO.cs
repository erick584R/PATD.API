using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class TarjetasDTO
    {
        public int Id { get; set; }
        public string? Emisor { get; set; }
        public string? CuentaAsignada { get; set; }
        public string? NumTarjeta { get; set; }
        public int? IdCTipoTarjeta { get; set; }
        public DateTime? Vencimiento { get; set; }
        public string? Cif { get; set; }
        public int? IdCEstadoTarjeta { get; set; }
        public int? IdLoteTarjeta { get; set; }
        public string? IdMotivoCancelacion { get; set; }
        public string? IdRazonReposicion { get; set; }
        public string? IdMotivoBloqueo { get; set; }
        public int? IdAgencia { get; set; }
        public int? IdEstadoLogisticoTarjeta { get; set; }
        public int? IdAgenciaEnviar { get; set; }
        public string? NombreMostrarTarjeta { get; set; }
        public DateTime FechaColocacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
