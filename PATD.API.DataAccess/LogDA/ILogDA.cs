using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PATD.API.Transversal.LogsT;

namespace PATD.API.DataAccess.LogDA
{
    public interface ILogDA
    {
        Task RegistrLogGestion(string IdGestion, string TipoEvento, string Desc, string Usuario, bool Error);
        Task RegistroLogVolcan(string IdGestion, string Request, string Response, string Usuario, bool Error);
        Task RegistrLogTarjeta(int IdTarjeta, string TipoEvento, string Desc, string Usuario, bool Error);
        Task<ObtenerLogsDTO> ObtenerLogs();
    }
}
