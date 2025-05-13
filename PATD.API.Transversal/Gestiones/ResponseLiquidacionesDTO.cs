using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Gestiones
{
    public class ResponseLiquidacionesDTO
    {
        public string? NumReferencia { get; set; }
        public string? NumCuenta { get; set; }
        public string? NumTarjeta { get; set; }
        public string? Autorizacion { get; set; }
        public string? NegAtmRem { get; set; }
        public string? Estado { get; set; }
        public string? FechaConsumo { get; set; }
        public string? FechaProcesado { get; set; }
        public int MonedaOrigen { get; set; }
        public decimal MontoOrigen { get; set; }
        public decimal TasaCambio { get; set; }
        public decimal MontoLPS { get; set; }
        public decimal Comision { get; set; }
        public decimal ComisionBancoPopular { get; set; }
        public decimal MontoLiquidado { get; set; }
        public string? FechaLiquidacion { get; set; }
        public string? HoraLiquidacion { get; set; }
        public string? NombreArchivoLiquidacion { get; set; }
        public string? FechaArchivoLiquidacion { get; set; }
        public string? CodigoRespuesta { get; set; }
        public string? Respuesta { get; set; }
    }
}
