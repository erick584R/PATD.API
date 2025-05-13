using System.Net;
using BancoPopular.Respuestas.Respuesta;
using BancoPopular.Servicios.Servicio;
using BancoPopular.Solicitudes.Solicitud;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PATD.API.DataAccess.Gestiones;
using PATD.API.Transversal.Helper;
using PATD.API.Transversal.Notificaciones;

namespace PATD.API.Application.Notificaciones
{
    public class NotificacionesAPP : INotificacionesAPP
    {
        private readonly IGestionesDA _gestionesDA;
        private readonly IHelper _helper;
        private readonly IServicio _servicio;
        private readonly ISolicitud solicitud;

        public NotificacionesAPP(IGestionesDA gestionesDA, IHelper helper, IServicio servicio, ISolicitud solicitud)
        {
            _gestionesDA = gestionesDA;
            _helper = helper;
            _servicio = servicio;
            this.solicitud = solicitud;
        }
        public async Task<RespuestaMicroservicio<bool>> EnviarNotificacion(NotificacionDTO notificacion)
        {
            var respuesta = new RespuestaMicroservicio<bool>();

            notificacion.CIF = notificacion.CIF.PadLeft(18, ' ');
            try
            {
                var Cliente = await _gestionesDA.ConsultarDatosClienteAS400(notificacion.CIF);
                if (Cliente == null)
                {
                    respuesta.Entidad = false;
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "No se encontraron datos";
                    return respuesta;
                }
                if (notificacion.SendEmail)
                {
                    bool error = false;
                    if (string.IsNullOrWhiteSpace(Cliente.vCORREO))
                    {
                        error = true;
                    }
                    if (!error)
                    {
                        var correos = new List<string>();
                        correos?.Add(Cliente.vCORREO.Trim());
                        await _helper.EnviaEmail(correos, notificacion.Mensaje, "Notificación");
                    }
                }
                if (notificacion.SendSMS)
                {
                    bool errortelefono = false;
                    if (string.IsNullOrWhiteSpace(Cliente.vCELULAR))
                    {
                        errortelefono = true;
                    }
                    if (!errortelefono)
                    {
                        var mensaje = new DatosPELSMSDTO
                        {
                            Telefono = Cliente.vCELULAR.Trim(),
                            Mensaje = notificacion.Mensaje,
                            Tag="Gestiones TD"
                        };
                        var convertir = _servicio.JsonToStringContent(mensaje);
                        var urlBase = Environment.GetEnvironmentVariable("UrlBaseNotificaciones");
                        var validarToken = await solicitud.Post(convertir, $"{urlBase}/sms/enviar-sms");

                        JObject sms = JsonConvert.DeserializeObject<JObject>(Convert.ToString(validarToken.Json));
                        var cod = sms.SelectToken("codigo").ToString();

                        if (cod != "200")
                        {
                            respuesta.Entidad = false;
                            respuesta.Codigo = HttpStatusCode.BadRequest;
                            respuesta.Mensaje = urlBase + mensaje.Mensaje + mensaje.Telefono;
                            return respuesta;
                        }
                    }
                }
                respuesta.Entidad = true;
                respuesta.Codigo = 0;
                respuesta.Mensaje = "Notificacion enviada exitosamente.";
                return respuesta;
            }
            catch (Exception ex)
            {
                return new RespuestaMicroservicio<bool>()
                {
                    Codigo = 500,
                    Excepcion = ex.Message,
                    Mensaje = "Ocurrio un error interno",
                };
            }
        }
    }
}
