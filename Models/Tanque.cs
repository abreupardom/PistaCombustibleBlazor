using System.ComponentModel.DataAnnotations;
namespace PistaCombustible.Models
{
    public class Tanque
    {
        [Required(ErrorMessage = "El id es requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El capacidad es requerida")]
        public Decimal? Capacidad { get; set; }
        [Required(ErrorMessage = "El nivel es requerido")]
        public Decimal? Nivel { get; set; }
        [Required(ErrorMessage = "El tipo de combustible es requerido")]
        public string? TipoCombustible { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string tanque => $"{Id} / {Nivel}L / {TipoCombustible}";
    }
}