using BancoPopular.Infraestructura.Docker;
using Dapper;
using PATD.API.Transversal.LogsT;

namespace PATD.API.DataAccess.LogDA
{
    public class LogDA : ILogDA
    {
        private readonly IInfraestructuraDocker _infraestructuraDocker;
        public LogDA(IInfraestructuraDocker infraestructuraDocker)
        {
            _infraestructuraDocker = infraestructuraDocker;
        }

        public async Task<ObtenerLogsDTO> ObtenerLogs()
        {
            var logsGestiones = await _infraestructuraDocker.ObtenerListaSQL<ObtenerLogsGestionesDTO>("SP_OBTENER_LOG_GESTIONES");
            var logsTarjeta = await _infraestructuraDocker.ObtenerListaSQL<ObtenerLogsTarjetaDTO>("SP_OBTENER_LOG_TARJETAS");
            var logsVolcan = await _infraestructuraDocker.ObtenerListaSQL<ObtenerLogsVolcanDTO>("SP_OBTENER_LOG_VOLCAN");
            return new ObtenerLogsDTO { LogsGestiones = logsGestiones, LogsTarjeta= logsTarjeta, LogsVolcan = logsVolcan };
        }

        public async Task RegistrLogGestion(string IdGestion, string TipoEvento, string Desc, string Usuario, bool Error)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdGestion", IdGestion);
            parametros.Add("@TipoEvento", TipoEvento);
            parametros.Add("@Desc", Desc);
            parametros.Add("@Usuario", Usuario);
            parametros.Add("@Error", Error);
            await _infraestructuraDocker.EjecutarSQL("SP_REGISTRO_LOG_GESTIONES", parametros);
        }

        public async Task RegistrLogTarjeta(int IdTarjeta, string TipoEvento, string Desc, string Usuario, bool Error)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", IdTarjeta);
            parametros.Add("@TipoEvento", TipoEvento);
            parametros.Add("@Desc", Desc);
            parametros.Add("@Usuario", Usuario);
            parametros.Add("@Error", Error);
            await _infraestructuraDocker.EjecutarSQL("SP_REGISTRO_LOG_TARJETAS", parametros);
        }

        public async Task RegistroLogVolcan(string IdGestion, string Request, string Response, string Usuario, bool Error)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdGestion", IdGestion);
            parametros.Add("@Request", Request);
            parametros.Add("@Response", Response);
            parametros.Add("@Usuario", Usuario);
            parametros.Add("@Error", Error);
            await _infraestructuraDocker.EjecutarSQL("SP_REGISTRO_LOG_VOLCAN", parametros);
        }
    }
}
