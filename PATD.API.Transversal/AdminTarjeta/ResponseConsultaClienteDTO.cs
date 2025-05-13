using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class ResponseConsultaClienteDTO
    {
        public string? Pan { get; set; }
        public string? Cuenta { get; set; }
        public string? MaskedPan { get; set; }
        public string? Cif { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string? StatusBanet { get; set; }
        public string? StatusVolcan { get; set; }
        public string? Aprobada { get; set; }
        public string? UsuarioAprobador { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public bool LimiteRetiroActivo { get; set; }
        public int LimiteRetiroNumeroDiario { get; set; }
        public decimal LimiteRetiroMontoDiario { get; set; }
        public decimal LimiteRetiroMontoTxn { get; set; }
        public int RetiroNumeroDia { get; set; }
        public decimal RetiroMontoDiario { get; set; }
        public DateTime FechaUltimoRetiro { get; set; }
        public bool LimiteCompraActivo { get; set; }
        public int LimiteCompraNumeroDiario { get; set; }
        public decimal LimiteCompraMontoDiario { get; set; }
        public decimal LimiteCompraMontoTxn { get; set; }
        public int CompraNumeroDia { get; set; }
        public decimal CompraMontoDiario { get; set; }
        public DateTime FechaUltimaCompra { get; set; }
        public IEnumerable<CardAccountsDTO>? CardAccounts { get; set; }
        public IEnumerable<AutorizacionesPaisDTO>? AutorizacionesPais { get; set; }
        public int AgenciaId { get; set; }
    }
    public class CardAccountsDTO
    {
        public string? Pan { get; set; }
        public string? AccountNo { get; set; }
        public int Priority { get; set; }
        public string? Type { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool CreatedBanet { get; set; }
        public bool UpdatedBanet { get; set; }
    }
    public class AutorizacionesPaisDTO
    {
        public string? Pan { get; set; }
        public int CodigoIsoNumerico { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
