using System.ComponentModel.DataAnnotations;
namespace PistaCombustible.Models
{
    public class Empleado
    {
        [Required(ErrorMessage = "El id es requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es requerido")]
        public string? Apellido { get; set; }
        [Required(ErrorMessage = "El email es requerido")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime? FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El salario es requerido")]
        public decimal? Salario { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}