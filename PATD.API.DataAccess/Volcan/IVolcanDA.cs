using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PATD.API.Transversal.Volcan;

namespace PATD.API.DataAccess.Volcan
{
    public interface IVolcanDA
    {
        Task<ResponseActualizarDatosClienteDTO> ActualizarDatosCliente(RequestActualizarDatosClienteDTO actualizarDatosClienteDTO, string idGestion, string user);
        Task<ResponseReasignarTarjetaDTO> ReasignarTarjeta(RequestReasignarTarjetaDTO requestReasignarTarjetaDTO, string idGestion, string user);
        Task<ResponseActivarTarjetaDTO> ActivarTarjeta(RequestActivarTarjetaDTO requestActivarTarjetaDTO, string idGestion, string user);
        Task<ResponseBloquearTarjetaDTO> BloquearTarjeta(RequestBloquearTarjetaDTO requestBloquearTarjetaDTO, string idGestion, string user);
        Task<ResponseDesbloquearTarjetaDTO> DesbloquearTarjeta(RequestDesbloquearTarjetaDTO requestDesbloquearTarjetaDTO, string idGestion, string user);
        Task<ResponseConsultarTarjetaDTO> ConsultarTarjeta(RequestConsultarTarjetaDTO requestConsultarTarjetaDTO, string idGestion, string user);
        Task<ResponseCancelarTarjetaDTO> CancelarTarjeta(RequestCancelarTarjetaDTO requestCancelarTarjetaDTO, string idGestion, string user);
        Task<ResponseAsignarPinDTO> AsignarPin(RequestAsignarPinDTO requestAsignarPinDTO, string idGestion, string user);
    }
}
