using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PATD.API.Application.Seguridad;
using PATD.API.Transversal.Login;
using BancoPopular.Solicitudes.Solicitud;
using BancoPopular.Respuestas.Respuesta;
using PATD.API.Transversal.Usuario;
using PATD.API.Transversal.Catalogos;

namespace PATD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        private ISeguridadApp _SeguridadApp;
        public SeguridadController(ISeguridadApp seguridad)
        {
            _SeguridadApp = seguridad;
        }

        [HttpPost("/v1/BancoPopular/Login")]
        [AllowAnonymous]
        public async Task<RespuestaMicroservicio<LoginResponseDTO>> Login(LoginRequestDTO loginRequest) => await _SeguridadApp.LoginAPP(loginRequest);

        [Authorize]
        [HttpPost("/v1/BancoPopular/CrearUsuario")]
        public async Task<RespuestaMicroservicio<bool>> CrearUsuario(UsuarioRequestDTO usuarioRequest) => await _SeguridadApp.CrearUsuarioAPP(usuarioRequest);

        [Authorize]
        [HttpPost("/v1/BancoPopular/ActualizarUsuario")]
        public async Task<RespuestaMicroservicio<bool>> ActualizarUsuario(UsuarioRequestDTO usuarioRequest) => await _SeguridadApp.ActualizarUsuarioAPP(usuarioRequest);

        [Authorize]
        [HttpPost("/v1/BancoPopular/ActualizarEstadoUsuario")]
        public async Task<RespuestaMicroservicio<bool>> CambiarEstadoUsuario(UsuarioCambioEstadoDTO usuarioCambioEstadoRequest) => await _SeguridadApp.CambiarEstadoUsuarioAPP(usuarioCambioEstadoRequest);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerUsuarios")]
        public async Task<RespuestaMicroservicio<IEnumerable<UsuarioRequestDTO>>> ObtenerUsuarios() => await _SeguridadApp.ObtenerUsuariosAPP();

        //[Authorize]
        //[HttpGet("/v1/BancoPopular/ObtenerRoles")]
        //public async Task<RespuestaMicroservicio<IEnumerable<RolDTO>>> ObtenerRoles() => await _SeguridadApp.ObtenerRolesAPP();

        //[Authorize]
        //[HttpGet("/v1/BancoPopular/ObtenerAgencias")]
        //public async Task<RespuestaMicroservicio<IEnumerable<AgenciaDTO>>> ObtenerAgencias() => await _SeguridadApp.ObtenerAgenciasAPP();

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerCatalogos")]
        public async Task<RespuestaMicroservicio<CatalogosDTO>> ObtenerCatalogos() => await _SeguridadApp.ObtenerCatalogosAPP();
    }
}
