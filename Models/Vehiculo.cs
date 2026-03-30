namespace PistaCombustible.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Matricula { get; set; }
        public bool Activo { get; set; }
        public string? TipoCombustible { get; set; }
        public int IdEmpleado { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string vehiculo => $"{Marca} / {Modelo} / {Matricula} / {TipoCombustible}";
    }
}