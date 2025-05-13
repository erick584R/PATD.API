using BancoPopular.Respuestas.Respuesta;
using PATD.API.Transversal.Notificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Application.Notificaciones
{
    public interface INotificacionesAPP
    {
        Task<RespuestaMicroservicio<bool>> EnviarNotificacion(NotificacionDTO notificacion);

    }
}
