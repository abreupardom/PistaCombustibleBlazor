using System.ComponentModel.DataAnnotations;
namespace PistaCombustible.Models
{
    public class Asignacion
    {
        [Required(ErrorMessage = "El id es requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El tanque es requerido")]
        public int? Tanque { get; set; }
        [Required(ErrorMessage = "El vehículo es requerido")]
        public int? Vehiculo { get; set; }
        [Required(ErrorMessage = "Los litros son requeridos")]
        public Decimal? Litros { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string tanqueCompleto { get; set; }
        public string vehiculoCompleto { get; set; }
    }
}