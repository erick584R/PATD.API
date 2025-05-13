using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using BancoPopular.Respuestas.Respuesta;
using Microsoft.IdentityModel.Tokens;
using PATD.API.DataAccess.Seguridad;
using PATD.API.Transversal.Catalogos;
using PATD.API.Transversal.Login;
using PATD.API.Transversal.Usuario;

namespace PATD.API.Application.Seguridad
{
    public class SeguridadApp : ISeguridadApp
    {
        private ISeguridadDA _SeguridadDA;
        public SeguridadApp(ISeguridadDA seguridad)
        {
            _SeguridadDA = seguridad;
        }

        public async Task<RespuestaMicroservicio<LoginResponseDTO>> LoginAPP(LoginRequestDTO loginRequest)
        {
            RespuestaMicroservicio<LoginResponseDTO> respuestaMicroservicio = new();

            try
            {
                respuestaMicroservicio = await _SeguridadDA.LoginDA(loginRequest);
                if (respuestaMicroservicio.Codigo.ToString() is "200" or "OK" or "Ok")
                {
                    string respuestaToken = await CrearJwt(loginRequest.IdUsuario.Trim(), respuestaMicroservicio.Entidad.IdRol.ToString(), respuestaMicroservicio.Entidad.Correo);

                    if (respuestaToken.Contains("Token no creado"))
                    {
                        respuestaMicroservicio.Mensaje = respuestaToken;
                        respuestaMicroservicio.Codigo = HttpStatusCode.Unauthorized;
                        respuestaMicroservicio.Entidad = new();
                    }
                    else respuestaMicroservicio.Entidad.JwtSesion = respuestaToken;
                }
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Mensaje = $"{e.Message} - API.APP";
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
            }

            return respuestaMicroservicio;
        }
        public async Task<RespuestaMicroservicio<bool>> CrearUsuarioAPP(UsuarioRequestDTO usuarioRequest)
        {
            RespuestaMicroservicio<bool> respuestaMicroservicio = new();

            try
            {
                respuestaMicroservicio = await _SeguridadDA.CrearUsuarioDA(usuarioRequest);
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Mensaje = e.Message;
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
                respuestaMicroservicio.Entidad = false;
            }

            return respuestaMicroservicio;
        }
        public async Task<RespuestaMicroservicio<bool>> ActualizarUsuarioAPP(UsuarioRequestDTO usuarioRequest)
        {
            RespuestaMicroservicio<bool> respuestaMicroservicio = new();

            try
            {
                respuestaMicroservicio = await _SeguridadDA.ActualizarUsuarioDA(usuarioRequest);
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Mensaje = e.Message;
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
                respuestaMicroservicio.Entidad = false;
            }

            return respuestaMicroservicio;
        }
        public async Task<RespuestaMicroservicio<bool>> CambiarEstadoUsuarioAPP(UsuarioCambioEstadoDTO usuarioCambioEstadoRequest)
        {
            RespuestaMicroservicio<bool> respuestaMicroservicio = new();

            try
            {
                respuestaMicroservicio = await _SeguridadDA.CambiarEstadoUsuarioDA(usuarioCambioEstadoRequest);
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Mensaje = e.Message;
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
                respuestaMicroservicio.Entidad = false;
            }

            return respuestaMicroservicio;
        }
        public async Task<RespuestaMicroservicio<IEnumerable<UsuarioRequestDTO>>> ObtenerUsuariosAPP()
        {
            RespuestaMicroservicio<IEnumerable<UsuarioRequestDTO>> respuestaMicroservicio = new();

            try
            {
                respuestaMicroservicio = await _SeguridadDA.ObtenerUsuariosDA();
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Mensaje = e.Message;
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
            }

            return respuestaMicroservicio;
        }
        private async Task<string> CrearJwt(string usuario, string rol, string correo)
        {
            try
            {
                var claims = new[]{
                    new Claim(ClaimTypes.NameIdentifier, usuario),
                    new Claim(ClaimTypes.Role, rol),
                    new Claim(ClaimTypes.Email, correo)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TokenKeyPATD")));
                var digitalSign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(20), signingCredentials: digitalSign);

                string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

                return token;
            }
            catch (Exception e)
            {
                return $"Token no creado {e.Message}";
            }
        }

        //public async Task<RespuestaMicroservicio<IEnumerable<RolDTO>>> ObtenerRolesAPP()
        //{
        //    RespuestaMicroservicio<IEnumerable<RolDTO>> respuestaMicroservicio = new();

        //    try
        //    {
        //        respuestaMicroservicio = await _SeguridadDA.ObtenerRolesDA();
        //    }
        //    catch (Exception e)
        //    {
        //        respuestaMicroservicio.Mensaje = e.Message;
        //        respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
        //    }

        //    return respuestaMicroservicio;
        //}

        //public async Task<RespuestaMicroservicio<IEnumerable<CAgenciaDTO>>> ObtenerAgenciasAPP()
        //{
        //    RespuestaMicroservicio<IEnumerable<CAgenciaDTO>> respuestaMicroservicio = new();

        //    try
        //    {
        //        respuestaMicroservicio = await _SeguridadDA.ObtenerAgenciasDA();
        //    }
        //    catch (Exception e)
        //    {
        //        respuestaMicroservicio.Mensaje = e.Message;
        //        respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
        //    }

        //    return respuestaMicroservicio;
        //}

        public async Task<RespuestaMicroservicio<CatalogosDTO>> ObtenerCatalogosAPP()
        {
            RespuestaMicroservicio<CatalogosDTO> respuestaMicroservicio = new();

            try
            {
                var obtenerAgencias = await _SeguridadDA.ObtenerAgenciasDA();
                var obtenerRoles = await _SeguridadDA.ObtenerRolesDA();
                var obtenerTiposTarjeta = await _SeguridadDA.TiposTarjetasDA();
                var obtenerEstadosTarjeta = await _SeguridadDA.EstadosTarjetasDA();
                var obtenerMotivosCancelacion = await _SeguridadDA.MotivosCancelacionDA();
                var obtenerRazonesReposicion = await _SeguridadDA.RazoneReposicionDA();
                var obtenerMotivosBloqueo = await _SeguridadDA.MotivosBloqueoDA();
                var obtenerEstadosLogisticos = await _SeguridadDA.EstadosLogisticoTarjetasDA();
                var obtenerTipoGestion = await _SeguridadDA.TipoGestionDA();
                var obtenerEstadoGestion = await _SeguridadDA.EstadoGestionDA();
                var obtenerPaisesISO = await _SeguridadDA.ObtenerPaisesIso();

                respuestaMicroservicio.Entidad = new CatalogosDTO { CatalagoAgencias = obtenerAgencias.AsEnumerable(), RolesUsuarios = obtenerRoles.AsEnumerable(), TiposTarjetas = obtenerTiposTarjeta.AsEnumerable(), EstadosTarjetas = obtenerEstadosTarjeta.AsEnumerable(), MotivosCancelacion = obtenerMotivosCancelacion.AsEnumerable(), RazonesReposicion = obtenerRazonesReposicion.AsEnumerable(), MotivosBloqueo = obtenerMotivosBloqueo.AsEnumerable(), EstadosLogisticoTarjetas = obtenerEstadosLogisticos.AsEnumerable(), TipoGestion = obtenerTipoGestion.AsEnumerable(), EstadoGestion = obtenerEstadoGestion.AsEnumerable(), PaisesIso= obtenerPaisesISO.AsEnumerable() };
                respuestaMicroservicio.Codigo = HttpStatusCode.Accepted;
                respuestaMicroservicio.Mensaje = "Exito";
            }
            catch (Exception e)
            {
                respuestaMicroservicio.Mensaje = "Ocurrio un error interno";
                respuestaMicroservicio.Excepcion = e.Message;
                respuestaMicroservicio.Codigo = HttpStatusCode.InternalServerError;
            }
            return respuestaMicroservicio;
        }
    }
}
