using BancoPopular.Respuestas.Respuesta;
using PATD.API.Transversal.AdminTarjeta;

namespace PATD.API.Application.AdminTarjeta
{
    public interface IAdminTarjeta
    {
        Task<RespuestaMicroservicio<bool>> CambiarPin(RequestCambioPinDTO requestCambioPinDTO);
        Task<RespuestaMicroservicio<ResponseCrearTarjetasDTO>> CrearTarjetas(IEnumerable<RequestCrearTarjetasDTO> requestCrearTarjetasDTO);
        Task<RespuestaMicroservicio<ResponseAsignarTarjetaDTO>> AsignarTarjeta(RequestAsignarTarjetaDTO requestAsignarTarjetaDTO);
        Task<RespuestaMicroservicio<bool>> RelacionarCuentaTarjeta(RequestRelacionarCuentaTarjetaDTO requestRelacionarCuentaTarjetaDTO);
        Task<RespuestaMicroservicio<bool>> ActivarTarjeta(RequestActivarTarjetaDTO requestActivarTarjetaDTO);
        Task<RespuestaMicroservicio<bool>> BloquearTarjeta(RequestBloquearTarjetaDTO requestBloquearTarjetaDTO);
        Task<RespuestaMicroservicio<bool>> DesbloquearTarjeta(RequestDesbloquearTarjetaDTO requestDesbloquearTarjetaDTO);
        Task<RespuestaMicroservicio<bool>> CancelarTarjeta(RequestCancelarTarjetaDTO requestCancelarTarjetaDTO);
        Task<RespuestaMicroservicio<bool>> AutorizarPais(RequestAutorizarPaisDTO requestAutorizarPaisDTO);
        Task<RespuestaMicroservicio<bool>> LimitesTarjeta(RequestLimitesTarjetaDTO requestLimitesTarjetaDTO);
        Task<RespuestaMicroservicio<IEnumerable<ResponseConsultaClienteDTO>>> ConsultaCliente(RequestConsultaClienteDTO requestConsultaClienteDTO);
        Task<RespuestaMicroservicio<bool>> DistribuirTarjetas(RequestDistribuirTarjetasDTO requestDistribuirTarjetasDTO);
        Task<RespuestaMicroservicio<IEnumerable<CPaisesIsoDTO>>> ObtenerPaisesIso();
        Task<RespuestaMicroservicio<IEnumerable<ResponseTrxTdDto>>> RequestTrxTd(RequestTrxTdDto requestTrxTdDto);
    }
}
