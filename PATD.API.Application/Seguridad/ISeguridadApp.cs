using BancoPopular.Respuestas.Respuesta;
using PATD.API.Transversal.Catalogos;
using PATD.API.Transversal.Login;
using PATD.API.Transversal.Usuario;

namespace PATD.API.Application.Seguridad
{
    public interface ISeguridadApp
    {
        Task<RespuestaMicroservicio<LoginResponseDTO>> LoginAPP(LoginRequestDTO loginRequest);
        Task<RespuestaMicroservicio<bool>> CrearUsuarioAPP(UsuarioRequestDTO usuarioRequest);
        Task<RespuestaMicroservicio<bool>> ActualizarUsuarioAPP(UsuarioRequestDTO usuarioRequest);
        Task<RespuestaMicroservicio<bool>> CambiarEstadoUsuarioAPP(UsuarioCambioEstadoDTO ioCambioEstadoRequest);
        Task<RespuestaMicroservicio<IEnumerable<UsuarioRequestDTO>>> ObtenerUsuariosAPP();
        //Task<RespuestaMicroservicio<IEnumerable<RolDTO>>> ObtenerRolesAPP();
        //Task<RespuestaMicroservicio<IEnumerable<AgenciaDTO>>> ObtenerAgenciasAPP();
        Task<RespuestaMicroservicio<CatalogosDTO>> ObtenerCatalogosAPP();
    }
}
