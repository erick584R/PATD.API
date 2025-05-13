using BancoPopular.Respuestas.Respuesta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PATD.API.Application.Notificaciones;
using PATD.API.Transversal.Gestiones;
using PATD.API.Transversal.Notificaciones;

namespace PATD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionesAPP _notificaciones;

        public NotificacionesController(INotificacionesAPP notificaciones)
        {
            _notificaciones = notificaciones;
        }

        [HttpPost("/v1/BancoPopular/EnvioNotificacion")]
        public async Task<RespuestaMicroservicio<bool>> RequestEnvioNotificacion(NotificacionDTO notificacion)
            => await _notificaciones.EnviarNotificacion(notificacion);
    }
}
