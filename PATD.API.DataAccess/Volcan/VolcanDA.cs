using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BancoPopular.Infraestructura.Docker;
using Newtonsoft.Json;
using PATD.API.DataAccess.LogDA;
using PATD.API.Transversal.Gestiones;
using PATD.API.Transversal.Volcan;

namespace PATD.API.DataAccess.Volcan
{
    public class VolcanDA : IVolcanDA
    {
        private readonly ILogDA _logDA;
        bool _Error;
        StringContent _Request;
        string _Response;
        public VolcanDA(ILogDA logDA)
        {
            _logDA = logDA;
        }
        public async Task<ResponseActualizarDatosClienteDTO> ActualizarDatosCliente(RequestActualizarDatosClienteDTO actualizarDatosClienteDTO, string idGestion, string user)
        {
            try
            {
                var login = await Login();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {login.Token}");
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));

                //var rq = JsonConvert.SerializeObject(actualizarDatosClienteDTO, Formatting.Indented);
                _Request = new StringContent(JsonConvert.SerializeObject(actualizarDatosClienteDTO, Formatting.Indented), Encoding.UTF8, "application/json");

                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/ActualizarDatosClienteOnbV2", _Request);

                _Response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseActualizarDatosClienteDTO>(_Response);
            }
            catch (Exception e)
            {
                _Error = true;
                return new ResponseActualizarDatosClienteDTO
                {
                    ResponseCode = 500,
                    ResponseDescription = e.Message
                };
            }
            finally { await _logDA.RegistroLogVolcan(idGestion, await StringContentToString(_Request), _Response, user, _Error); }
        }
        public async Task<ResponseReasignarTarjetaDTO> ReasignarTarjeta(RequestReasignarTarjetaDTO requestReasignarTarjetaDTO, string idGestion, string user)
        {
            try
            {
                var login = await Login();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {login.Token}");
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));
                // var rq = JsonConvert.SerializeObject(requestReasignarTarjetaDTO, Formatting.Indented);
                _Request = new StringContent(JsonConvert.SerializeObject(requestReasignarTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/ReasignarTarjetaV2", _Request);
                _Response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseReasignarTarjetaDTO>(_Response);
            }
            catch (Exception e)
            {
                _Error = true;
                return new ResponseReasignarTarjetaDTO
                {
                    PreviusAccessEntry = $"Error",
                    NewAccessEntry = e.Message
                };
            }
            finally { await _logDA.RegistroLogVolcan(idGestion, await StringContentToString(_Request), _Response, user, _Error); }
        }
        public async Task<ResponseActivarTarjetaDTO> ActivarTarjeta(RequestActivarTarjetaDTO requestActivarTarjetaDTO, string idGestion, string user)
        {
            try
            {
                var login = await Login();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {login.Token}");
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));
                //var rq = JsonConvert.SerializeObject(requestActivarTarjetaDTO, Formatting.Indented);
                _Request = new StringContent(JsonConvert.SerializeObject(requestActivarTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");

                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/ActivarTarjeta", _Request);

                _Response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseActivarTarjetaDTO>(_Response);
            }
            catch (Exception e)
            {
                _Error = true;
                throw;
            }
            finally { await _logDA.RegistroLogVolcan(idGestion, await StringContentToString(_Request), _Response, user, _Error); }
        }
        public async Task<ResponseBloquearTarjetaDTO> BloquearTarjeta(RequestBloquearTarjetaDTO requestBloquearTarjetaDTO, string idGestion, string user)
        {
            try
            {
                var login = await Login();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {login.Token}");
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));

                //var rq = JsonConvert.SerializeObject(requestBloquearTarjetaDTO, Formatting.Indented);
                _Request = new StringContent(JsonConvert.SerializeObject(requestBloquearTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");

                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/BloquearTarjetaV2", _Request);

                _Response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseBloquearTarjetaDTO>(_Response);
            }
            catch (Exception e)
            {
                _Error = true;
                return new ResponseBloquearTarjetaDTO
                {
                    CodRespuesta = 500,
                    DescRespuesta = e.Message
                };
            }
            finally { await _logDA.RegistroLogVolcan(idGestion, await StringContentToString(_Request), _Response, user, _Error); }
        }
        public async Task<ResponseDesbloquearTarjetaDTO> DesbloquearTarjeta(RequestDesbloquearTarjetaDTO requestDesbloquearTarjetaDTO, string idGestion, string user)
        {
            try
            {
                var login = await Login();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {login.Token}");
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));
                //var rq = JsonConvert.SerializeObject(requestDesbloquearTarjetaDTO, Formatting.Indented);
                _Request = new StringContent(JsonConvert.SerializeObject(requestDesbloquearTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");

                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/DesbloquearTarjetaV2", _Request);

                _Response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseDesbloquearTarjetaDTO>(_Response);
            }
            catch (Exception e)
            {
                _Error = true;
                return new ResponseDesbloquearTarjetaDTO
                {
                    CodRespuesta = 500,
                    DescRespuesta = e.Message
                };
            }
            finally { await _logDA.RegistroLogVolcan(idGestion, await StringContentToString(_Request), _Response, user, _Error); }
        }
        public async Task<ResponseConsultarTarjetaDTO> ConsultarTarjeta(RequestConsultarTarjetaDTO requestConsultarTarjetaDTO, string idGestion, string user)
        {
            try
            {
                var login = await Login();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {login.Token}");
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));
                //var rq = JsonConvert.SerializeObject(requestConsultarTarjetaDTO, Formatting.Indented);
                _Request = new StringContent(JsonConvert.SerializeObject(requestConsultarTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");

                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/ConsultarTarjetaV2", _Request);

                _Response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseConsultarTarjetaDTO>(_Response);
            }
            catch (Exception e)
            {
                _Error = true;
                throw;
            }
            finally { await _logDA.RegistroLogVolcan(idGestion, await StringContentToString(_Request), _Response, user, _Error); }
        }
        public async Task<ResponseCancelarTarjetaDTO> CancelarTarjeta(RequestCancelarTarjetaDTO requestCancelarTarjetaDTO, string idGestion, string user)
        {
            try
            {
                var login = await Login();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {login.Token}");
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));
                //var rq = JsonConvert.SerializeObject(requestCancelarTarjetaDTO, Formatting.Indented);
                _Request = new StringContent(JsonConvert.SerializeObject(requestCancelarTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");

                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/CancelarTarjeta", _Request);

                _Response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseCancelarTarjetaDTO>(_Response);
            }
            catch (Exception e)
            {
                _Error = true;
                return new ResponseCancelarTarjetaDTO
                {
                    CodRespuesta = 500,
                    DescRespuesta = e.Message
                };
            }
            finally { await _logDA.RegistroLogVolcan(idGestion, await StringContentToString(_Request), _Response, user, _Error); }
        }
        private async Task<ResponseLoginDTO> Login()
        {
            try
            {
                var requestLoginDTO = new RequestLoginDTO
                {
                    NombreUsuario = Environment.GetEnvironmentVariable("UsuarioVolcan"),
                    Password = Environment.GetEnvironmentVariable("PasswordVolcan")
                };
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));

                StringContent request = new StringContent(JsonConvert.SerializeObject(requestLoginDTO, Formatting.Indented), Encoding.UTF8, "application/json");

                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/Login", request);

                var response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseLoginDTO>(response);
            }
            catch (Exception e)
            {
                _Error = true;
                throw;
            }
        }
        public async Task<ResponseAsignarPinDTO> AsignarPin(RequestAsignarPinDTO requestAsignarPinDTO, string idGestion, string user)
        {
            try
            {
                var login = await Login();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", login.Token);
                httpClient.DefaultRequestHeaders.Add("Credenciales", Environment.GetEnvironmentVariable("CredencialVolcan"));

                _Request = new StringContent(JsonConvert.SerializeObject(requestAsignarPinDTO, Formatting.Indented), Encoding.UTF8, "application/json");

                var responseJson = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("URLBaseVolcan")}/wsParabiliumVolcan/api/AsignarNIP", _Request);

                _Response = (await responseJson.Content.ReadAsStringAsync() == null) ? "" : await responseJson.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseAsignarPinDTO>(_Response);
            }
            catch (Exception e)
            {
                _Error = true;
                return new ResponseAsignarPinDTO
                {
                    CodRespuesta = 500,
                    DescRespuesta = e.Message
                };
            }
            finally { await _logDA.RegistroLogVolcan(idGestion, await StringContentToString(_Request), _Response, user, _Error); }
        }
        private async Task<string> StringContentToString(StringContent stringContent) 
            => await stringContent.ReadAsStringAsync() is null ? "": await stringContent.ReadAsStringAsync();
        
    }
}

