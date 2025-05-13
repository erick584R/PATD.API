using Microsoft.Extensions.Configuration;
using PATD.API.Transversal.Email;
using System.Net;
using System.Net.Mail;

namespace PATD.API.Transversal.Helper
{
    public class Helper : IHelper
	{
		private readonly IConfiguration configuration;

		public Helper(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        public async Task EnviaEmail(List<string> Emails, string Cuerpo, string Asunto)
		{
			EmailCredenciales emailCredenciales = CredencialesEmail();
			SmtpClient smtpClient = new SmtpClient(emailCredenciales.ServerMail, emailCredenciales.Port);
			smtpClient.Credentials = new NetworkCredential(emailCredenciales.Email, emailCredenciales.Contraseña);
			MailMessage mailMessage = new MailMessage();
			mailMessage.Subject = Asunto;
			foreach (string Email in Emails)
			{
				mailMessage.To.Add(Email);
			}

			mailMessage.IsBodyHtml = true;
			mailMessage.Priority = MailPriority.High;
			mailMessage.From = new MailAddress(emailCredenciales.Email, emailCredenciales.Alias);
			mailMessage.Body = Cuerpo;
			smtpClient.EnableSsl = true;
			await smtpClient.SendMailAsync(mailMessage);
		}

		private EmailCredenciales CredencialesEmail()
		{
			IConfigurationSection section = configuration.GetSection("EmailCredenciales");
			return new EmailCredenciales
			{
				Alias = section.GetSection("Alias").Value,
				Email = section.GetSection("Email").Value,
				Contraseña = section.GetSection("Password").Value,
				Port = Convert.ToInt32(section.GetSection("Port").Value),
				ServerMail = section.GetSection("ServerMail").Value
			};
		}
	}
}
