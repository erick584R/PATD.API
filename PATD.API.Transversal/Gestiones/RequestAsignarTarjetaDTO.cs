namespace PATD.API.Transversal.Gestiones
{
    public class RequestAsignarTarjetaDTO
    {
        public int? IdTarjeta { get; set; }
        public string? NumCuenta { get; set; }
        public int? IdTipoTarjeta { get; set; }
        public string? Cif { get; set; }
    }
}