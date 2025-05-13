using BancoPopular.Respuestas.Respuesta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PATD.API.Application.Gestiones;
using PATD.API.Transversal.Gestiones;
using PATD.API.Transversal.LogsT;
using PATD.API.Transversal.Volcan;

namespace PATD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GestionesController : ControllerBase
    {
        private readonly IGestionesAPP _gestionesAPP;
        public GestionesController(IGestionesAPP gestionesAPP)
        {
            _gestionesAPP = gestionesAPP;
        }

        [Authorize]
        [HttpGet("/v1/BancoPopular/CrearLote")]
        public async Task<RespuestaMicroservicio<ResponseCrearLoteDTO>> RequestCrearLote([FromQuery] string user)
            => await _gestionesAPP.RequestCrearLote(user);

        [Authorize]
        [HttpPost("/v1/BancoPopular/CrearTarjeta")]
        public async Task<RespuestaMicroservicio<bool>> RequestCrearTarjeta(RequestCrearTarjetaDTO requestCrearTarjetaDTO)
            => await _gestionesAPP.RequestCrearTarjeta(requestCrearTarjetaDTO);

        [Authorize]
        [HttpPost("/v1/BancoPopular/AsignarTarjetaAgencia")]
        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> RequestAsignarTarjetaAgencia(RequestAsignarTarjetaAgenciaDTO requestAsignarTarjetaAgenciaDTO)
            => await _gestionesAPP.RequestAsignarTarjetaAgencia(requestAsignarTarjetaAgenciaDTO);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerTarjetaPorId")]
        public async Task<RespuestaMicroservicio<TarjetasDTO>> GetTarjetaPorId([FromQuery] int idTarjeta, [FromQuery] string user)
            => await _gestionesAPP.GetTarjetaPorId(idTarjeta, user);

        [Authorize]
        [HttpPost("/v1/BancoPopular/CrearGestion")]
        public async Task<RespuestaMicroservicio<RequestGestionDTO>> RequestCrearGestion(RequestCrearGestionDTO requestCrearGestionDTO)
            => await _gestionesAPP.RequestCrearGestion(requestCrearGestionDTO);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerTipoGestiones")]
        public async Task<RespuestaMicroservicio<IEnumerable<GetTipoGestionesDTO>>> GetTipoGestiones([FromQuery] string user)
            => await _gestionesAPP.GetTipoGestiones(user);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerTarjetas")]
        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> GetTarjetas([FromQuery] string user)
            => await _gestionesAPP.GetTarjetas(user);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerTarjetasAgencia")]
        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> TarjetasAgencia([FromQuery] int idAgencia, [FromQuery] string user)
            => await _gestionesAPP.TarjetasAgencia(idAgencia, user);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerTarjetasAgenciaAceptar")]
        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> TarjetasPorAceptar([FromQuery] int idAgencia, [FromQuery] string user)
            => await _gestionesAPP.TarjetasPorAceptar(idAgencia, user);
        
        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerGestiones")]
        public async Task<RespuestaMicroservicio<IEnumerable<GetGestiones>>> GetGestiones([FromQuery] string user)
            => await _gestionesAPP.GetGestiones(user);
        
        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerGestionesAgencia")]
        public async Task<RespuestaMicroservicio<IEnumerable<GetGestiones>>> GetGestionesAgencia([FromQuery] int idAgencia,[FromQuery] string user)
            => await _gestionesAPP.GetGestionesAgencia(idAgencia, user);
        
        [Authorize]
        [HttpGet("/v1/BancoPopular/AceptarTarjetaAgencia")]
        public async Task<RespuestaMicroservicio<bool>> AceptarTarjeta([FromQuery] int idTarjeta,[FromQuery] string user)
            => await _gestionesAPP.AceptarTarjeta(idTarjeta, user);
        
        [Authorize]
        [HttpGet("/v1/BancoPopular/AceptarTodasTarjetaAgencia")]
        public async Task<RespuestaMicroservicio<bool>> AceptarTodasTarjeta([FromQuery] int idAgencia,[FromQuery] string user)
            => await _gestionesAPP.AceptarTodasTarjeta(idAgencia, user);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerEstadoGestion")]
        public async Task<RespuestaMicroservicio<bool>> ObtenerEstadoGestion( [FromQuery] string idGestion, [FromQuery] string user)
            => await _gestionesAPP.ObtenerEstadoGestion(idGestion, user);

        [Authorize]
        [HttpGet("/v1/BancoPopular/EditarEstadoGestion")]
        public async Task<RespuestaMicroservicio<bool>> EditarEstadoGestion([FromQuery] int idEstado, [FromQuery] string idGestion, [FromQuery] string user)
            => await _gestionesAPP.EditarEstadoGestion(idEstado, idGestion, user);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerTarjetasDisponiblesAgencias")]
        public async Task<RespuestaMicroservicio<IEnumerable<TarjetasDisponiblesAgenciasDTO>>> TarjetasDisponiblesAgencias([FromQuery] string user)
            => await _gestionesAPP.TarjetasDisponiblesAgencias(user);

        [Authorize]
        [HttpPost("/v1/BancoPopular/EjecutarGestion")]
        public async Task<RespuestaMicroservicio<bool>> EjecutarGestion(RequestEjecutarGestionDTO requestEjecutarGestionDTO)
            => await _gestionesAPP.EjecutarGestion(requestEjecutarGestionDTO);

        [Authorize]
        [HttpGet("/v1/BancoPopular/EditarEstadoTarjeta")]
        public async Task<RespuestaMicroservicio<bool>> EditarEstadoTarjeta([FromQuery] int idEstado, [FromQuery] int IdTarjeta, [FromQuery] string user)
            => await _gestionesAPP.EditarEstadoTarjeta(idEstado, IdTarjeta, user);

        [HttpGet("/v1/BancoPopular/ConsultaInfoCliente")]
        public async Task<RespuestaMicroservicio<InformacionClienteDTO>> ConsultaInfoCliente([FromQuery] string identidad, [FromQuery] string user)
            => await _gestionesAPP.ConsultaInfoCliente(identidad, user);

        [Authorize]
        [HttpPost("/v1/BancoPopular/ActualizarDatosClienteCore")]
        public async Task<RespuestaMicroservicio<bool>> ActualizarClienteAs400(ActualizarDatosClientePELDTO actualizarDatosClientePELDTO)
            => await _gestionesAPP.ActualizarClienteAs400(actualizarDatosClientePELDTO);

        [Authorize]
        [HttpPost("/v1/BancoPopular/ObtenerLiquidaciones")]
        public async Task<RespuestaMicroservicio<IEnumerable<ResponseLiquidacionesDTO>>> Liquidaciones(RequestLiquidacionesDTO requestLiquidacionesDTO)
            => await _gestionesAPP.Liquidaciones(requestLiquidacionesDTO);

        [Authorize]
        [HttpPost("/v1/BancoPopular/ObtenerMovimientos")]
        public async Task<RespuestaMicroservicio<IEnumerable<ResponseMovimientosTarjetaDTO>>> ConsultaMovimientos(RequestMovimientosTarjetaDTO requestMovimientosTarjetaDTO)
            => await _gestionesAPP.ConsultaMovimientos(requestMovimientosTarjetaDTO);

        [Authorize]
        [HttpGet("/v1/BancoPopular/ObtenerLogs")]
        public async Task<RespuestaMicroservicio<ObtenerLogsDTO>> ObtenerLogs()
            => await _gestionesAPP.ObtenerLogs();
    }
}