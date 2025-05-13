using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Net;
using BancoPopular.Infraestructura.Docker;
using BancoPopular.Respuestas.Respuesta;
using Dapper;
using PATD.API.Transversal.AdminTarjeta;
using PATD.API.Transversal.Catalogos;
using PATD.API.Transversal.Login;
using PATD.API.Transversal.Usuario;

namespace PATD.API.DataAccess.Seguridad
{
    public class SeguridadDA : ISeguridadDA
    {
        private IInfraestructuraDocker _Request;
        public SeguridadDA(IInfraestructuraDocker infraestructura)
        {
            _Request = infraestructura;
        }

        public async Task<RespuestaMicroservicio<LoginResponseDTO>> LoginDA(LoginRequestDTO loginRequest)
        {
            RespuestaMicroservicio<LoginResponseDTO> respuestaMicroservicio = new();

            try
            {
                var domainContext = new PrincipalContext(ContextType.Domain);
                var validate = domainContext.ValidateCredentials(loginRequest.IdUsuario, loginRequest.Contraseña);
                respuestaMicroservicio.Entidad = new();
                respuestaMicroservicio.Entidad.ValidacionActiveDirectory = validate;


                if (!validate)
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Unauthorized;
                    respuestaMicroservicio.Mensaje = "Credenciales inválidas";
                    return respuestaMicroservicio;
                }

                var parametros = new DynamicParameters();
                parametros.Add("@IdUsuario", loginRequest.IdUsuario, DbType.String, ParameterDirection.Input);
                var datosUsuarioSQL = await _Request.ObtenerSQL<LoginResponseDTO>("SP_OBTENER_DATOS_USUARIO", parametros);

                if (datosUsuarioSQL is null)
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Unauthorized;
                    respuestaMicroservicio.Mensaje = "Credenciales inválidas";
                    return respuestaMicroservicio;
                }

                if (datosUsuarioSQL.Estado is not "A")
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Unauthorized;
                    respuestaMicroservicio.Mensaje = "IdUsuario no activo";
                    return respuestaMicroservicio;
                }

                respuestaMicroservicio.Codigo = HttpStatusCode.OK;
                respuestaMicroservicio.Mensaje = "Credenciales válidas";
                respuestaMicroservicio.Entidad = datosUsuarioSQL;
                respuestaMicroservicio.Entidad.ValidacionActiveDirectory = true;
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Mensaje = $"{e.Message} - API.DA";
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
            }

            return respuestaMicroservicio;
        }


        #region USUARIOS
        public async Task<RespuestaMicroservicio<bool>> CrearUsuarioDA(UsuarioRequestDTO usuarioRequest)
        {
            RespuestaMicroservicio<bool> respuestaMicroservicio = new();

            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@IdUsuario", usuarioRequest.IdUsuario, DbType.String, ParameterDirection.Input);
                parametros.Add("@PrimerNombre", usuarioRequest.PrimerNombre, DbType.String, ParameterDirection.Input);
                parametros.Add("@SegundoNombre", usuarioRequest.SegundoNombre, DbType.String, ParameterDirection.Input);
                parametros.Add("@PrimerApellido", usuarioRequest.PrimerApellido, DbType.String, ParameterDirection.Input);
                parametros.Add("@SegundoApellido", usuarioRequest.SegundoApellido, DbType.String, ParameterDirection.Input);
                parametros.Add("@IdRol", usuarioRequest.IdRol, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@IdAgenciaAsignada", usuarioRequest.IdAgenciaAsignada, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@Estado", usuarioRequest.Estado, DbType.String, ParameterDirection.Input);
                parametros.Add("@Correo", usuarioRequest.Correo, DbType.String, ParameterDirection.Input);
                var respuestaSql = await _Request.ObtenerSQL<string>("SP_CREAR_USUARIO", parametros);

                if (respuestaSql is null)
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Conflict;
                    respuestaMicroservicio.Mensaje = "No creado";
                    respuestaMicroservicio.Entidad = false;
                    return respuestaMicroservicio;
                }

                if (respuestaSql is not "OK")
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Conflict;
                    respuestaMicroservicio.Mensaje = respuestaSql;
                    respuestaMicroservicio.Entidad = false;
                    return respuestaMicroservicio;
                }

                respuestaMicroservicio.Codigo = HttpStatusCode.Created;
                respuestaMicroservicio.Mensaje = "IdUsuario creado";
                respuestaMicroservicio.Entidad = true;
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
                respuestaMicroservicio.Mensaje = e.Message;
                respuestaMicroservicio.Entidad = false;
            }

            return respuestaMicroservicio;
        }
        public async Task<RespuestaMicroservicio<bool>> ActualizarUsuarioDA(UsuarioRequestDTO usuarioRequest)
        {
            RespuestaMicroservicio<bool> respuestaMicroservicio = new();

            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@IdUsuario", usuarioRequest.IdUsuario, DbType.String, ParameterDirection.Input);
                parametros.Add("@PrimerNombre", usuarioRequest.PrimerNombre, DbType.String, ParameterDirection.Input);
                parametros.Add("@SegundoNombre", usuarioRequest.SegundoNombre, DbType.String, ParameterDirection.Input);
                parametros.Add("@PrimerApellido", usuarioRequest.PrimerApellido, DbType.String, ParameterDirection.Input);
                parametros.Add("@SegundoApellido", usuarioRequest.SegundoApellido, DbType.String, ParameterDirection.Input);
                parametros.Add("@IdRol", usuarioRequest.IdRol, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@IdAgenciaAsignada", usuarioRequest.IdAgenciaAsignada, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@Correo", usuarioRequest.Correo, DbType.String, ParameterDirection.Input);
                var respuestaSql = await _Request.ObtenerSQL<string>("SP_ACTUALIZAR_USUARIO", parametros);

                if (respuestaSql is null)
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Conflict;
                    respuestaMicroservicio.Mensaje = "No actualizado";
                    respuestaMicroservicio.Entidad = false;
                    return respuestaMicroservicio;
                }

                if (respuestaSql is not "OK")
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Conflict;
                    respuestaMicroservicio.Mensaje = respuestaSql;
                    respuestaMicroservicio.Entidad = false;
                    return respuestaMicroservicio;
                }

                respuestaMicroservicio.Codigo = HttpStatusCode.Accepted;
                respuestaMicroservicio.Mensaje = "IdUsuario actualizado";
                respuestaMicroservicio.Entidad = true;
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
                respuestaMicroservicio.Mensaje = e.Message;
                respuestaMicroservicio.Entidad = false;
            }

            return respuestaMicroservicio;
        }
        public async Task<RespuestaMicroservicio<bool>> CambiarEstadoUsuarioDA(UsuarioCambioEstadoDTO usuarioCambioEstadoRequest)
        {
            RespuestaMicroservicio<bool> respuestaMicroservicio = new();

            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@IdUsuario", usuarioCambioEstadoRequest.IdUsuario, DbType.String, ParameterDirection.Input);
                parametros.Add("@Estado", usuarioCambioEstadoRequest.Estado, DbType.String, ParameterDirection.Input);
                var respuestaSql = await _Request.ObtenerSQL<string>("SP_ACTUALIZAR_ESTADO_USUARIO", parametros);

                if (respuestaSql is null)
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Conflict;
                    respuestaMicroservicio.Mensaje = "No actualizado";
                    respuestaMicroservicio.Entidad = false;
                    return respuestaMicroservicio;
                }

                if (respuestaSql is not "OK")
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.Conflict;
                    respuestaMicroservicio.Mensaje = respuestaSql;
                    respuestaMicroservicio.Entidad = false;
                    return respuestaMicroservicio;
                }

                respuestaMicroservicio.Codigo = HttpStatusCode.Accepted;
                respuestaMicroservicio.Mensaje = "IdUsuario actualizado";
                respuestaMicroservicio.Entidad = true;
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
                respuestaMicroservicio.Mensaje = e.Message;
                respuestaMicroservicio.Entidad = false;
            }

            return respuestaMicroservicio;
        }
        public async Task<RespuestaMicroservicio<IEnumerable<UsuarioRequestDTO>>> ObtenerUsuariosDA()
        {
            RespuestaMicroservicio<IEnumerable<UsuarioRequestDTO>> respuestaMicroservicio = new();

            try
            {
                var respuestaSql = await _Request.ObtenerListaSQL<UsuarioRequestDTO>("SP_OBTENER_USUARIOS");

                if (respuestaSql is null)
                {
                    respuestaMicroservicio.Codigo = HttpStatusCode.NotFound;
                    respuestaMicroservicio.Mensaje = "Datos no encontrados";
                    return respuestaMicroservicio;
                }

                respuestaMicroservicio.Codigo = HttpStatusCode.OK;
                respuestaMicroservicio.Mensaje = "Datos obtenidos";
                respuestaMicroservicio.Entidad = respuestaSql;
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
                respuestaMicroservicio.Mensaje = e.Message;
            }

            return respuestaMicroservicio;
        }

        public async Task<IEnumerable<CRolesUsuarioDTO>> ObtenerRolesDA()
        {
            return await _Request.ObtenerListaSQL<CRolesUsuarioDTO>("SP_OBTENER_ROLES");
        }

        public async Task<IEnumerable<CAgenciaDTO>> ObtenerAgenciasDA()
        {
            return await _Request.ObtenerListaSQL<CAgenciaDTO>("SP_OBTENER_AGENCIAS");

        }

        public async Task<IEnumerable<DescriptivosDTO>> TiposTarjetasDA()
        {
            return await _Request.ObtenerListaSQL<DescriptivosDTO>("SP_OBTENER_TIPO_TARJETA");
        }

        public async Task<IEnumerable<CEstadosTarjetasDTO>> EstadosTarjetasDA()
        {
            return await _Request.ObtenerListaSQL<CEstadosTarjetasDTO>("SP_OBTENER_ESTADO_TARJETA");
        }

        public async Task<IEnumerable<Descriptivos2DTO>> MotivosCancelacionDA()
        {
            return await _Request.ObtenerListaSQL<Descriptivos2DTO>("SP_OBTENER_MOTIVOS_CANCELACION");
        }

        public async Task<IEnumerable<Descriptivos2DTO>> RazoneReposicionDA()
        {
            return await _Request.ObtenerListaSQL<Descriptivos2DTO>("SP_OBTENER_RAZONES_REPOSICION");
        }

        public async Task<IEnumerable<Descriptivos2DTO>> MotivosBloqueoDA()
        {
            return await _Request.ObtenerListaSQL<Descriptivos2DTO>("SP_OBTENER_MOTIVOS_BLOQUEO");
        }

        public async Task<IEnumerable<DescriptivosDTO>> EstadosLogisticoTarjetasDA()
        {
            return await _Request.ObtenerListaSQL<DescriptivosDTO>("SP_OBTENER_ESTADOS_LOGISTICOS_TARJETAS");
        }
        public async Task<IEnumerable<DescriptivosDTO>> TipoGestionDA()
        {
            return await _Request.ObtenerListaSQL<DescriptivosDTO>("SP_OBTENER_TIPO_GESTION");
        }

        public async Task<IEnumerable<DescriptivosDTO>> EstadoGestionDA()
        {
            return await _Request.ObtenerListaSQL<DescriptivosDTO>("SP_OBTENER_ESTADO_GESTION");
        }
        public async Task<IEnumerable<CPaisesIsoDTO>> ObtenerPaisesIso()
        {
            return await _Request.ObtenerListaSQL<CPaisesIsoDTO>("SP_OBTENER_CPAISES_ISO");
        }
        public async Task<UsuarioRequestDTO> ObtenerUsuarioPorId(string usuario)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Usuario", usuario);
            return await _Request.ObtenerSQL<UsuarioRequestDTO>("SP_OBTENER_USUARIO_POR_ID", parametros);
        }

        public async Task<UsuarioRequestDTO> ObtenerEncargadoAgencia(int? Agencia)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdAgencia", Agencia);
            return await _Request.ObtenerSQL<UsuarioRequestDTO>("SP_OBTENER_ENCARGADO_AGENCIA", parametros);
        }
        #endregion
    }
}
