using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Volcan
{
    public class RequestActualizarDatosClienteDTO
    {
        public string? IdRequest { get; set; }
        public string? UpdateType { get; set; }
        public string? CompanyID { get; set; }
        public string? Card { get; set; }
        public string? AccessEntry { get; set; }
        public string? AccessEntryType { get; set; }
        public string? Name { get; set; }
        public string? FirstLastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? CardholderName { get; set; }
        public string? Nationality { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? Birthdate { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CellPhone { get; set; }
        public string? Occupation { get; set; }
        public string? Profession { get; set; }
        public string? LineOfBusiness { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? StateKey { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? ExteriorNumber { get; set; }
        public string? InteriorNumber { get; set; }
        public string? Neighborhood { get; set; }
        public string? Delegation { get; set; }
        public string? TownID { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string? IDType { get; set; }
        public string? IDNumber { get; set; }
        public string? addres_line1 { get; set; }
        public string? addres_line2 { get; set; }
        public string? CustomerType { get; set; }
        public int? Acceptance { get; set; }
    }
}
