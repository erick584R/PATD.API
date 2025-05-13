using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Notificaciones
{
    public class NotificacionDTO
    {
        public string CIF { get; set; }
        public string Mensaje { get; set; }
        public bool SendEmail { get; set; }
        public bool SendSMS { get; set; }
        public string? Usuario { get; set; }
    }
}
