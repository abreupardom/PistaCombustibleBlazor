using Microsoft.Data.SqlClient;
using PistaCombustible.Data;
using PistaCombustible.Models;
using System.Data;

namespace PistaCombustible.Services
{
    public class VehiculoService
    {
        private readonly ConexionSQL _conexion;

        public VehiculoService(ConexionSQL conexion)
        {
            _conexion = conexion;
        }

        /// <summary>
        /// Obtener todos los Vehiculos. <br/>
        /// Tenga en cuenta que este método hace un INNER JOIN con la tabla Empleados para obtener el nombre completo del empleado asociado a cada vehículo.<br/>
        /// Si necesitas un filtro específico, puedes pasarlo como parámetro de entrada donde el alias de la tabla Empleados es "e" y el alias de la tabla Vehiculos es "v".
        /// </summary>
        /// <param name="filtro">Filtro opcional para búsqueda (ej: "WHERE v.Activo = 1")</param>
        /// <returns></returns>
        public List<Vehiculo> GetVehiculos(string filtro = "")
        {
            string query = @"
                    SELECT
                        v.Id,
                        v.Marca,
                        v.Modelo,
                        v.Matricula,
                        v.Activo,
                        v.IdEmpleado,
                        concat(e.Nombre, ' ', e.Apellido) Nombre,
                        v.TipoCombustible,
                        v.FechaCreacion
                    FROM
                        Vehiculos AS v
                    INNER JOIN Empleados AS e ON v.IdEmpleado = e.Id " +
                    filtro +
                    @" ORDER BY v.Id DESC";

            var dataTable = _conexion.EjecutarConsulta(query);
            var Vehiculos = new List<Vehiculo>();

            foreach (DataRow row in dataTable.Rows)
            {
                Vehiculos.Add(new Vehiculo
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Marca = row["Marca"].ToString(),
                    Modelo = row["Modelo"].ToString(),
                    Matricula = row["Matricula"].ToString(),
                    Activo = Convert.ToBoolean(row["Activo"]),
                    TipoCombustible = row["TipoCombustible"].ToString(),
                    IdEmpleado = Convert.ToInt32(row["IdEmpleado"]),
                    Nombre = row["Nombre"].ToString(),
                    FechaCreacion = Convert.ToDateTime(row["FechaCreacion"])
                });
            }

            return Vehiculos;
        }

        /// <summary>
        /// Obtener vehiculo por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vehiculo GetVehiculoById(int id)
        {
            string query = @"
                SELECT Id, Marca, Modelo, Matricula, Activo, IdEmpleado,TipoCombustible, FechaCreacion
                FROM Vehiculos 
                WHERE Id = @Id";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = _conexion.EjecutarConsulta(query, parametros);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            return new Vehiculo
            {
                Id = Convert.ToInt32(row["Id"]),
                Marca = row["Marca"].ToString(),
                Modelo = row["Modelo"].ToString(),
                Matricula = row["Matricula"].ToString(),
                Activo = Convert.ToBoolean(row["Activo"]),
                TipoCombustible = row["TipoCombustible"].ToString(),
                IdEmpleado = Convert.ToInt32(row["IdEmpleado"]),
                FechaCreacion = Convert.ToDateTime(row["FechaCreacion"])
            };
        }

        /// <summary>
        /// Agregar nuevo vehiculo
        /// </summary>
        /// <param name="vehiculo"></param>
        /// <returns></returns>
        public int AddVehiculo(Vehiculo vehiculo)
        {
            string query = @"
                INSERT INTO Vehiculos (Marca, Modelo, Matricula, Activo, TipoCombustible,  IdEmpleado, FechaCreacion)
                VALUES (@Marca, @Modelo, @Matricula, @Activo, @TipoCombustible, @IdEmpleado, GETDATE());
                SELECT SCOPE_IDENTITY();";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Marca", vehiculo.Marca),
                new SqlParameter("@Modelo", vehiculo.Modelo),
                new SqlParameter("@Matricula", vehiculo.Matricula),
                new SqlParameter("@Activo", vehiculo.Activo),
                new SqlParameter("@TipoCombustible", vehiculo.TipoCombustible),
                new SqlParameter("@IdEmpleado", vehiculo.IdEmpleado),
            };

            var resultado = _conexion.EjecutarEscalar(query, parametros);
            return Convert.ToInt32(resultado);
        }

        /// <summary>
        /// Actualizar vehiculo
        /// </summary>
        /// <param name="vehiculo"></param>
        /// <returns></returns>
        public bool UpdateVehiculo(Vehiculo vehiculo)
        {
            string query = @"
                UPDATE Vehiculos 
                SET Marca = @Marca, 
                    Modelo = @Modelo, 
                    Matricula = @Matricula, 
                    Activo = @Activo,
                    TipoCombustible = @TipoCombustible,
                    IdEmpleado = @IdEmpleado
                WHERE Id = @Id";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", vehiculo.Id),
                new SqlParameter("@Marca", vehiculo.Marca),
                new SqlParameter("@Modelo", vehiculo.Modelo),
                new SqlParameter("@Matricula", vehiculo.Matricula),
                new SqlParameter("@Activo", vehiculo.Activo),
                new SqlParameter("@TipoCombustible", vehiculo.TipoCombustible),
                new SqlParameter("@IdEmpleado", vehiculo.IdEmpleado),
            };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Desactivar vehiculo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DesactivaVehiculo(int id)
        {
            string query = "UPDATE Vehiculos SET Activo = 0 WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Activar vehiculo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ActivarVehiculo(int id)
        {
            string query = "UPDATE Vehiculos SET Activo = 1 WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Eliminar vehiculo físicamente (borrado permanente)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteVehiculoFisico(int id)
        {
            string query = "DELETE FROM Vehiculos WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Obtener cantidad total de Vehiculos
        /// </summary>
        /// <returns></returns>
        public int GetTotalVehiculos()
        {
            string query = "SELECT COUNT(*) FROM Vehiculos WHERE Activo => 1";
            var resultado = _conexion.EjecutarEscalar(query);
            return Convert.ToInt32(resultado);
        }
    }
}