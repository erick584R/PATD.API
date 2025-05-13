using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Helper
{
	public interface IHelper
	{
		Task EnviaEmail(List<string> Emails, string Cuerpo, string Asunto);
	}
}
