namespace PistaCombustible.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public decimal? Salario { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}