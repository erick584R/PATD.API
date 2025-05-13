namespace PATD.API.Transversal.Gestiones
{
    public class RequestCrearTarjetaDTO
    {
        public int? IdLote { get; set; }
        public string? Emisor { get; set; }
        public string? NumTarjeta { get; set; }
        public int? IdTipoTarjeta { get; set; }
        public DateTime Vencimiento { get; set; }
        public string? Usuario { get; set; }
    }
}