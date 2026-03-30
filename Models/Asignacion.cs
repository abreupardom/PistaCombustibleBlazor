namespace PistaCombustible.Models
{
    public class Asignacion
    {
        public int Id { get; set; }
        public int Tanque { get; set; }
        public int Vehiculo { get; set; }
        public Decimal? Litros { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string tanqueCompleto { get; set;}
        public string vehiculoCompleto { get; set;}
    }
}