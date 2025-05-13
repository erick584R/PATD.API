using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BancoPopular.Infraestructura.Docker;
using BancoPopular.Respuestas.Respuesta;
using Dapper;
using Newtonsoft.Json;
using PATD.API.DataAccess.Gestiones;
using PATD.API.DataAccess.LogDA;
using PATD.API.DataAccess.Seguridad;
using PATD.API.Transversal.AdminTarjeta;
using StackExchange.Redis;
using RequestAsignarTarjetaDTO = PATD.API.Transversal.AdminTarjeta.RequestAsignarTarjetaDTO;

namespace PATD.API.Application.AdminTarjeta
{
    public class AdminTarjeta : IAdminTarjeta
    {
        private IDatabase _DB;
        private readonly IGestionesDA _gestionesDA;
        private readonly IInfraestructuraDocker _infraestructura;
        private readonly ISeguridadDA _seguridadDA;
        private readonly ILogDA _logDA;
        StringContent _Request;
        bool _Error;
        string _Response;

        public AdminTarjeta(IInfraestructuraDocker infraestructura, ISeguridadDA seguridadDA, ILogDA logDA, IGestionesDA gestionesDA)
        {
            _infraestructura = infraestructura;
            _seguridadDA = seguridadDA;
            _logDA = logDA;
            _gestionesDA = gestionesDA;
            _infraestructura.IniciarRedis();
            _DB = _infraestructura.Connection.GetDatabase();
        }
        #region Integraciones Apis
        public async Task<RespuestaMicroservicio<bool>> CambiarPin(RequestCambioPinDTO requestCambioPinDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var _ApiNewPin = new ApiNewPin
                {
                    Sender = "Sender",
                    PinBlock = GeneratePinBlock(requestCambioPinDTO.Pin, requestCambioPinDTO.NumTarjeta),
                    Pan = requestCambioPinDTO.NumTarjeta
                };
                _Request = new StringContent(JsonConvert.SerializeObject(requestCambioPinDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiNewPin);
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/NewPin", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Cambio de pin exitoso";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Cambio de pin fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Cambio de pin fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Cambio de pin fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;

            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("PIN", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> RelacionarCuentaTarjeta(RequestRelacionarCuentaTarjetaDTO requestRelacionarCuentaTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var _ApilinkAccount = new ApilinkAccount
                {
                    Sender = "Sender",
                    Pan = requestRelacionarCuentaTarjetaDTO.NumTarjeta,
                    AccountNo = requestRelacionarCuentaTarjetaDTO.Cuenta
                };
                _Request = new StringContent(JsonConvert.SerializeObject(requestRelacionarCuentaTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApilinkAccount);
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/RelacionarCuenta", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Relación exitosa";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Relación fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Relación fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Relación fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;

            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("adCC", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<ResponseCrearTarjetasDTO>> CrearTarjetas(IEnumerable<RequestCrearTarjetasDTO> requestCrearTarjetasDTO)
        {
            var respuesta = new RespuestaMicroservicio<ResponseCrearTarjetasDTO>();
            try
            {
                var _ApiCreateCard = new List<ApiCreateCard>();

                foreach (var item in requestCrearTarjetasDTO)
                {
                    var CreateCard = new ApiCreateCard();
                    CreateCard.Sender = "Sender";
                    CreateCard.Pan = item.NumTarjeta;
                    CreateCard.ExpirationDate = item.FechaVencimiento;
                    _ApiCreateCard.Add(CreateCard);
                }
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiCreateCard);
                _Request = new StringContent(JsonConvert.SerializeObject(requestCrearTarjetasDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/CrearTarjetas", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00" || _ApiResponse.CodigoRespuesta is "99")
                        {
                            var getDecrypted = DecryptedJson(_ApiResponse.Datos, _RequestEncrytedApiDTO.Key, _RequestEncrytedApiDTO.Iv);
                            getDecrypted = getDecrypted.Replace("'Created'", "\"Created\"");
                            getDecrypted = getDecrypted.Replace("'NotCreated'", "\"NotCreated\"");
                            getDecrypted = getDecrypted.Replace("'[", "[");
                            getDecrypted = getDecrypted.Replace("]'", "]");
                            respuesta.Entidad = JsonConvert.DeserializeObject<ResponseCrearTarjetasDTO>(getDecrypted);
                            respuesta.Mensaje = "Exito";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Mensaje = "Creacion fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Mensaje = "Creacion fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Mensaje = "Creacion fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;

            }
            catch (Exception e)
            {
                _Error = false;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("Creartj", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<ResponseAsignarTarjetaDTO>> AsignarTarjeta(RequestAsignarTarjetaDTO requestAsignarTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<ResponseAsignarTarjetaDTO>();
            try
            {
                var _ApiAssignCard = new ApiAssignCard
                {
                    Sender = "Sender",
                    Cif = requestAsignarTarjetaDTO.Cif,
                    Agencia = Convert.ToInt16(requestAsignarTarjetaDTO.Agencia)
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiAssignCard);
                _Request = new StringContent(JsonConvert.SerializeObject(requestAsignarTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/AsignarTarjeta", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            var getDecrypted = DecryptedJson(_ApiResponse.Datos, _RequestEncrytedApiDTO.Key, _RequestEncrytedApiDTO.Iv);
                            respuesta.Entidad = JsonConvert.DeserializeObject<ResponseAsignarTarjetaDTO>(getDecrypted);
                            respuesta.Mensaje = "Asignación exitosa";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Mensaje = "Asignación fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Mensaje = "Asignación fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Mensaje = "Asignación fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("asignarT", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> ActivarTarjeta(RequestActivarTarjetaDTO requestActivarTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var _ApiActivateCard = new ApiActivateCard
                {
                    Sender = "Sender",
                    Pan = requestActivarTarjetaDTO.NumTarjeta
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiActivateCard);
                _Request = new StringContent(JsonConvert.SerializeObject(requestActivarTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/Activartarjeta", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Activacion exitosa";
                            respuesta.Codigo = HttpStatusCode.OK;
                            await _ActualizarEstadoTarjeta(requestActivarTarjetaDTO.NumTarjeta?.Trim(), 2);
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Activacion fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Activacion fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Activacion fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("activarT", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> CancelarTarjeta(RequestCancelarTarjetaDTO requestCancelarTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var _ApiCancelCard = new ApiCancelCard
                {
                    Sender = "Sender",
                    Pan = requestCancelarTarjetaDTO.NumTarjeta,
                    Razon = string.IsNullOrEmpty(requestCancelarTarjetaDTO.CodMotivo) ? "03" : requestCancelarTarjetaDTO.CodMotivo
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiCancelCard);
                _Request = new StringContent(JsonConvert.SerializeObject(requestCancelarTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/CancelarTarjeta", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Cancelacion exitoso";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Cancelacion fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Cancelacion fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Cancelacion fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("CancelarT", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> BloquearTarjeta(RequestBloquearTarjetaDTO requestBloquearTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var _ApiBlockCard = new ApiBlockCard
                {
                    Sender = "Sender",
                    Pan = requestBloquearTarjetaDTO.NumTarjeta,
                    Razon = requestBloquearTarjetaDTO.Extravio ? "007" : "006"
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiBlockCard);
                _Request = new StringContent(JsonConvert.SerializeObject(requestBloquearTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/BloquearTarjeta", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Bloqueo exitoso";
                            respuesta.Codigo = HttpStatusCode.OK;
                            await _ActualizarEstadoTarjeta(requestBloquearTarjetaDTO.NumTarjeta?.Trim(), 4);
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Bloqueo fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Bloqueo fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Bloqueo fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("bloquearT", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> DesbloquearTarjeta(RequestDesbloquearTarjetaDTO requestDesbloquearTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var _ApiUnblockCard = new ApiUnblockCard
                {
                    Sender = "Sender",
                    Pan = requestDesbloquearTarjetaDTO.NumTarjeta
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiUnblockCard);
                _Request = new StringContent(JsonConvert.SerializeObject(requestDesbloquearTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/DesbloquearTarjeta", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Desbloqueo exitoso";
                            respuesta.Codigo = HttpStatusCode.OK;
                            await _ActualizarEstadoTarjeta(requestDesbloquearTarjetaDTO.NumTarjeta?.Trim(), 2);
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Desbloqueo fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Desbloqueo fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Desbloqueo fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("desblock", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> AutorizarPais(RequestAutorizarPaisDTO requestAutorizarPaisDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var _ApiCountryAuthorization = new ApiCountryAuthorization
                {
                    Sender = "Sender",
                    Pan = requestAutorizarPaisDTO.NumTarjeta,
                    CodigoIsoNumerico = (short)requestAutorizarPaisDTO.CodigoPais,
                    //FechaDesde = DateOnly.Parse(requestAutorizarPaisDTO.FechaDesde.ToString("yyyy-MM-dd")),
                    //FechaHasta = DateOnly.Parse(requestAutorizarPaisDTO.FechaHasta.ToString("yyyy-MM-dd"))
                    FechaDesde = requestAutorizarPaisDTO.FechaDesde.ToString("yyyy-MM-dd"),
                    FechaHasta = requestAutorizarPaisDTO.FechaHasta.ToString("yyyy-MM-dd")
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiCountryAuthorization);
                _Request = new StringContent(JsonConvert.SerializeObject(requestAutorizarPaisDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/AutorizarPais", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Autorización exitosa";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Autorización fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Autorización fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Autorización fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("authPais", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> LimitesTarjeta(RequestLimitesTarjetaDTO requestLimitesTarjetaDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                if (requestLimitesTarjetaDTO.LimiteRetiroNumeroDiario < 1 || requestLimitesTarjetaDTO.LimiteRetiroMontoDiario < 1 || requestLimitesTarjetaDTO.LimiteRetiroMontoTxn < 1 || requestLimitesTarjetaDTO.LimiteCompraNumeroDiario < 1 || requestLimitesTarjetaDTO.LimiteCompraMontoDiario < 1 || requestLimitesTarjetaDTO.LimiteCompraMontoTxn < 1)
                {
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Autorización fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                if (!requestLimitesTarjetaDTO.PATD)
                {
                    var categoriaMaxima = "MAXIMOPEL";
                    var obtenerLimMax = await _gestionesDA.LimitesGenerales(categoriaMaxima);
                    if (obtenerLimMax.Count() is 0)
                    {
                        categoriaMaxima = "NORMAL";
                        obtenerLimMax = await _gestionesDA.LimitesGenerales(categoriaMaxima);
                    }
                    LimitesGeneralesDTO limitesGeneralesDTO = new LimitesGeneralesDTO();
                    limitesGeneralesDTO = obtenerLimMax.Where(x => x.Categoria == categoriaMaxima).First();
                    if(requestLimitesTarjetaDTO.LimiteRetiroNumeroDiario > limitesGeneralesDTO.LimiteRetiroNumeroDiario || requestLimitesTarjetaDTO.LimiteRetiroMontoDiario > limitesGeneralesDTO.LimiteRetiroMontoDiario || requestLimitesTarjetaDTO.LimiteRetiroMontoTxn > limitesGeneralesDTO.LimiteRetiroMontoTxn || requestLimitesTarjetaDTO.LimiteCompraNumeroDiario > limitesGeneralesDTO.LimiteCompraNumeroDiario || requestLimitesTarjetaDTO.LimiteCompraMontoDiario > limitesGeneralesDTO.LimiteCompraMontoDiario || requestLimitesTarjetaDTO.LimiteCompraMontoTxn > limitesGeneralesDTO.LimiteCompraMontoTxn)
                    {
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Autorización fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                }
                var _ApiChangeLimits = new ApiChangeLimits
                {
                    Sender = "Sender",
                    Pan = requestLimitesTarjetaDTO.NumTarjeta,
                    LimiteRetiroActivo = requestLimitesTarjetaDTO.LimiteRetiroActivo,
                    LimiteRetiroNumeroDiario = requestLimitesTarjetaDTO.LimiteRetiroNumeroDiario,
                    LimiteRetiroMontoDiario = requestLimitesTarjetaDTO.LimiteRetiroMontoDiario,
                    LimiteRetiroMontoTxn = requestLimitesTarjetaDTO.LimiteRetiroMontoTxn,
                    LimiteCompraActivo = requestLimitesTarjetaDTO.LimiteCompraActivo,
                    LimiteCompraNumeroDiario = requestLimitesTarjetaDTO.LimiteCompraNumeroDiario,
                    LimiteCompraMontoDiario = requestLimitesTarjetaDTO.LimiteCompraMontoDiario,
                    LimiteCompraMontoTxn = requestLimitesTarjetaDTO.LimiteCompraMontoTxn
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiChangeLimits);
                _Request = new StringContent(JsonConvert.SerializeObject(requestLimitesTarjetaDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/ModificarLimites", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Autorización exitosa";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Autorización fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Autorización fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Autorización fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("limits", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<IEnumerable<ResponseConsultaClienteDTO>>> ConsultaCliente(RequestConsultaClienteDTO requestConsultaClienteDTO)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<ResponseConsultaClienteDTO>>();
            try
            {
                var _ApiCardInquiry = new ApiCardInquiry
                {
                    Sender = "Sender",
                    Cif = requestConsultaClienteDTO.Cif
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiCardInquiry);
                _Request = new StringContent(JsonConvert.SerializeObject(requestConsultaClienteDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/ConsultaCif", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            var getDecrypted = DecryptedJson(_ApiResponse.Datos, _RequestEncrytedApiDTO.Key, _RequestEncrytedApiDTO.Iv);


                            var ListaConsulta = new List<ResponseConsultaClienteDTO>();
                            ListaConsulta = JsonConvert.DeserializeObject<List<ResponseConsultaClienteDTO>>(getDecrypted);
                            var ListaResponse = new List<ResponseConsultaClienteDTO>();
                            if (ListaConsulta.Count() > 0)
                            {
                                foreach (var item in ListaConsulta)
                                {
                                    if(item.Status == "A" || item.Status == "B")
                                    {
                                        var cuenta = await _ObtenerCuenta_Tarjeta(item.Pan);
                                        item.Cuenta = cuenta;
                                        item.Name = item.Name.Replace("NoAplica", string.Empty).Trim();
                                        ListaResponse.Add(item);
                                    }
                                }
                            }
                            respuesta.Entidad = ListaResponse;
                            respuesta.Mensaje = "Consulta exitosa";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Mensaje = "Consulta fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Mensaje = "Consulta fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Mensaje = "Consulta fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("ConsCli", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<bool>> DistribuirTarjetas(RequestDistribuirTarjetasDTO requestDistribuirTarjetasDTO)
        {
            var respuesta = new RespuestaMicroservicio<bool>();
            try
            {
                var _ApiCardDistribution = new ApiCardDistribution
                {
                    Sender = "Sender",
                    Pan = requestDistribuirTarjetasDTO.NumTarjeta,
                    Agencia = Convert.ToInt16(requestDistribuirTarjetasDTO.Agencia)
                };
                var _RequestEncrytedApiDTO = await EncryptedJson(_ApiCardDistribution);
                _Request = new StringContent(JsonConvert.SerializeObject(requestDistribuirTarjetasDTO, Formatting.Indented), Encoding.UTF8, "application/json");
                ApiRequest _Encrypted = new()
                {
                    EncryptedKey = _RequestEncrytedApiDTO.EncryptedKey,
                    EncryptedIv = _RequestEncrytedApiDTO.EncryptedIv,
                    EncryptedMessage = _RequestEncrytedApiDTO.EncryptedMessage
                };
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using HttpClient client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var _content = new StringContent(JsonConvert.SerializeObject(_Encrypted), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/DistribuirTarjeta", _content);
                _Response = (await response.Content.ReadAsStringAsync() == null) ? "" : await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    if (_ApiResponse != null)
                    {
                        if (_ApiResponse.CodigoRespuesta is "00")
                        {
                            respuesta.Entidad = true;
                            respuesta.Mensaje = "Distribucion exitosa";
                            respuesta.Codigo = HttpStatusCode.OK;
                            return respuesta;
                        }
                        _Error = true;
                        respuesta.Entidad = false;
                        respuesta.Mensaje = "Distribucion fallo";
                        respuesta.Codigo = HttpStatusCode.BadRequest;
                        return respuesta;
                    }
                    _Error = true;
                    respuesta.Entidad = false;
                    respuesta.Mensaje = "Distribucion fallo";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                _Error = true;
                respuesta.Entidad = false;
                respuesta.Mensaje = "Distribucion fallo";
                respuesta.Codigo = HttpStatusCode.BadRequest;
                return respuesta;
            }
            catch (Exception e)
            {
                _Error = true;
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
            finally { await _logDA.RegistroLogVolcan("distribuir", await StringContentToString(_Request), _Response, "admin", _Error); }
        }
        public async Task<RespuestaMicroservicio<IEnumerable<CPaisesIsoDTO>>> ObtenerPaisesIso()
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<CPaisesIsoDTO>>();
            try
            {
                var obtener = await _seguridadDA.ObtenerPaisesIso();
                if (obtener.Count() is 0)
                {
                    respuesta.Mensaje = "Error al consultar paises";
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    return respuesta;
                }
                respuesta.Entidad = obtener;
                respuesta.Mensaje = "Se consulto con exito";
                respuesta.Codigo = HttpStatusCode.OK;
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }

        }
        private async Task<string> _ObtenerCuenta_Tarjeta(string NumTarjeta)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@NumTarjeta", NumTarjeta);

            return await _infraestructura.ObtenerSQL<string>("SP_OBTENER_CUENTA_TARJETA", parametros);
        }
        #endregion
        #region Helpers
        private string GeneratePinBlock(string pin, string cardNumber)
        {
            if (pin.Length < 4 || pin.Length > 6)
            {
                throw new ArgumentException("PIN length must be between 4 and 6 characters.");
            }
            string pinBlock = $"0{pin.Length}{pin}";
            while (pinBlock.Length != 16)
            {
                pinBlock += "F";
            }
            int cardLen = cardNumber.Length;
            string pan = "0000" + cardNumber.Substring(cardLen - 13, 12);
            return XorHex(pinBlock, pan);
        }
        private string XorHex(string a, string b)
        {
            char[] chars = new char[a.Length];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = ToHex(FromHex(a[i]) ^ FromHex(b[i]));
            }
            return new string(chars).ToUpper();
        }
        private char ToHex(int nybble)
        {
            if (nybble < 0 || nybble > 15)
            {
                throw new ArgumentException();
            }
            return "0123456789ABCDEF"[nybble];
        }
        private int FromHex(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }
            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }
            throw new ArgumentException();
        }
        private static Task<RequestEncrytedApiDTO> EncryptedJson<T>(T dto)
        {
            byte[] fileBytes = File.ReadAllBytes("C:\\temp\\publickeyinfo.bin");
            using RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(fileBytes, out _);
            using TripleDES tripleDES = TripleDES.Create();
            tripleDES.GenerateKey();
            tripleDES.GenerateIV();
            tripleDES.Mode = CipherMode.CBC;
            tripleDES.Padding = PaddingMode.PKCS7;
            byte[] encryptedKey = rsa.Encrypt(tripleDES.Key, RSAEncryptionPadding.Pkcs1);
            byte[] encryptedIv = rsa.Encrypt(tripleDES.IV, RSAEncryptionPadding.Pkcs1);
            string json = JsonConvert.SerializeObject(dto);
            byte[] message = Encoding.UTF8.GetBytes(json);
            byte[] encryptedMessage;
            using (ICryptoTransform encryptor = tripleDES.CreateEncryptor())
            {
                encryptedMessage = encryptor.TransformFinalBlock(message, 0, message.Length);
            }
            RequestEncrytedApiDTO _RequestEncrytedApiDTO = new()
            {
                EncryptedKey = Convert.ToBase64String(encryptedKey),
                EncryptedIv = Convert.ToBase64String(encryptedIv),
                EncryptedMessage = Convert.ToBase64String(encryptedMessage),
                Key = tripleDES.Key,
                Iv = tripleDES.IV
            };

            return Task.FromResult(_RequestEncrytedApiDTO);
        }
        private string DecryptedJson(string datos, byte[] key, byte[] iv)
        {
            byte[] message = Convert.FromBase64String(datos);
            using TripleDES tripleDES = TripleDES.Create();
            tripleDES.Key = key;
            tripleDES.IV = iv;
            byte[] decryptedMessage;
            using (ICryptoTransform decryptor = tripleDES.CreateDecryptor())
            {
                decryptedMessage = decryptor.TransformFinalBlock(message, 0, message.Length);
            }
            return Encoding.UTF8.GetString(decryptedMessage);
        }
        private async Task<string> StringContentToString(StringContent stringContent)
            => await stringContent.ReadAsStringAsync() is null ? "" : await stringContent.ReadAsStringAsync();
        #endregion
        #region Manejo de Sesión apis
        public async Task<string> GetToken()
        {
            var obtenerToken = await _DB.StringGetAsync("TokenApiRB".ToString());
            if (obtenerToken.ToString() is not null)
            {
                return obtenerToken.ToString();
            }
            //var obtenerTokenRefresh = await _DB.StringGetAsync("TokenApiRefreshRB".ToString());
            //if (obtenerTokenRefresh.ToString() is not null)
            //{
            //    return obtenerTokenRefresh.ToString();
            //}
            loginResponse? loginResponse = await login("api@bancopopular.hn", "Api1234!");
            if (loginResponse is null)
            {
                return "";
            }
            await _DB.StringSetAsync("TokenApiRB", loginResponse.accessToken, TimeSpan.FromSeconds(loginResponse.expiresIn));
            //await _DB.StringSetAsync("TokenApiRefreshRB", loginResponse.refreshToken, TimeSpan.FromSeconds(loginResponse.expiresIn + 864000));
            return loginResponse.accessToken;
        }
        private async Task<loginResponse?> login(string email, string password)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using HttpClient httpClient = new HttpClient(clientHandler);
            var loginRequest = new
            {
                Email = email,
                Password = password
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            var rc = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("UrlBaseInterfazRB")}/Api/identity/login", content);
            var response = rc;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var loginResponse = JsonConvert.DeserializeObject<loginResponse>(responseContent);
                if (loginResponse != null)
                {
                    return loginResponse;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion


        public async Task<RespuestaMicroservicio<IEnumerable<ResponseTrxTdDto>>> RequestTrxTd(RequestTrxTdDto requestTrxTdDto)
        {
            var respuesta = new RespuestaMicroservicio<IEnumerable<ResponseTrxTdDto>>();
            try
            {
                var requestInfoCliente = await _gestionesDA.InformacionCliente(requestTrxTdDto.IdentidadCliente);
                if (requestInfoCliente is null)
                {
                    _Error = true;
                    respuesta.Codigo = HttpStatusCode.BadRequest;
                    respuesta.Mensaje = $"Error al intentar consulta información del cliente";
                    return respuesta;
                }
                var transaccines = new List<ResponseTrxTdDto>();
                var requestCuentas = await _gestionesDA.ObtenerCuentas(requestInfoCliente.Cif);
                if (requestCuentas.Count() > 0)
                {
                    foreach (var item in requestCuentas)
                    {
                        var obtener = await _obtenerTrxTd(item.cuenta.Replace("-", string.Empty).Trim());
                        if (obtener.Count() > 0)
                        {
                            foreach (var item2 in obtener)
                            {
                                item2.Cuenta = item2.Cuenta.Replace(" ", string.Empty).Trim();
                                item2.Cuenta = item2.Cuenta.Replace("-", string.Empty).Trim();
                                item2.Cuenta = Regex.Replace(item2.Cuenta.Substring(item2.Cuenta.Length - 4, 4), @"\D", "");

                                if (requestTrxTdDto.UltimosDigitosTarjeta.Contains(item2.Cuenta) || requestTrxTdDto.UltimosDigitosTarjeta.Substring(1, 3).Contains(item2.Cuenta))
                                {
                                    item2.Cuenta = item.cuenta.Replace("-", string.Empty).Trim();
                                    transaccines.Add(item2);
                                }
                            }
                        }
                    };
                }
                respuesta.Entidad = transaccines;
                respuesta.Mensaje = "Se consulto con exito";
                respuesta.Codigo = HttpStatusCode.OK;
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta.Mensaje = e.Message;
                respuesta.Codigo = HttpStatusCode.InternalServerError;
                return respuesta;
            }
        }
        private async Task<IEnumerable<ResponseTrxTdDto>> _obtenerTrxTd(string Cuenta)
        {
            var parametros = new DynamicParameters();

            parametros.Add("ICHCUENTA", Cuenta, dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("OINTCODIGOERROR", 0, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parametros.Add("OVARMENSAJEERROR", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            var obtener = await _infraestructura.ObtenerListaDB2<ResponseTrxTdDto>("SP_SE_SEL_TRX_TARJETA_DEBITO", parametros);
            return obtener.Values.AsList();
        }
        private async Task<string> _ActualizarEstadoTarjeta(string tarjeta, int estado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@numTarjeta", tarjeta);
            parametros.Add("@Estado", estado);
            return await _infraestructura.ObtenerSQL<string>("SP_ACTUALIZAR_ESTADO_TARJETA_NUM_TARJETA", parametros);
        }
    }
}
