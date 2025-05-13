using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class RequestEncrytedApiDTO
    {
        public byte[]? Key { get; set; }
        public byte[]? Iv { get; set; }
        public string? EncryptedKey { get; set; }
        public string? EncryptedIv { get; set; }
        public string? EncryptedMessage { get; set; }
    }
}
