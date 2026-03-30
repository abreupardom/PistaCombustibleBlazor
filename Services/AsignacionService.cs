using Microsoft.Data.SqlClient;
using PistaCombustible.Data;
using PistaCombustible.Models;
using System.Data;

namespace PistaCombustible.Services
{
    public class AsignacionService
    {
        private readonly ConexionSQL _conexion;

        public AsignacionService(ConexionSQL conexion)
        {
            _conexion = conexion;
        }

        /// <summary>
        /// Obtener todos los asignacion
        /// </summary>
        /// <returns></returns>
        public List<Asignacion> GetAsignaciones()
        {
            string query = @"
                    SELECT
                        a.Id,
                        Tanque,
                        Vehiculo,
                        Litros,
                        a.FechaCreacion,
                        CONCAT (t.Id, ' / ', t.Nivel, ' / ', t.TipoCombustible) AS tanqueCompleto,
                        CONCAT (v.Marca, ' / ', v.Modelo, ' / ', v.Matricula) AS vehiculoCompleto
                    FROM
                        Asignaciones AS a
                    INNER JOIN Tanques AS t ON a.Tanque = t.Id
                    INNER JOIN Vehiculos AS v ON a.Vehiculo = v.Id
                    ORDER BY
                        Id DESC";

            var dataTable = _conexion.EjecutarConsulta(query);
            var Asignacion = new List<Asignacion>();

            foreach (DataRow row in dataTable.Rows)
            {
                Asignacion.Add(new Asignacion
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Tanque = Convert.ToInt32(row["Tanque"]),
                    Vehiculo = Convert.ToInt32(row["Vehiculo"]),
                    Litros = row["Litros"] == DBNull.Value ? null : Convert.ToDecimal(row["Litros"]),
                    FechaCreacion = Convert.ToDateTime(row["FechaCreacion"]),
                    tanqueCompleto = Convert.ToString(row["tanqueCompleto"]),
                    vehiculoCompleto = Convert.ToString(row["vehiculoCompleto"])
                });
            }

            return Asignacion;
        }

        /// <summary>
        /// Obtener asignacion por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Asignacion GetAsignacionById(int id)
        {
            string query = @"
                SELECT Id, Tanque, Vehiculo, Litros, FechaCreacion
                FROM Asignaciones 
                WHERE Id = @Id";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = _conexion.EjecutarConsulta(query, parametros);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            return new Asignacion
            {
                Id = Convert.ToInt32(row["Id"]),
                Tanque = Convert.ToInt32(row["Tanque"]),
                Vehiculo = Convert.ToInt32(row["Vehiculo"]),
                Litros = row["Litros"] == DBNull.Value ? null : Convert.ToDecimal(row["Litros"]),
                FechaCreacion = Convert.ToDateTime(row["FechaCreacion"])
            };
        }

        /// <summary>
        /// Agregar nuevo asignacion
        /// </summary>
        /// <param name="asignacion"></param>
        /// <returns></returns>
        public int AddAsignacion(Asignacion asignacion)
        {
            string query = @"
                INSERT INTO Asignaciones (Tanque, Vehiculo, Litros, FechaCreacion)
                VALUES (@Tanque, @Vehiculo, @Litros,  GETDATE());
                SELECT SCOPE_IDENTITY();";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Tanque", asignacion.Tanque),
                new SqlParameter("@Vehiculo", asignacion.Vehiculo),
                new SqlParameter("@Litros", asignacion.Litros),

            };

            var resultado = _conexion.EjecutarEscalar(query, parametros);
            if (Convert.ToInt32(resultado) > 0)
            {
                query = @"
                    UPDATE Tanques
                    SET Nivel = Nivel - @Litros
                    WHERE
                      Id = @Id";

                parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", asignacion.Tanque),
                    new SqlParameter("@Litros", asignacion.Litros),
                };

                resultado = _conexion.EjecutarEscalar(query, parametros);
            }
            return Convert.ToInt32(resultado);
        }

        /// <summary>
        /// Actualizar asignacion
        /// </summary>
        /// <param name="asignacion"></param>
        /// <returns></returns>
        public bool UpdateAsignacion(Asignacion asignacion)
        {
            string query = @"
                SELECT Litros
                FROM Asignaciones 
                WHERE Id = @Id";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", asignacion.Id),
            };

            var asignacionLitros = _conexion.EjecutarEscalar(query, parametros);

            query = @"
                UPDATE Asignaciones 
                SET Tanque = @Tanque, 
                    Vehiculo = @Vehiculo, 
                    Litros = @Litros
                WHERE Id = @Id";

            parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", asignacion.Id),
                new SqlParameter("@Tanque", asignacion.Tanque),
                new SqlParameter("@Vehiculo", asignacion.Vehiculo),
                new SqlParameter("@Litros", asignacion.Litros),
            };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            
            if (filasAfectadas > 0)
            {
                query = @"
                    UPDATE Tanques
                    SET Nivel = Nivel + @asignacionLitros - @Litros
                    WHERE
                      Id = @Id";

                parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", asignacion.Tanque),
                    new SqlParameter("@asignacionLitros", asignacionLitros),
                    new SqlParameter("@Litros", asignacion.Litros),
                };

                _conexion.EjecutarEscalar(query, parametros);
            }
            return filasAfectadas > 0;
        }


        /// <summary>
        /// Eliminar asignacion físicamente (borrado permanente)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAsignacionFisico(int id)
        {
            string query = "DELETE FROM Asignaciones WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Obtener cantidad total de asignaciones
        /// </summary>
        /// <returns></returns>
        public int GetTotalAsignaciones()
        {
            string query = "SELECT COUNT(*) FROM Asignaciones";
            var resultado = _conexion.EjecutarEscalar(query);
            return Convert.ToInt32(resultado);
        }
    }
}