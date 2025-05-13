namespace PATD.API.Transversal.LogsT
{
    public class ObtenerLogsDTO
    {
        public IEnumerable<ObtenerLogsGestionesDTO>? LogsGestiones { get; set; }
        public IEnumerable<ObtenerLogsTarjetaDTO>? LogsTarjeta { get; set; }
        public IEnumerable<ObtenerLogsVolcanDTO>? LogsVolcan { get; set; }
    }
    public class ObtenerLogsGestionesDTO
    {
        public int Id { get; set; }
        public string? IdGestion { get; set; }
        public string? TipoEvento { get; set; }
        public string? Descripcion { get; set; }
        public string? Usuario { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Error { get; set; }
    }
    public class ObtenerLogsTarjetaDTO
    {
        public int Id { get; set; }
        public int IdTarjeta { get; set; }
        public string? TipoEvento { get; set; }
        public string? Descripcion { get; set; }
        public string? Usuario { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Error { get; set; }
    }
    public class ObtenerLogsVolcanDTO
    {
        public int Id { get; set; }
        public string? IdGestion { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Usuario { get; set; }
    }
}
