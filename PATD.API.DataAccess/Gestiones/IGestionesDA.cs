using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PATD.API.Transversal.AdminTarjeta;
using PATD.API.Transversal.Gestiones;
using PATD.API.Transversal.Notificaciones;

namespace PATD.API.DataAccess.Gestiones
{
    public interface IGestionesDA
    {
        Task<string> RequestCrearTarjeta(RequestCrearTarjetaDTO requestCrearTarjetaDTO);
        Task<string> RequestCrearLote();
        Task<string> RequestCrearGestion(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<string> RequestAsignarTarjeta(RequestCrearGestionDTO requestCrearGestionDTO, string numTarjeta);
        Task<string> RequestReAsignarTarjeta(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<string> RequestCancelarTarjeta(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<string> RequestUpdateAliasTarjeta(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<string> RequestBloqueoTarjeta(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<string> RequestDesbloqueoTarjeta(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<string> RequestUpdateCuentaTarjeta(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<string> RequestReposicionTarjeta(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<bool> ValidaGestion(RequestCrearGestionDTO requestCrearGestionDTO);
        Task<IEnumerable<TarjetasDTO>> RequestAsignarTarjetaAgencia(RequestAsignarTarjetaAgenciaDTO requestAsignarTarjetaAgenciaDTO);
        Task<IEnumerable<GetTipoGestionesDTO>> GetTipoGestiones();
        Task<IEnumerable<TarjetasDTO>> GetTarjetas();
        Task<IEnumerable<GetGestiones>> GetGestiones(string user);
        Task<IEnumerable<TarjetasDisponiblesAgenciasDTO>> TarjetasDisponiblesAgencias();
        Task<InformacionClienteDTO> InformacionCliente(string identidad);
        Task<IEnumerable<CuentasDTO>> ObtenerCuentas(string cif);
        Task<IEnumerable<TarjetasDTO>> GetTarjetasCif(string cif);
        Task<IEnumerable<TarjetasDTO>> TarjetasPorAceptar(int idAgencia);
        Task<IEnumerable<TarjetasDTO>> TarjetasAgencia(int idAgencia);
        Task<TarjetasDTO> GetTarjetaPorId(int idTarjeta);
        Task AceptarTarjeta(int idTarjeta);
        Task AceptarTodasTarjeta(int idAgencia);
        Task<string> EditarEstadoGestion(int idEstado, string idGestion, string user);
        Task<string> EditarEstadoTarjeta(int idEstado, int idTarjeta);
        Task<int> ObtenerEstadoGestion(string idGestion);
        Task<IEnumerable<GetGestiones>> GetGestionesAgencia(int idAgencia);
        Task ActualizarDatosClienteAS400(ActualizarDatosClientePELDTO actualizarDatosClientePELDTO);
        Task RollBackTarjeta(int idTarjeta);
        Task<IEnumerable<ResponseMovimientosTarjetaDTO>> ConsultaMovimientos(RequestMovimientosTarjetaDTO requestMovimientosTarjetaDTO);
        Task<IEnumerable<ResponseLiquidacionesDTO>> Liquidaciones(RequestLiquidacionesDTO requestLiquidacionesDTO);
        Task<string> ObtenerCorreoSupervisor(int IdAgencia);
        Task<string> ObtenerCorreoOficial(int IdGestion);
        Task<DatosClientesPELDTO> ConsultarDatosClienteAS400(string CIF);
        Task<ObtenerUsuarioPELDTO> ConsultarDatosClienteSQL(string User);
        Task<IEnumerable<LimitesGeneralesDTO>> LimitesGenerales(string Categoria = "NORMAL");
        Task<GetGestiones> GetGestionesId(string idGestion);
        Task DenegarGestion(string idGestion);
        Task RollBackTarjetaAsignadaAgencia(int idTarjeta);
        Task<IEnumerable<LimitesGeneralesDTO>> ObtenerListaLimites();
        Task<bool> EditarLimites(LimitesGeneralesDTO limitesGeneralesDTO);
        Task<bool> CrearLimites(LimitesGeneralesDTO limitesGeneralesDTO);

    }
}
