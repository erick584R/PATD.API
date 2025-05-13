using System.Net;
using System.Resources;
using BancoPopular.Respuestas.Respuesta;
using BancoPopular.Servicios.Servicio;
using PATD.API.Application.AdminTarjeta;
using PATD.API.Application.Notificaciones;
using PATD.API.DataAccess.Gestiones;
using PATD.API.DataAccess.LogDA;
using PATD.API.DataAccess.Seguridad;
using PATD.API.Transversal.AdminTarjeta;
using PATD.API.Transversal.Gestiones;
using PATD.API.Transversal.Helper;
using PATD.API.Transversal.LogsT;
using PATD.API.Transversal.Notificaciones;

namespace PATD.API.Application.Gestiones
{
    public class GestionesAPP : IGestionesAPP
    {
        private readonly IGestionesDA _gestionesDA;
        private readonly IHelper _helper;
        private readonly ISeguridadDA _seguridadDA;
        private readonly ILogDA _logDA;
        private readonly IAdminTarjeta _adminTarjeta;
        private readonly IServicio _servicio;
        private readonly INotificacionesAPP _notificacionesAPP;
        bool _Error;
        public GestionesAPP(IGestionesDA gestionesDA, ISeguridadDA seguridadDA, ILogDA logDA, IHelper helper, IAdminTarjeta adminTarjeta, IServicio servicio, INotificacionesAPP notificacionesAPP)
        {
            _gestionesDA = gestionesDA;
            _helper = helper;
            _seguridadDA = seguridadDA;
            _logDA = logDA;
            _adminTarjeta = adminTarjeta;
            _servicio = servicio;
            _notificacionesAPP = notificacionesAPP;
        }

        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> GetTarjetas(string user)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<TarjetasDTO>>();
            try
            {
                var request = await _gestionesDA.GetTarjetas();
                if (request is null)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontró información";
                }

                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(0, "Obtener", "Obtener listado de tarjetas", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<GetTipoGestionesDTO>>> GetTipoGestiones(string user)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<GetTipoGestionesDTO>>();
            try
            {
                var request = await _gestionesDA.GetTipoGestiones();
                if (request.Count() is 0)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontró información";
                    return respuesta;
                }

                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion("N/A", "Obtener", "Obtener listado de tipo de gestiones", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<ResponseCrearLoteDTO>> RequestCrearLote(string user)
        {
            string lote = string.Empty;
            var respuesta = new RespuestaMicroservicio<ResponseCrearLoteDTO>();
            try
            {
                var request = await _gestionesDA.RequestCrearLote();
                if (!request.All(char.IsDigit))
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Error al intentar ingresar lote";
                    return respuesta;
                }
                lote = request;
                respuesta.Entidad = new ResponseCrearLoteDTO { IdLote = Convert.ToInt32(request) };
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Lote creado con exito";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion("N/A", "Crear", $"Creación del lote número {lote}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<bool>> RequestCrearTarjeta(RequestCrearTarjetaDTO requestCrearTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {                
                var lista = new List<RequestCrearTarjetasDTO>();
                lista.Add(new RequestCrearTarjetasDTO { NumTarjeta = requestCrearTarjetaDTO.NumTarjeta, FechaVencimiento = requestCrearTarjetaDTO.Vencimiento.ToString("yyyy-MM-dd") });
                var crearTarjetas = await _adminTarjeta.CrearTarjetas(lista);
                if (crearTarjetas.Entidad.NotCreated.Count > 0)
                {
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Tarjeta número {requestCrearTarjetaDTO.NumTarjeta} no ingresada";
                }
                else if (crearTarjetas.Entidad.Created.Count > 0)
                {
                    await _gestionesDA.RequestCrearTarjeta(requestCrearTarjetaDTO);
                    respuesta.Entidad = true;
                    respuesta.Codigo = HttpStatusCode.Accepted;
                    respuesta.Mensaje = $"Tarjeta número {requestCrearTarjetaDTO.NumTarjeta} ingresada con exito";
                }
                else
                {
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Tarjeta número {requestCrearTarjetaDTO.NumTarjeta} no ingresada";
                }
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(0, "Crear", $"Creación de la tarjeta número {requestCrearTarjetaDTO.NumTarjeta}", requestCrearTarjetaDTO.Usuario, _Error); }
        }

        public async Task<RespuestaMicroservicio<RequestGestionDTO>> RequestCrearGestion(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var respuesta = new RespuestaMicroservicio<RequestGestionDTO>();
            //var prueba = "";
            string requestGestion = "";
            string NumGestion = string.Empty;
            try
            {
                if (await _gestionesDA.ValidaGestion(requestCrearGestionDTO))
                {
                    respuesta.Entidad = new RequestGestionDTO() { IdGestion = null, Gestion = false };
                    respuesta.Codigo = HttpStatusCode.OK;
                    respuesta.Mensaje = $"Ya existe una gestión pendiente.";
                    return respuesta;
                }
                switch (requestCrearGestionDTO.IdTipoGestion)
                {
                    case 2:
                        var requestInfoCliente = await _gestionesDA.InformacionCliente(requestCrearGestionDTO.Cif);
                        var tarjetaAsignada = await _adminTarjeta.AsignarTarjeta(new Transversal.AdminTarjeta.RequestAsignarTarjetaDTO { Cif = requestInfoCliente.Cif.Trim(), Agencia = requestCrearGestionDTO.IdAgencia.ToString() });
                        if(string.IsNullOrEmpty(tarjetaAsignada.Entidad.Pan))
                        {
                            respuesta.Codigo = HttpStatusCode.Conflict;
                            respuesta.Mensaje = $"Error al intentar asignar tarjeta, contacte a soporte tecnico.";
                            return respuesta;
                        }
                        var obteneridtarjeta = await _gestionesDA.RequestAsignarTarjeta(requestCrearGestionDTO, tarjetaAsignada.Entidad.Pan);
                        requestCrearGestionDTO.IdTarjeta = Convert.ToInt32(obteneridtarjeta);

                        break;
                    case 3:
                        requestGestion = (await _gestionesDA.RequestCancelarTarjeta(requestCrearGestionDTO)) is not "OK" ? "ERROR" : "";
                        break;
                    case 4:
                        requestGestion = (await _gestionesDA.RequestUpdateAliasTarjeta(requestCrearGestionDTO)) is not "OK" ? "ERROR" : "";
                        break;
                    case 6:
                        requestGestion = (await _gestionesDA.RequestBloqueoTarjeta(requestCrearGestionDTO)) is not "OK" ? "ERROR" : "";
                        break;
                    case 7:
                        requestGestion = (await _gestionesDA.RequestDesbloqueoTarjeta(requestCrearGestionDTO)) is not "OK" ? "ERROR" : "";
                        break;
                    case 10:
                        requestGestion = (await _gestionesDA.RequestUpdateCuentaTarjeta(requestCrearGestionDTO)) is not "OK" ? "ERROR" : "";
                        break;
                    case 11:
                        requestGestion = (await _gestionesDA.RequestReposicionTarjeta(requestCrearGestionDTO)) is not "OK" ? "ERROR" : "";
                        break;
                }
                if (requestGestion == "")
                {
                    requestGestion = await _gestionesDA.RequestCrearGestion(requestCrearGestionDTO);
                }
                //if (requestGestion is not "OK")
                if (!requestGestion.Contains("OK"))
                {
                    if (requestCrearGestionDTO.IdTipoGestion == 2 && requestCrearGestionDTO.IdTarjeta != null)
                    {
                        await _gestionesDA.RollBackTarjeta((int)requestCrearGestionDTO.IdTarjeta);
                    }
                    _Error = true;
                    respuesta.Entidad = new RequestGestionDTO() { IdGestion = null, Gestion = false };
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Error al intentar crear gestión, contacte a soporte tecnico."+requestGestion;
                    return respuesta;
                }
                NumGestion = requestGestion.Substring(3);
                respuesta.Entidad = new RequestGestionDTO() { IdGestion = requestGestion.Substring(3), Gestion = false };
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Gestión creada con exitó";

                var obtenerEncargadoEmail = await _seguridadDA.ObtenerEncargadoAgencia(requestCrearGestionDTO.IdAgencia);
                //prueba = obtenerEncargadoEmail.Correo+obtenerEncargadoEmail.PrimerNombre;
                if (obtenerEncargadoEmail is not null)
                {
                    await EnviarCorreo(obtenerEncargadoEmail.Correo.Trim(), $"Hola {obtenerEncargadoEmail.PrimerNombre.Trim()} {obtenerEncargadoEmail.PrimerApellido.Trim()}, {requestCrearGestionDTO.Usuario} ha creado la gestíon número {NumGestion}, ingrese a la <a href=\"http://plataformaadmintd.bancopopular.hn:9080/\">plataforma administrativa de tarjetas de débitos</a> para más detalles.");
                    //prueba = "correo1";
                }
                //prueba = "correo2 fallo antes";
                var obtenerUsuarioEmail = await _seguridadDA.ObtenerUsuarioPorId(requestCrearGestionDTO.Usuario);
                if (obtenerUsuarioEmail is not null)
                {
                    await EnviarCorreo(obtenerUsuarioEmail.Correo.Trim(), $"Hola {obtenerUsuarioEmail.PrimerNombre.Trim()} {obtenerUsuarioEmail.PrimerApellido.Trim()}, se notificó a {obtenerEncargadoEmail.IdUsuario} de la gestíon número {NumGestion}, ingrese a la <a href=\"http://plataformaadmintd.bancopopular.hn:9080/\">plataforma administrativa de tarjetas de débitos</a> para más detalles.");
                    //prueba = "correo2";
                }

                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno ";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion(NumGestion, "Crear", $"Creación de la gestión número {NumGestion} referente al id de tarjeta {requestCrearGestionDTO.IdTarjeta}", requestCrearGestionDTO.Usuario, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> RequestAsignarTarjetaAgencia(RequestAsignarTarjetaAgenciaDTO requestAsignarTarjetaAgenciaDTO)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<TarjetasDTO>>();
            try
            {
                var requestGestionLote = await _gestionesDA.RequestAsignarTarjetaAgencia(requestAsignarTarjetaAgenciaDTO);
                if (requestGestionLote is null)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Error al actualizar datos";
                    return respuesta;
                }
                var listaTarjetasEnviadas = new List<TarjetasDTO>();
                foreach (var item in requestGestionLote)
                {
                    var distribuir = await _adminTarjeta.DistribuirTarjetas(new RequestDistribuirTarjetasDTO { Agencia = requestAsignarTarjetaAgenciaDTO.IdAgenciaEnviar.ToString(), NumTarjeta = item.NumTarjeta });
                    if (!distribuir.Entidad)
                    {
                        await _gestionesDA.RollBackTarjetaAsignadaAgencia(item.Id);
                        item.IdCEstadoTarjeta = 8;
                        _Error = true;
                    }
                    listaTarjetasEnviadas.Add(item);
                }
                respuesta.Entidad = listaTarjetasEnviadas;
                respuesta.Codigo = _Error ? HttpStatusCode.Conflict : HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Datos actualizados con exitó";
                var obtenerEncargadoEmail = await _seguridadDA.ObtenerEncargadoAgencia(requestAsignarTarjetaAgenciaDTO.IdAgenciaEnviar);
                if (obtenerEncargadoEmail is not null)
                {
                    await EnviarCorreo(obtenerEncargadoEmail.Correo.Trim(), $"Hola {obtenerEncargadoEmail.PrimerNombre.Trim()} {obtenerEncargadoEmail.PrimerApellido.Trim()}, se han asignado nuevas tarjetas de débito a su agencia, ingrese a la <a href=\"http://plataformaadmintd.bancopopular.hn:9080/\">plataforma administrativa de tarjetas de débitos</a> para más detalles.");
                }
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno - " + e.Message;
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(0, "Asignar", $"Asignación de {requestAsignarTarjetaAgenciaDTO.CantidadTarjetas} al id de agencia {requestAsignarTarjetaAgenciaDTO.IdAgenciaEnviar}", requestAsignarTarjetaAgenciaDTO.Usuario, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDisponiblesAgenciasDTO>>> TarjetasDisponiblesAgencias(string user)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<TarjetasDisponiblesAgenciasDTO>>();
            try
            {
                var requestGestionLote = await _gestionesDA.TarjetasDisponiblesAgencias();
                if (requestGestionLote is null)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Error al traer datos";
                    return respuesta;
                }
                respuesta.Entidad = requestGestionLote;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(0, "Obtener", $"Obtener tarjetas disponibles en Stock", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<GetGestiones>>> GetGestiones(string user)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<GetGestiones>>();
            try
            {
                var request = await _gestionesDA.GetGestiones(user);
                if (request.Count() is 0)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontró información";
                    return respuesta;
                }

                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion("N/A", "Obtener", $"Obtener listado de gestiones", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<InformacionClienteDTO>> ConsultaInfoCliente(string identidad, string user)
        {
            var respuesta = new RespuestaMicroservicio<InformacionClienteDTO>();
            try
            {
                var requestInfoCliente = await _gestionesDA.InformacionCliente(identidad);
                if (requestInfoCliente is null)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Error al intentar consulta información del cliente";
                    return respuesta;
                }

                var requestCuentas = await _gestionesDA.ObtenerCuentas(requestInfoCliente.Cif);
                var requestTarjetas = await _gestionesDA.GetTarjetasCif(identidad);
                var cuentasActivas = new List<CuentasDTO>();
                if (requestCuentas.Count() > 0)
                {                    
                    foreach (var item in requestCuentas)
                    {
                        if (item.tipoproducto is 2)
                        {
                            if ((item.subtipoProducto >= 10 && item.subtipoProducto <= 14) || (item.subtipoProducto >= 22 && item.subtipoProducto <= 26) || item.subtipoProducto == 61)
                            {
                                if(requestTarjetas.Count() > 0)
                                {
                                    item.Tarjetas = requestTarjetas.Where(x => x.CuentaAsignada.Trim() == item.cuenta.Trim());
                                }
                                cuentasActivas.Add(item);
                            }
                        }
                    }
                }
                requestInfoCliente.CuentasActivas = cuentasActivas.Count() > 0 ? cuentasActivas.Where(x => x.tipoproducto == 2 && x.moneda == "LPS") : null;
                respuesta.Entidad = requestInfoCliente;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion("N/A", "Obtener", $"Obtener información del cliente con identidad no. ...{(string.IsNullOrEmpty(identidad) || identidad.Count() < 3 ? "N/A" : identidad.Substring(identidad.Length - 3))}", user, _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> EditarEstadoTarjeta(int idEstado, int IdTarjeta, string user)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var editar = await _gestionesDA.EditarEstadoTarjeta(idEstado, IdTarjeta);
                if (editar != "OK")
                {
                    respuesta.Entidad = false;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Error";
                    return respuesta;
                }
                respuesta.Entidad = true;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(IdTarjeta, "Editar", $"Editar estado del id tarjeta {IdTarjeta} a {idEstado}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<GetGestiones>>> GetGestionesAgencia(int idAgencia, string user)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<GetGestiones>>();
            try
            {
                var request = await _gestionesDA.GetGestionesAgencia(idAgencia);
                if (request.Count() is 0)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontró información";
                    return respuesta;
                }

                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion("N/A", "Obtener", $"Obtener listado de gestiones del id agencia {idAgencia}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> TarjetasAgencia(int idAgencia, string user)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<TarjetasDTO>>();
            try
            {
                var requestGestionLote = await _gestionesDA.TarjetasAgencia(idAgencia);
                if (requestGestionLote.Count() is 0)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontraron datos";
                    return respuesta;
                }
                respuesta.Entidad = requestGestionLote;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(0, "Obtener", $"Obtener listado de tarjetas del id agencia {idAgencia}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> TarjetasPorAceptar(int idAgencia, string user)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<TarjetasDTO>>();
            try
            {
                var requestGestionLote = await _gestionesDA.TarjetasPorAceptar(idAgencia);
                if (requestGestionLote.Count() is 0)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontraron datos";
                    return respuesta;
                }
                respuesta.Entidad = requestGestionLote;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(0, "Obtener", $"Obtener listado de tarjetas por aceptar del id agencia {idAgencia}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<bool>> AceptarTarjeta(int idTarjeta, string user)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                await _gestionesDA.AceptarTarjeta(idTarjeta);
                respuesta.Entidad = true;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(idTarjeta, "Asignar", $"Asignación de id tarjeta {idTarjeta} a la agencia del usuario {user}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<TarjetasDTO>> GetTarjetaPorId(int idTarjeta, string user)
        {
            var respuesta = new RespuestaMicroservicio<TarjetasDTO>();
            try
            {
                var obtenerTarjeta = await _gestionesDA.GetTarjetaPorId(idTarjeta);
                if (obtenerTarjeta is null)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontraron datos";
                    return respuesta;
                }
                respuesta.Entidad = obtenerTarjeta;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(idTarjeta, "Obtener", $"Obtener id tarjeta {idTarjeta}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<bool>> EditarEstadoGestion(int idEstado, string idGestion, string user)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var editar = await _gestionesDA.EditarEstadoGestion(idEstado, idGestion, user);
                if (editar != "OK")
                {
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Error";
                    return respuesta;
                }
                if (idEstado is 3)
                {
                    await _gestionesDA.DenegarGestion(idGestion);
                }
                if (idEstado is 2 || idEstado is 3)
                {
                    var obtenerGestion = await _gestionesDA.GetGestionesId(idGestion);
                    var obtenerEmail = await _seguridadDA.ObtenerUsuarioPorId(obtenerGestion.IdUsuarioIngreso);
                    if (obtenerEmail is not null)
                    {
                        await EnviarCorreo(obtenerEmail.Correo.Trim(), $"Hola {obtenerEmail.PrimerNombre.Trim()} {obtenerEmail.PrimerApellido.Trim()}, se ha {(idEstado is 2 ? "Autorizado" : "Rechazado")} la gestion no.{idGestion}, ingrese a la <a href=\"http://plataformaadmintd.bancopopular.hn:9080/\">plataforma administrativa de tarjetas de débitos</a> para más detalles.");
                    }
                }
                respuesta.Entidad = true;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion(idGestion, "Editar", $"Editar estado de gestión a {idEstado}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<bool>> ObtenerEstadoGestion(string IdGestion, string user)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var validar = await _gestionesDA.ObtenerEstadoGestion(IdGestion);
                if (validar is 0)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No existe registro";
                    return respuesta;
                }
                if (validar is 1)
                {
                    respuesta.Codigo = HttpStatusCode.Continue;
                    respuesta.Mensaje = $"Aún no hay respuesta del supervisor";
                    return respuesta;
                }
                respuesta.Entidad = validar is 2 ? true : false;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion(IdGestion, "Obtener", $"Obtener estado de gestión", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<bool>> EjecutarGestion(RequestEjecutarGestionDTO requestEjecutarGestionDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                switch (requestEjecutarGestionDTO.IdTipoGestion)
                {
                    case 2:
                        var InformacionCliente = await _gestionesDA.InformacionCliente(requestEjecutarGestionDTO.IdentidadCliente);
                        var agencias = await _seguridadDA.ObtenerAgenciasDA();

                        var relacionar = await _adminTarjeta.RelacionarCuentaTarjeta(new RequestRelacionarCuentaTarjetaDTO { Cuenta = requestEjecutarGestionDTO.TarjetaCliente.CuentaAsignada, NumTarjeta = requestEjecutarGestionDTO.TarjetaCliente.NumTarjeta });

                        var activar = await _adminTarjeta.ActivarTarjeta(new Transversal.AdminTarjeta.RequestActivarTarjetaDTO { NumTarjeta = requestEjecutarGestionDTO.TarjetaCliente.NumTarjeta });
                        if (!activar.Entidad)
                        {                            
                            respuesta.Codigo = HttpStatusCode.Conflict;
                            respuesta.Mensaje = "Error al activar tarjeta" + activar.Mensaje;
                            break;
                        }                        
                        
                        //activar en base de datos
                        await _gestionesDA.EditarEstadoTarjeta(2, requestEjecutarGestionDTO.TarjetaCliente.Id);

                        //asignacion de pin
                        var Pin = _servicio.GenerarToken(4).ToString();
                        var setearPin = await _adminTarjeta.CambiarPin(new RequestCambioPinDTO { NumTarjeta = requestEjecutarGestionDTO.TarjetaCliente.NumTarjeta, Pin = Pin });

                        if (!setearPin.Entidad)
                        {
                            respuesta.Codigo = HttpStatusCode.Continue;
                            respuesta.Mensaje = setearPin.Mensaje + " - Se activo la tarjeta pero no se logro asignar Pin";
                            break;
                        }
                        else
                        {
                            //envio de Pin al cliente
                            await _notificacionesAPP.EnviarNotificacion(new NotificacionDTO { CIF= InformacionCliente.Cif, Mensaje= $"Banco Popular informa, su Tarjeta {requestEjecutarGestionDTO.TarjetaCliente.NumTarjeta.Substring(requestEjecutarGestionDTO.TarjetaCliente.NumTarjeta.Length - 4)} ha sido activada con Pin {Pin}.", SendEmail=true, SendSMS=true });
                        }
                        respuesta.Entidad = true;
                        respuesta.Codigo = HttpStatusCode.Accepted;
                        respuesta.Mensaje = "Tarjeta asignada y activada con exito";
                        break;
                    case 3:
                        var cancelar = await _adminTarjeta.CancelarTarjeta(new Transversal.AdminTarjeta.RequestCancelarTarjetaDTO { NumTarjeta= requestEjecutarGestionDTO.TarjetaCliente.NumTarjeta, CodMotivo = "03" });
                        if (!cancelar.Entidad)
                        {
                            respuesta.Codigo = HttpStatusCode.Conflict;
                            respuesta.Mensaje = "Error al intentar cancelar la tarjeta";
                            break;
                        }
                        await _gestionesDA.EditarEstadoTarjeta(3, requestEjecutarGestionDTO.TarjetaCliente.Id);
                        respuesta.Entidad = true;
                        respuesta.Codigo = HttpStatusCode.Accepted;
                        respuesta.Mensaje = "Tarjeta cancelada con exito";
                        break;
                    case 6:
                        var bloquear = await _adminTarjeta.BloquearTarjeta(new Transversal.AdminTarjeta.RequestBloquearTarjetaDTO { Extravio = true, NumTarjeta = requestEjecutarGestionDTO.TarjetaCliente.NumTarjeta });
                        if (!bloquear.Entidad)
                        {
                            respuesta.Codigo = HttpStatusCode.Conflict;
                            respuesta.Mensaje = "Error al bloquear tarjeta";
                            break;
                        }
                        await _gestionesDA.EditarEstadoTarjeta(4, requestEjecutarGestionDTO.TarjetaCliente.Id);
                        respuesta.Entidad = true;
                        respuesta.Codigo = HttpStatusCode.Accepted;
                        respuesta.Mensaje = "Tarjeta bloqueada con exito";
                        break;
                    case 7:                        
                        var desbloquear = await _adminTarjeta.DesbloquearTarjeta(new Transversal.AdminTarjeta.RequestDesbloquearTarjetaDTO { NumTarjeta = requestEjecutarGestionDTO.TarjetaCliente.NumTarjeta });
                        if (!desbloquear.Entidad)
                        {
                            _Error = true;
                            respuesta.Codigo = HttpStatusCode.Conflict;
                            respuesta.Mensaje = "Error al intentar desbloquear tarjeta";
                            break;
                        }
                        await _gestionesDA.EditarEstadoTarjeta(2, requestEjecutarGestionDTO.TarjetaCliente.Id);
                        respuesta.Entidad = true;
                        respuesta.Codigo = HttpStatusCode.Accepted;
                        respuesta.Mensaje = "Tarjeta desbloqueada con exito";
                        break;
                    case 11:
                        var idNuevaTarjeta = await _gestionesDA.RequestReAsignarTarjeta(new RequestCrearGestionDTO { NumCuenta = requestEjecutarGestionDTO.TarjetaCliente.CuentaAsignada, Cif = requestEjecutarGestionDTO.TarjetaCliente.Cif, IdAgencia = requestEjecutarGestionDTO.Gestion.IdAgenciaAsignada });

                        var nuevoNumeroTarjeta = await _gestionesDA.GetTarjetaPorId(Convert.ToInt32(idNuevaTarjeta));
                        var reasignar = await _adminTarjeta.RelacionarCuentaTarjeta(new RequestRelacionarCuentaTarjetaDTO { Cuenta = requestEjecutarGestionDTO.TarjetaCliente.CuentaAsignada, NumTarjeta = nuevoNumeroTarjeta.NumTarjeta });

                        if (!reasignar.Entidad)
                        {
                            _Error = true;
                            respuesta.Codigo = HttpStatusCode.Conflict;
                            respuesta.Mensaje = "Error al reasignar cuenta";
                            break;
                        }
                        await _gestionesDA.EditarEstadoTarjeta(4, requestEjecutarGestionDTO.TarjetaCliente.Id);
                        respuesta.Entidad = true;
                        respuesta.Codigo = HttpStatusCode.Accepted;
                        respuesta.Mensaje = nuevoNumeroTarjeta.NumTarjeta.ToString();
                        break;
                }

                var obtenerEncargadoEmail = await _seguridadDA.ObtenerEncargadoAgencia(requestEjecutarGestionDTO.Gestion.IdAgenciaAsignada);
                var obtenerUsuarioEmail = await _seguridadDA.ObtenerUsuarioPorId(requestEjecutarGestionDTO.Gestion.IdUsuarioIngreso);
                if (obtenerUsuarioEmail is not null)
                {
                    await EnviarCorreo(obtenerUsuarioEmail.Correo.Trim(), $"Hola {obtenerUsuarioEmail.PrimerNombre.Trim()} {obtenerUsuarioEmail.PrimerApellido.Trim()}, {obtenerEncargadoEmail.IdUsuario} dío respuesta a la gestíon número {requestEjecutarGestionDTO.Gestion.IdGestion}, ingrese a la <a href=\"http://192.168.1.67:9080\">plataforma administrativa de tarjetas de débitos</a> para más detalles.");
                }
                if (obtenerEncargadoEmail is not null)
                {
                    await EnviarCorreo(obtenerEncargadoEmail.Correo.Trim(), $"Hola {obtenerEncargadoEmail.PrimerNombre.Trim()} {obtenerEncargadoEmail.PrimerApellido.Trim()}, se notificó a {requestEjecutarGestionDTO.Gestion.IdUsuarioIngreso} su respuesta de la gestíon número {requestEjecutarGestionDTO.Gestion.IdGestion}, ingrese a la <a href=\"http://plataformaadmintd.bancopopular.hn:9080/\">plataforma administrativa de tarjetas de débitos</a> para más detalles.");
                }
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion(requestEjecutarGestionDTO.Gestion.IdGestion, "Ejecutar", $"Ejecutar tipo de gestión{requestEjecutarGestionDTO.Gestion.DescTipoGestion}", requestEjecutarGestionDTO.Gestion.IdUsuarioIngreso, _Error); }
        }
        string? Truncate(string value, int maxLength)
        {
            return value?.Length > maxLength ? value.Substring(0, maxLength) : value;
        }

        public async Task<RespuestaMicroservicio<bool>> ActualizarClienteAs400(ActualizarDatosClientePELDTO actualizarDatosClientePELDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                await _gestionesDA.ActualizarDatosClienteAS400(actualizarDatosClientePELDTO);
                respuesta.Entidad = true;
                respuesta.Codigo = HttpStatusCode.OK;
                respuesta.Mensaje = "Se actualizó la información del cliente en el core";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion("N/A", "Editar", $"Editar Información del cliente con cif {actualizarDatosClientePELDTO.Cif}", actualizarDatosClientePELDTO.Usuario, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<ResponseLiquidacionesDTO>>> Liquidaciones(RequestLiquidacionesDTO requestLiquidacionesDTO)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<ResponseLiquidacionesDTO>>();
            try
            {
                var request = await _gestionesDA.Liquidaciones(requestLiquidacionesDTO);
                if (request.Count() is 0)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontraron datos";
                    return respuesta;
                }
                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion("N/A", "Obtener", $"Obtener liquidaciones", requestLiquidacionesDTO.Usuario, _Error); }
        }
        public async Task<RespuestaMicroservicio<IEnumerable<ResponseMovimientosTarjetaDTO>>> ConsultaMovimientos(RequestMovimientosTarjetaDTO requestMovimientosTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<ResponseMovimientosTarjetaDTO>>();
            try
            {
                requestMovimientosTarjetaDTO.Cuenta = requestMovimientosTarjetaDTO?.Cuenta?.Replace("-", string.Empty);
                var request = await _gestionesDA.ConsultaMovimientos(requestMovimientosTarjetaDTO);
                if (request.Count() is 0)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontraron datos";
                    return respuesta;
                }
                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogGestion("N/A", "Obtener", $"Obtener movimientos de la cuenta no. {(string.IsNullOrEmpty(requestMovimientosTarjetaDTO.Cuenta) || requestMovimientosTarjetaDTO.Cuenta.Count() < 3 ? "N/A" : requestMovimientosTarjetaDTO.Cuenta.Substring(requestMovimientosTarjetaDTO.Cuenta.Length - 3))}", requestMovimientosTarjetaDTO.Usuario, _Error); }
        }       

        public async Task<RespuestaMicroservicio<ObtenerLogsDTO>> ObtenerLogs()
        {
            var respuesta = new RespuestaMicroservicio<ObtenerLogsDTO>();
            try
            {
                var obtener = await _logDA.ObtenerLogs();
                if (obtener is null)
                {
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = "Error al consultar";
                    return respuesta;
                }
                respuesta.Entidad = obtener;
                respuesta.Codigo = HttpStatusCode.OK;
                respuesta.Mensaje = "Consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
        }
        private async Task EnviarCorreo(string correo, string Mensaje)
        {
            var correos = new List<string>();
            correos?.Add(correo.Trim());
            await _helper.EnviaEmail(correos, Mensaje, "Notificación PATD");
        }

        public async Task<RespuestaMicroservicio<IEnumerable<LimitesGeneralesDTO>>> LimitesGenerales(string usuario)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<LimitesGeneralesDTO>>();
            try
            {
                var request = await _gestionesDA.LimitesGenerales();
                if (request is null)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontró información";
                }

                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally 
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    await _logDA.RegistrLogTarjeta(0, "Obtener", "Obtener listado de tarjetas", usuario, _Error);
                }
            }
        }

        public async Task<RespuestaMicroservicio<bool>> AceptarTodasTarjeta(int idAgencia, string user)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                await _gestionesDA.AceptarTodasTarjeta(idAgencia);
                respuesta.Entidad = true;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"exitó";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally { await _logDA.RegistrLogTarjeta(0, "Asignar", $"Asignación de todas las tarjetas en transito a la agencia {idAgencia}", user, _Error); }
        }

        public async Task<RespuestaMicroservicio<IEnumerable<LimitesGeneralesDTO>>> ObtenerListaLimites(string usuario)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<LimitesGeneralesDTO>>();
            try
            {
                var request = await _gestionesDA.ObtenerListaLimites();
                if (request is null)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontró información";
                }

                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Consulta exitosa";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    await _logDA.RegistrLogTarjeta(0, "Obtener", "Obtener listado de tarjetas", usuario, _Error);
                }
            }
        }

        public async Task<RespuestaMicroservicio<bool>> EditarLimites(LimitesGeneralesDTO limitesGeneralesDTO, string usuario)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var request = await _gestionesDA.EditarLimites(limitesGeneralesDTO);
                if (!request)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"No se encontró la categoría";
                }

                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Se editaron los límites exitosamente";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    await _logDA.RegistrLogTarjeta(0, "Obtener", "Obtener listado de tarjetas", usuario, _Error);
                }
            }
        }

        public async Task<RespuestaMicroservicio<bool>> CrearLimites(LimitesGeneralesDTO limitesGeneralesDTO, string usuario)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var request = await _gestionesDA.CrearLimites(limitesGeneralesDTO);
                if (!request)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.Conflict;
                    respuesta.Mensaje = $"Error en la creación";
                }

                respuesta.Entidad = request;
                respuesta.Codigo = HttpStatusCode.Accepted;
                respuesta.Mensaje = $"Se crearon los límites exitosamente";
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                respuesta.Mensaje = "Ocurrio un error interno";
                respuesta.Excepcion = e.Message;
                return respuesta;
            }
            finally
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    await _logDA.RegistrLogTarjeta(0, "Obtener", "Obtener listado de tarjetas", usuario, _Error);
                }
            }
        }
    }
}
