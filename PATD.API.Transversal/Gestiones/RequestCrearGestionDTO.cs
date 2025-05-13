namespace PATD.API.Transversal.Gestiones
{
    public class RequestCrearGestionDTO
    {
        public int? IdTipoGestion { get; set; }
        public int? IdLote { get; set; }
        public int? IdTarjeta { get; set; }
        public string? NumCuenta { get; set; }
        public string? motivo { get; set; }
        public string? Alias { get; set; }
        public string ? Cif { get; set; }
        public int IdAgencia { get; set; }
        public string? Usuario { get; set; }
        
    }
}