using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BancoPopular.Respuestas.Respuesta;
using PATD.API.Transversal.AdminTarjeta;
using PATD.API.Transversal.Gestiones;
using PATD.API.Transversal.LogsT;
using PATD.API.Transversal.Volcan;

namespace PATD.API.Application.Gestiones
{
    public interface IGestionesAPP
    {
        Task<RespuestaMicroservicio<bool>> RequestCrearTarjeta(RequestCrearTarjetaDTO requestCrearTarjetaDTO);
        Task<RespuestaMicroservicio<ResponseCrearLoteDTO>> RequestCrearLote(string user);
        Task<RespuestaMicroservicio<RequestGestionDTO>> RequestCrearGestion(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> RequestAsignarTarjetaAgencia(RequestAsignarTarjetaAgenciaDTO requestAsignarTarjetaAgenciaDTO);
        Task<RespuestaMicroservicio<IEnumerable<GetTipoGestionesDTO>>> GetTipoGestiones(string user);
        Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> GetTarjetas(string user);
        Task<RespuestaMicroservicio<IEnumerable<GetGestiones>>> GetGestiones(string user);
        Task<RespuestaMicroservicio<IEnumerable<TarjetasDisponiblesAgenciasDTO>>> TarjetasDisponiblesAgencias(string user);
        Task<RespuestaMicroservicio<InformacionClienteDTO>> ConsultaInfoCliente(string identidad, string user);
        Task<RespuestaMicroservicio<bool>> EditarEstadoTarjeta(int idEstado, int IdTarjeta, string user);
        Task<RespuestaMicroservicio<IEnumerable<GetGestiones>>> GetGestionesAgencia(int idAgencia, string user);
        Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> TarjetasAgencia(int idAgencia, string user);
        Task<RespuestaMicroservicio<TarjetasDTO>> GetTarjetaPorId(int idTarjeta, string user);
        Task<RespuestaMicroservicio<bool>> AceptarTarjeta(int idTarjeta, string user);
        Task<RespuestaMicroservicio<bool>> EditarEstadoGestion(int idEstado, string IdGestion, string user);
        Task<RespuestaMicroservicio<bool>> ObtenerEstadoGestion(string IdGestion, string user);
        Task<RespuestaMicroservicio<IEnumerable<TarjetasDTO>>> TarjetasPorAceptar(int idAgencia, string user);
        Task<RespuestaMicroservicio<bool>> EjecutarGestion(RequestEjecutarGestionDTO requestEjecutarGestionDTO);
        Task<RespuestaMicroservicio<bool>> ActualizarClienteAs400(ActualizarDatosClientePELDTO actualizarDatosClientePELDTO);
        Task<RespuestaMicroservicio<IEnumerable<ResponseLiquidacionesDTO>>> Liquidaciones(RequestLiquidacionesDTO requestLiquidacionesDTO);
        Task<RespuestaMicroservicio<IEnumerable<ResponseMovimientosTarjetaDTO>>> ConsultaMovimientos(RequestMovimientosTarjetaDTO requestMovimientosTarjetaDTO);
        Task<RespuestaMicroservicio<ObtenerLogsDTO>> ObtenerLogs();
        Task<RespuestaMicroservicio<IEnumerable<LimitesGeneralesDTO>>> LimitesGenerales(string usuario);
        Task<RespuestaMicroservicio<bool>> AceptarTodasTarjeta(int idAgencia, string user);
        Task<RespuestaMicroservicio<IEnumerable<LimitesGeneralesDTO>>> ObtenerListaLimites(string usuario);
        Task<RespuestaMicroservicio<bool>> EditarLimites(LimitesGeneralesDTO limitesGeneralesDTO, string usuario);
        Task<RespuestaMicroservicio<bool>> CrearLimites(LimitesGeneralesDTO limitesGeneralesDTO, string usuario);
    }
}
