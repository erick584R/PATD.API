using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class ResponseConsultarTarjetaDTO
    {
        public int? IdRequest { get; set; }
        public string? Card { get; set; }
        public string? ManufacturingType { get; set; }
        public int? Account { get; set; }
        public int? InernalAccount { get; set; }
        public string? ExpirationDate { get; set; }
        public int? Status { get; set; }
        public string? DescripcionStatus { get; set; }
        public string? Name { get; set; }
        public string? FirstLastName { get; set; }
        public CurrentBalance? CurrentBalance { get; set; }
    }
    public class CurrentBalance
    {
        public string? AccountId { get; set; }
        public string? AccountTypeKey { get; set; }
        public string? AccountTypeDescription { get; set; }
        public int? Balance { get; set; }
    }
}
