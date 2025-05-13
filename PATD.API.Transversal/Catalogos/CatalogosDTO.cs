using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PATD.API.Transversal.AdminTarjeta;

namespace PATD.API.Transversal.Catalogos
{
    public class CatalogosDTO
    {
        public IEnumerable<CRolesUsuarioDTO>? RolesUsuarios { get; set; }
        public IEnumerable<CAgenciaDTO>? CatalagoAgencias { get; set; }
        public IEnumerable<DescriptivosDTO>? TiposTarjetas { get; set; }
        public IEnumerable<CEstadosTarjetasDTO>? EstadosTarjetas { get; set; }
        public IEnumerable<Descriptivos2DTO>? MotivosCancelacion { get; set; }
        public IEnumerable<Descriptivos2DTO>? RazonesReposicion { get; set; }
        public IEnumerable<Descriptivos2DTO>? MotivosBloqueo { get; set; }
        public IEnumerable<DescriptivosDTO>? EstadosLogisticoTarjetas { get; set; }
        public IEnumerable<DescriptivosDTO>? TipoGestion { get; set; }
        public IEnumerable<DescriptivosDTO>? EstadoGestion { get; set; }
        public IEnumerable<CPaisesIsoDTO>? PaisesIso { get; set; }
    }
    public class CRolesUsuarioDTO
    {
        public int? Id { get; set; }
        public string? Descripcion { get; set; }

    }
    public class CAgenciaDTO
    {
        public int? Id { get; set; }
        public string? Descripcion { get; set; }
        public int? IdDepartamento { get; set; }
        public string? DescripcionDepartamento { get; set; }
        public int? IdCiudad { get; set; }
        public string? DescripcionCiudad { get; set; }
        public string? CodPostal { get; set; }
        public string? Direccion { get; set; }
        public string? Latitud { get; set; }
        public string? Longitud { get; set; }
        public bool? Estado { get; set; }
    }
    public class DescriptivosDTO
    {
        public int? Id { get; set; }
        public string? Descripcion { get; set; }
    }
    public class CEstadosTarjetasDTO
    {
        public int? Id { get; set; }
        public string? CodResultado { get; set; }
        public string? Descripcion { get; set; }
    }
    public class Descriptivos2DTO
    {
        public string? Id { get; set; }
        public string? Descripcion { get; set; }
    }
}
