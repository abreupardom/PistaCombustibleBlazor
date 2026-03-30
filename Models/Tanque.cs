namespace PistaCombustible.Models
{
    public class Tanque
    {
        public int Id { get; set; }
        public Decimal? Capacidad { get; set; }
        public Decimal? Nivel { get; set; }
        public string? TipoCombustible { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string tanque => $"{Id} / {Nivel}L / {TipoCombustible}";
    }
}