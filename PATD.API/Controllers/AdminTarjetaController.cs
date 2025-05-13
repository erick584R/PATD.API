using BancoPopular.Respuestas.Respuesta;
using Microsoft.AspNetCore.Mvc;
using PATD.API.Application.AdminTarjeta;
using PATD.API.Application.Gestiones;
using PATD.API.Transversal.AdminTarjeta;
using RequestAsignarTarjetaDTO = PATD.API.Transversal.AdminTarjeta.RequestAsignarTarjetaDTO;

namespace PATD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminTarjetaController : ControllerBase
    {
        private readonly IAdminTarjeta _adminTarjeta;
        private readonly IGestionesAPP _gestionesAPP;
        public AdminTarjetaController(IAdminTarjeta adminTarjeta, IGestionesAPP gestionesAPP)
        {
            _adminTarjeta = adminTarjeta;
            _gestionesAPP = gestionesAPP;
        }
        [HttpPost("/v1/BancoPopular/admin/cambiar-Pin")]
        public async Task<RespuestaMicroservicio<bool>> CambiarPin(RequestCambioPinDTO requestCambioPinDTO) 
            => await _adminTarjeta.CambiarPin(requestCambioPinDTO);

        [HttpPost("/v1/BancoPopular/admin/crear-tarjetas")]
        public async Task<RespuestaMicroservicio<ResponseCrearTarjetasDTO>> CrearTarjetas(IEnumerable<RequestCrearTarjetasDTO> requestCrearTarjetasDTO)
            => await _adminTarjeta.CrearTarjetas(requestCrearTarjetasDTO);

        [HttpPost("/v1/BancoPopular/admin/asignar-tarjeta")]
        public async Task<RespuestaMicroservicio<ResponseAsignarTarjetaDTO>> AsignarTarjeta(RequestAsignarTarjetaDTO requestAsignarTarjetaDTO)
            => await _adminTarjeta.AsignarTarjeta(requestAsignarTarjetaDTO);

        [HttpPost("/v1/BancoPopular/admin/relacionar-cuenta-tarjeta")]
        public async Task<RespuestaMicroservicio<bool>> RelacionarCuentaTarjeta(RequestRelacionarCuentaTarjetaDTO requestRelacionarCuentaTarjetaDTO)
            => await _adminTarjeta.RelacionarCuentaTarjeta(requestRelacionarCuentaTarjetaDTO);

        [HttpPost("/v1/BancoPopular/admin/activar-tarjeta")]
        public async Task<RespuestaMicroservicio<bool>> ActivarTarjeta(RequestActivarTarjetaDTO requestActivarTarjetaDTO)
            => await _adminTarjeta.ActivarTarjeta(requestActivarTarjetaDTO);

        [HttpPost("/v1/BancoPopular/admin/cancelar-tarjeta")]
        public async Task<RespuestaMicroservicio<bool>> CancelarTarjeta(RequestCancelarTarjetaDTO requestCancelarTarjetaDTO)
            => await _adminTarjeta.CancelarTarjeta(requestCancelarTarjetaDTO);

        [HttpPost("/v1/BancoPopular/admin/bloquear-tarjeta")]
        public async Task<RespuestaMicroservicio<bool>> BloquearTarjeta(RequestBloquearTarjetaDTO requestBloquearTarjetaDTO)
            => await _adminTarjeta.BloquearTarjeta(requestBloquearTarjetaDTO);

        [HttpPost("/v1/BancoPopular/admin/desbloquear-tarjeta")]
        public async Task<RespuestaMicroservicio<bool>> DesbloquearTarjeta(RequestDesbloquearTarjetaDTO requestDesbloquearTarjetaDTO)
            => await _adminTarjeta.DesbloquearTarjeta(requestDesbloquearTarjetaDTO);

        [HttpPost("/v1/BancoPopular/admin/autorizar-pais")]
        public async Task<RespuestaMicroservicio<bool>> AutorizarPais(RequestAutorizarPaisDTO requestAutorizarPaisDTO)
            => await _adminTarjeta.AutorizarPais(requestAutorizarPaisDTO);

        [HttpPost("/v1/BancoPopular/admin/modificar-limites-tarjeta")]
        public async Task<RespuestaMicroservicio<bool>> LimitesTarjeta(RequestLimitesTarjetaDTO requestLimitesTarjetaDTO)
            => await _adminTarjeta.LimitesTarjeta(requestLimitesTarjetaDTO);

        [HttpPost("/v1/BancoPopular/admin/consulta-cliente")]
        public async Task<RespuestaMicroservicio<IEnumerable<ResponseConsultaClienteDTO>>> ConsultaCliente(RequestConsultaClienteDTO requestConsultaClienteDTO)
            => await _adminTarjeta.ConsultaCliente(requestConsultaClienteDTO);

        [HttpPost("/v1/BancoPopular/admin/asignar-tarjeta-agencia")]
        public async Task<RespuestaMicroservicio<bool>> DistribuirTarjetas(RequestDistribuirTarjetasDTO requestDistribuirTarjetasDTO)
            => await _adminTarjeta.DistribuirTarjetas(requestDistribuirTarjetasDTO);

        [HttpGet("/v1/BancoPopular/admin/obtener-paises-iso")]
        public async Task<RespuestaMicroservicio<IEnumerable<CPaisesIsoDTO>>> ObtenerPaisesIso()
            => await _adminTarjeta.ObtenerPaisesIso();

        [HttpGet("/v1/BancoPopular/admin/obtener-limites-generales")]
        public async Task<RespuestaMicroservicio<IEnumerable<LimitesGeneralesDTO>>> LimitesGenerales([FromQuery] string usuario)
            => await _gestionesAPP.LimitesGenerales(usuario);

        [HttpPost("/v1/BancoPopular/admin/obtener-trx-td")]
        public async Task<RespuestaMicroservicio<IEnumerable<ResponseTrxTdDto>>> RequestTrxTd(RequestTrxTdDto requestTrxTdDto)
            => await _adminTarjeta.RequestTrxTd(requestTrxTdDto);

        [HttpGet("/v1/BancoPopular/admin/obtener-lista-limites")]
        public async Task<RespuestaMicroservicio<IEnumerable<LimitesGeneralesDTO>>> ObtenerListaLimites([FromQuery] string usuario)
            => await _gestionesAPP.ObtenerListaLimites(usuario);

        [HttpPost("/v1/BancoPopular/admin/editar-limites")]
        public async Task<RespuestaMicroservicio<bool>> EditarLimites(LimitesGeneralesDTO limitesGeneralesDTO,[FromQuery] string usuario)
            => await _gestionesAPP.EditarLimites(limitesGeneralesDTO, usuario);

        [HttpPost("/v1/BancoPopular/admin/crear-limites")]
        public async Task<RespuestaMicroservicio<bool>> CrearLimites(LimitesGeneralesDTO limitesGeneralesDTO,[FromQuery] string usuario)
            => await _gestionesAPP.CrearLimites(limitesGeneralesDTO, usuario);
    }
}
