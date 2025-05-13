using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BancoPopular.Infraestructura.Docker;
using Dapper;
using PATD.API.Transversal.AdminTarjeta;
using PATD.API.Transversal.Gestiones;
using PATD.API.Transversal.Notificaciones;

namespace PATD.API.DataAccess.Gestiones
{
    public class GestionesDA : IGestionesDA
    {
        private IInfraestructuraDocker _infraestructuraDocker;
        public GestionesDA(IInfraestructuraDocker infraestructuraDocker)
        {
            _infraestructuraDocker = infraestructuraDocker;
        }

        public async Task<IEnumerable<GetGestiones>> GetGestiones(string user)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Usuario", user);
            return await _infraestructuraDocker.ObtenerListaSQL<GetGestiones>("SP_OBTENER_GESTIONES", parametros);
        }

        public async Task<IEnumerable<GetGestiones>> GetGestionesAgencia(int idAgencia)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdAgencia", idAgencia);
            return await _infraestructuraDocker.ObtenerListaSQL<GetGestiones>("SP_OBTENER_GESTIONES_AGENCIA", parametros);
        }

        public async Task<IEnumerable<TarjetasDTO>> GetTarjetas()
        {
            return await _infraestructuraDocker.ObtenerListaSQL<TarjetasDTO>("SP_OBTENER_TARJETAS");
        }

        public async Task<IEnumerable<TarjetasDTO>> GetTarjetasCif(string cif)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Cif", cif);
            return await _infraestructuraDocker.ObtenerListaSQL<TarjetasDTO>("SP_OBTENER_TARJETAS_CIF", parametros);
        }

        public async Task<IEnumerable<GetTipoGestionesDTO>> GetTipoGestiones()
        {
            return await _infraestructuraDocker.ObtenerListaSQL<GetTipoGestionesDTO>("SP_OBTENER_TIPO_GESTION");
        }

        public async Task<InformacionClienteDTO> InformacionCliente(string identidad)
        {
            var parametros = new DynamicParameters();
            parametros.Add("NUMIDENTIDAD", identidad, dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("OINTCODIGOERROR", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            parametros.Add("OVARMENSAJEERROR", "", dbType: DbType.String, direction: ParameterDirection.InputOutput);

            var request = await _infraestructuraDocker.ObtenerDB2<InformacionClienteDTO>("SP_SEL_INFO_CLIENTE", parametros);
            return request.Values;
        }

        public async Task<IEnumerable<CuentasDTO>> ObtenerCuentas(string cif)
        {
            var param = new DynamicParameters();
            param.Add("ICHCIF", cif, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("OINTCODIGOERROR", 0, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            param.Add("OVARMENSAJEERROR", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            var obtener = await _infraestructuraDocker.ObtenerListaDB2<CuentasDTO>("SP_SE_SEL_OBTENERPRODUCTOS", param);
            return obtener.Values.AsList();
        }

        public async Task<IEnumerable<TarjetasDTO>> RequestAsignarTarjetaAgencia(RequestAsignarTarjetaAgenciaDTO requestAsignarTarjetaAgenciaDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdAgencia", requestAsignarTarjetaAgenciaDTO.IdAgencia);
            parametros.Add("@IdAgenciaEnviar", requestAsignarTarjetaAgenciaDTO.IdAgenciaEnviar);
            parametros.Add("@Cantidad", requestAsignarTarjetaAgenciaDTO.CantidadTarjetas);

            return await _infraestructuraDocker.ObtenerListaSQL<TarjetasDTO>("SP_ASIGNAR_AGENCIA_TARJETAS", parametros);
        }

        public async Task<string> RequestCrearGestion(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var parametros = new DynamicParameters();
            //parametros.Add("@idLote", requestCrearGestionDTO.IdLote);
            parametros.Add("@idTarjeta", requestCrearGestionDTO.IdTarjeta);
            parametros.Add("@idTipoGestion", requestCrearGestionDTO.IdTipoGestion);
            parametros.Add("@idUsuario", requestCrearGestionDTO.Usuario);

            return await _infraestructuraDocker.ObtenerSQL<string>("SP_CREAR_GESTION", parametros);
        }

        public async Task<string> RequestCrearLote()
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Descripcion", $"Creado el: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_CREAR_LOTE", parametros);
        }
        //cambios en tarjetas 
        public async Task<string> RequestCrearTarjeta(RequestCrearTarjetaDTO requestCrearTarjetaDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Emisor", requestCrearTarjetaDTO.Emisor);
            parametros.Add("@NumTarjeta", requestCrearTarjetaDTO.NumTarjeta);
            parametros.Add("@IdTipoTarjeta", requestCrearTarjetaDTO.IdTipoTarjeta);
            parametros.Add("@Vencimiento", requestCrearTarjetaDTO.Vencimiento);
            parametros.Add("@IdLote", requestCrearTarjetaDTO.IdLote);

           return await _infraestructuraDocker.ObtenerSQL<string>("SP_CREAR_TARJETA", parametros);
        }
        public async Task<string> RequestAsignarTarjeta(RequestCrearGestionDTO requestCrearGestionDTO, string tarjeta)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Tarjeta", tarjeta);
            parametros.Add("@numCuenta",requestCrearGestionDTO.NumCuenta);
            parametros.Add("@Alias",requestCrearGestionDTO.Alias);
            parametros.Add("@CIF",requestCrearGestionDTO.Cif);
            parametros.Add("@IdAgencia", requestCrearGestionDTO.IdAgencia);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_ASIGNAR_TARJETA", parametros);
        }
        public async Task<string> RequestReAsignarTarjeta(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@numCuenta", requestCrearGestionDTO.NumCuenta);
            parametros.Add("@Alias", requestCrearGestionDTO.Alias);
            parametros.Add("@CIF", requestCrearGestionDTO.Cif);
            parametros.Add("@IdAgencia", requestCrearGestionDTO.IdAgencia);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_REASIGNAR_TARJETA", parametros);
        }
        public async Task<string> RequestCancelarTarjeta(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", requestCrearGestionDTO.IdTarjeta);
            parametros.Add("@IdMotivo", requestCrearGestionDTO.motivo);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_MOTIVO_CANCELACION", parametros);
        }
        public async Task<string> RequestUpdateAliasTarjeta(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", requestCrearGestionDTO.IdTarjeta);
            parametros.Add("@Alias", requestCrearGestionDTO.Alias);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_MODIFICAR_ALIAS", parametros);
        }
        public async Task<string> RequestBloqueoTarjeta(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", requestCrearGestionDTO.IdTarjeta);
            parametros.Add("@IdMotivo", requestCrearGestionDTO.motivo);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_MOTIVO_BLOQUEO", parametros);
        }
        public async Task<string> RequestDesbloqueoTarjeta(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", requestCrearGestionDTO.IdTarjeta);
            parametros.Add("@Estado", 2);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_ACTUALIZAR_ESTADO_TARJETA", parametros);
        }
        public async Task<string> RequestUpdateCuentaTarjeta(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", requestCrearGestionDTO.IdTarjeta);
            parametros.Add("@numCuenta", requestCrearGestionDTO.NumCuenta);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_ACTUALIZAR_CUENTA_TARJETA", parametros);
        }
        public async Task<string> RequestReposicionTarjeta(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", requestCrearGestionDTO.IdTarjeta);
            parametros.Add("@IdMotivo", requestCrearGestionDTO.motivo);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_RAZON_REPOSICION", parametros);
        }
        public async Task<IEnumerable<TarjetasDisponiblesAgenciasDTO>> TarjetasDisponiblesAgencias()
        {
            return await _infraestructuraDocker.ObtenerListaSQL<TarjetasDisponiblesAgenciasDTO>("SP_OBTENER_TARJETAS_DISPONIBLES_AGENCIAS");
        }

        public async Task<IEnumerable<TarjetasDTO>> TarjetasPorAceptar(int idAgencia)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdAgencia", idAgencia);
            return await _infraestructuraDocker.ObtenerListaSQL<TarjetasDTO>("SP_OBTENER_TARJETAS_POR_ACEPTAR_AGENCIA", parametros);
        }

        public async Task<IEnumerable<TarjetasDTO>> TarjetasAgencia(int idAgencia)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdAgencia", idAgencia);
            return await _infraestructuraDocker.ObtenerListaSQL<TarjetasDTO>("SP_OBTENER_TARJETAS_AGENCIA", parametros);
        }

        public async Task AceptarTarjeta(int idTarjeta)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", idTarjeta);
            await _infraestructuraDocker.EjecutarSQL("SP_ACEPTAR_TARJETA_AGENCIA", parametros);
        }

        public async Task<bool> ValidaGestion(RequestCrearGestionDTO requestCrearGestionDTO)
        {
            if (requestCrearGestionDTO.IdTarjeta == null)
            {
                return false;
            }
            var parametros = new DynamicParameters();
            parametros.Add("@tipoGestion", requestCrearGestionDTO.IdTipoGestion);
            parametros.Add("@idTarjeta", requestCrearGestionDTO.IdTarjeta);
            return (await _infraestructuraDocker.ObtenerSQL<string>("SP_VALIDA_GESTION", parametros) == "Existe" ? true:false);
        }

        public async Task<TarjetasDTO> GetTarjetaPorId(int idTarjeta)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", idTarjeta);
            return await _infraestructuraDocker.ObtenerSQL<TarjetasDTO>("SP_OBTENER_TARJETA_ID", parametros);
        }

        public async Task<string> EditarEstadoGestion(int idEstado, string idGestion, string user)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdGestion", idGestion);
            parametros.Add("@IdUsuario", user);
            parametros.Add("@Estado", idEstado);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_ACTUALIZAR_ESTADO_GESTION", parametros);
        }

        public async Task<int> ObtenerEstadoGestion(string idGestion)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdGestion", idGestion);
            return await _infraestructuraDocker.ObtenerSQL<int>("SP_OBTENER_ESTADO_GESTION_ID", parametros);
        }

        public async Task<string> EditarEstadoTarjeta(int idEstado, int idTarjeta)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdTarjeta", idTarjeta);
            parametros.Add("@Estado", idEstado);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_ACTUALIZAR_ESTADO_TARJETA", parametros);
        }

        public async Task ActualizarDatosClienteAS400(ActualizarDatosClientePELDTO actualizarDatosClientePELDTO)
        {
            var sp = "SP_ACT_DATOSCLIENTES_PEL";
            var parametros = new DynamicParameters();
            parametros.Add("ICHCIF", actualizarDatosClientePELDTO.Cif?.Trim().PadLeft(18), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHESTADO", actualizarDatosClientePELDTO.EstadoCivil?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHDIRECCION", actualizarDatosClientePELDTO.DireccionPersonal?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHBLOQUESECTOR", actualizarDatosClientePELDTO.BloqueSectorPersonal?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHPUNTOREF", actualizarDatosClientePELDTO.PuntoReferenciaPersonal?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHDIRECCIONTRA", actualizarDatosClientePELDTO.DireccionTrabajo?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHBLOQUESECTORTRA", actualizarDatosClientePELDTO.BloqueSectorTrabajo?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHPUNTOREFTRA", actualizarDatosClientePELDTO.PuntoReferenciaTrabajo?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHCELULAR", actualizarDatosClientePELDTO.Celular?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHTELPERSONAL", actualizarDatosClientePELDTO.TelFijoPer?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHTELLABORAL", actualizarDatosClientePELDTO.TelFijoLab?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("ICHCORREO", actualizarDatosClientePELDTO.Correo?.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("OINTCODIGOERROR", 0, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parametros.Add("OVARMENSAJEERROR", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);
            await _infraestructuraDocker.ObtenerParametrosDB2(sp, parametros);
        }

        public async Task RollBackTarjeta(int idTarjeta)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idTarjeta", idTarjeta);
            await _infraestructuraDocker.EjecutarSQL("SP_ROLLBACK_TARJETA", parametros);
        }
        public async Task<IEnumerable<ResponseMovimientosTarjetaDTO>> ConsultaMovimientos(RequestMovimientosTarjetaDTO requestMovimientosTarjetaDTO)
        {
            var param = new DynamicParameters();
            param.Add("CUENTA", requestMovimientosTarjetaDTO.Cuenta, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("OINTCODIGOERROR", 0, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            param.Add("OVARMENSAJEERROR", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            var obtener = await _infraestructuraDocker.ObtenerListaDB2<ResponseMovimientosTarjetaDTO>("SP_SEL_MOV_TARJETA", param);
            return obtener.Values.AsList();
        }
        public async Task<IEnumerable<ResponseLiquidacionesDTO>> Liquidaciones(RequestLiquidacionesDTO requestLiquidacionesDTO)
        {
            var param = new DynamicParameters();
            param.Add("FECHAINICIO", requestLiquidacionesDTO.FechaInicio.ToString("yyyyMMdd"), dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("FECHAFIN", requestLiquidacionesDTO.FechaFin.ToString("yyyyMMdd"), dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("OINTCODIGOERROR", 0, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            param.Add("OVARMENSAJEERROR", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            var obtener = await _infraestructuraDocker.ObtenerListaDB2<ResponseLiquidacionesDTO>("SP_SEL_LIQUIDACION_TD", param);
            return obtener.Values.AsList();
        }

        public async Task<string> ObtenerCorreoSupervisor(int IdAgencia)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdAgencia", IdAgencia);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_OBTENER_CORREO_SUPERVISOR", parametros);
        }

        public async Task<string> ObtenerCorreoOficial(int IdGestion)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdGestion", IdGestion);
            return await _infraestructuraDocker.ObtenerSQL<string>("SP_OBTENER_CORREO_OFICIAL_OP", parametros);
        }
        public async Task<DatosClientesPELDTO> ConsultarDatosClienteAS400(string CIF)
        {
            var sp = "SP_CONSULTAR_DATOSCLIENTES_PEL";
            var parametros = new DynamicParameters();
            parametros.Add("ICHCIF", CIF, dbType: DbType.String, direction: ParameterDirection.Input);
            parametros.Add("OINTCODIGOERROR", 0, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parametros.Add("OVARMENSAJEERROR", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);
            var request = await _infraestructuraDocker.ObtenerDB2<DatosClientesPELDTO>(sp, parametros);

            return request.Values;
        }

        public async Task<IEnumerable<LimitesGeneralesDTO>> LimitesGenerales(string Categoria)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Categoria", Categoria);
            return await _infraestructuraDocker.ObtenerListaSQL<LimitesGeneralesDTO>("SP_OBTENER_LIMITE_CAT", parametros);
        }

        public async Task<GetGestiones> GetGestionesId(string idGestion)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Id", idGestion);
            return await _infraestructuraDocker.ObtenerSQL<GetGestiones>("SP_OBTENER_GESTIONES_ID", parametros);
        }

        public async Task RollBackTarjetaAsignadaAgencia(int idTarjeta)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idTarjeta", idTarjeta);
            await _infraestructuraDocker.EjecutarSQL("SP_ROLLBACK_TARJETA_ASIGNADA_AGENCIA", parametros);
        }

        public async Task<ObtenerUsuarioPELDTO> ConsultarDatosClienteSQL(string User)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@NombreUsuario", User);
            return await _infraestructuraDocker.ObtenerSQL<ObtenerUsuarioPELDTO>("sp_obtener_usuario_pel", parametros, "Popular_Seguridad");
        }

        public async Task AceptarTodasTarjeta(int idAgencia)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdAgencia", idAgencia);
            await _infraestructuraDocker.EjecutarSQL("SP_ACEPTAR_TARJETA_AGENCIA_TODAS", parametros);
        }

        public async Task DenegarGestion(string idGestion)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IdGestion", idGestion);
            await _infraestructuraDocker.EjecutarSQL("SP_DENEGAR_GESTION", parametros);
        }

        public async Task<IEnumerable<LimitesGeneralesDTO>> ObtenerListaLimites()
        {
            return await _infraestructuraDocker.ObtenerListaSQL<LimitesGeneralesDTO>("SP_OBTENER_LIMITES_GENERALES");
        }

        public async Task <bool> EditarLimites(LimitesGeneralesDTO limitesGeneralesDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Categoria", limitesGeneralesDTO.Categoria);
            parametros.Add("@Descripcion", limitesGeneralesDTO.Descripcion);
            parametros.Add("@LimiteRetiroNumeroDiario", limitesGeneralesDTO.LimiteRetiroNumeroDiario);
            parametros.Add("@LimiteRetiroMontoDiario", limitesGeneralesDTO.LimiteRetiroMontoDiario);
            parametros.Add("@LimiteRetiroMontoTxn", limitesGeneralesDTO.LimiteRetiroMontoTxn);
            parametros.Add("@LimiteCompraNumeroDiario", limitesGeneralesDTO.LimiteCompraNumeroDiario);
            parametros.Add("@LimiteCompraMontoDiario", limitesGeneralesDTO.LimiteCompraMontoDiario);
            parametros.Add("@LimiteCompraMontoTxn", limitesGeneralesDTO.LimiteCompraMontoTxn);
            await _infraestructuraDocker.EjecutarSQL("SP_EDITAR_LIMITE_CAT", parametros);
            return true;
        }

        public async Task<bool> CrearLimites(LimitesGeneralesDTO limitesGeneralesDTO)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Categoria", limitesGeneralesDTO.Categoria);
            parametros.Add("@Descripcion", limitesGeneralesDTO.Descripcion);
            parametros.Add("@LimiteRetiroNumeroDiario", limitesGeneralesDTO.LimiteRetiroNumeroDiario);
            parametros.Add("@LimiteRetiroMontoDiario", limitesGeneralesDTO.LimiteRetiroMontoDiario);
            parametros.Add("@LimiteRetiroMontoTxn", limitesGeneralesDTO.LimiteRetiroMontoTxn);
            parametros.Add("@LimiteCompraNumeroDiario", limitesGeneralesDTO.LimiteCompraNumeroDiario);
            parametros.Add("@LimiteCompraMontoDiario", limitesGeneralesDTO.LimiteCompraMontoDiario);
            parametros.Add("@LimiteCompraMontoTxn", limitesGeneralesDTO.LimiteCompraMontoTxn);
            await _infraestructuraDocker.EjecutarSQL("SP_CREAR_LIMITE_CAT", parametros);
            return true;
        }
    }
}