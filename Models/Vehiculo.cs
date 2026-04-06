using System.ComponentModel.DataAnnotations;
namespace PistaCombustible.Models
{
    public class Vehiculo
    {
        [Required(ErrorMessage = "El id es requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "La marca es requerida")]  
        public string? Marca { get; set; }
        [Required(ErrorMessage = "El modelo es requerido")]
        public string? Modelo { get; set; }
        [Required(ErrorMessage = "La matricula es requerida")]
        public string? Matricula { get; set; }
        public bool Activo { get; set; }
        [Required(ErrorMessage = "El tipo de combustible es requerido")]
        public string? TipoCombustible { get; set; }
        [Required(ErrorMessage = "El empleado es requerido")]
        public int? IdEmpleado { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string vehiculo => $"{Marca} / {Modelo} / {Matricula} / {TipoCombustible}";
    }
}