using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.AdminTarjeta
{
    public class loginResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public long expiresIn { get; set; }
    }
}
