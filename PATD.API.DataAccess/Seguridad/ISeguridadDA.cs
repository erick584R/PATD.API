using BancoPopular.Respuestas.Respuesta;
using PATD.API.Transversal.AdminTarjeta;
using PATD.API.Transversal.Catalogos;
using PATD.API.Transversal.Login;
using PATD.API.Transversal.Usuario;

namespace PATD.API.DataAccess.Seguridad
{
    public interface ISeguridadDA
    {
        Task<RespuestaMicroservicio<LoginResponseDTO>> LoginDA(LoginRequestDTO loginRequest);
        Task<RespuestaMicroservicio<bool>> CrearUsuarioDA(UsuarioRequestDTO usuarioRequest);
        Task<RespuestaMicroservicio<bool>> ActualizarUsuarioDA(UsuarioRequestDTO usuarioRequest);
        Task<RespuestaMicroservicio<bool>> CambiarEstadoUsuarioDA(UsuarioCambioEstadoDTO usuarioCambioEstadoRequest);
        Task<RespuestaMicroservicio<IEnumerable<UsuarioRequestDTO>>> ObtenerUsuariosDA();
        Task<IEnumerable<CRolesUsuarioDTO>> ObtenerRolesDA();
        Task<IEnumerable<CAgenciaDTO>> ObtenerAgenciasDA();
        Task<IEnumerable<DescriptivosDTO>> TiposTarjetasDA();
        Task<IEnumerable<CEstadosTarjetasDTO>> EstadosTarjetasDA();
        Task<IEnumerable<Descriptivos2DTO>> MotivosCancelacionDA();
        Task<IEnumerable<Descriptivos2DTO>> RazoneReposicionDA();
        Task<IEnumerable<Descriptivos2DTO>> MotivosBloqueoDA();
        Task<IEnumerable<DescriptivosDTO>> EstadosLogisticoTarjetasDA();
        Task<IEnumerable<DescriptivosDTO>> TipoGestionDA();
        Task<IEnumerable<DescriptivosDTO>> EstadoGestionDA();
        Task<IEnumerable<CPaisesIsoDTO>> ObtenerPaisesIso();
        Task<UsuarioRequestDTO> ObtenerUsuarioPorId(string usuario);
        Task<UsuarioRequestDTO> ObtenerEncargadoAgencia(int? Agencia);
    }
}
