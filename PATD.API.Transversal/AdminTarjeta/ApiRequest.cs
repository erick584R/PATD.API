namespace PATD.API.Transversal.AdminTarjeta
{
    public class ApiRequest
    {
        public string? EncryptedKey { get; set; }
        public string? EncryptedIv { get; set; }
        public string? EncryptedMessage { get; set; }
    }
}
