using Microsoft.Data.SqlClient;
using PistaCombustible.Data;
using PistaCombustible.Models;
using System.Data;

namespace PistaCombustible.Services
{
    public class TanqueService
    {
        private readonly ConexionSQL _conexion;

        public TanqueService(ConexionSQL conexion)
        {
            _conexion = conexion;
        }

        /// <summary>
        /// Obtener todos los Tanques
        /// </summary>
        /// <returns></returns>
        public List<Tanque> GetTanques()
        {
            string query = @"
                    SELECT
                        Id,
                        Capacidad,
                        Nivel,
                        TipoCombustible,
                        Activo,
                        FechaCreacion
                    FROM
                        Tanques
                    WHERE
                        Activo >= 0
                        ORDER BY
                        Id DESC";

            var dataTable = _conexion.EjecutarConsulta(query);
            var Tanques = new List<Tanque>();

            foreach (DataRow row in dataTable.Rows)
            {
                Tanques.Add(new Tanque
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Capacidad = row["Capacidad"] == DBNull.Value ? null : Convert.ToDecimal(row["Capacidad"]),
                    Nivel = row["Nivel"] == DBNull.Value ? null : Convert.ToDecimal(row["Nivel"]),
                    TipoCombustible = row["TipoCombustible"].ToString(),
                    Activo = Convert.ToBoolean(row["Activo"]),
                    FechaCreacion = Convert.ToDateTime(row["FechaCreacion"])
                });
            }

            return Tanques;
        }

        /// <summary>
        /// Obtener tanque por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tanque GetTanqueById(int id)
        {
            string query = @"
                SELECT Id, Capacidad, Nivel, TipoCombustible, Activo, FechaCreacion
                FROM Tanques 
                WHERE Id = @Id";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = _conexion.EjecutarConsulta(query, parametros);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            return new Tanque
            {
                Id = Convert.ToInt32(row["Id"]),
                Capacidad = row["Capacidad"] == DBNull.Value ? null : Convert.ToDecimal(row["Capacidad"]),
                Nivel = row["Nivel"] == DBNull.Value ? null : Convert.ToDecimal(row["Nivel"]),
                TipoCombustible = row["TipoCombustible"].ToString(),
                Activo = Convert.ToBoolean(row["Activo"]),
                FechaCreacion = Convert.ToDateTime(row["FechaCreacion"])
            };
        }

        /// <summary>
        /// Agregar nuevo tanque
        /// </summary>
        /// <param name="tanque"></param>
        /// <returns></returns>
        public int AddTanque(Tanque tanque)
        {
            string query = @"
                INSERT INTO Tanques (Capacidad, Nivel, TipoCombustible, Activo, FechaCreacion)
                VALUES (@Capacidad, @Nivel, @TipoCombustible, @Activo,  GETDATE());
                SELECT SCOPE_IDENTITY();";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Capacidad", tanque.Capacidad),
                new SqlParameter("@Nivel", tanque.Nivel),
                new SqlParameter("@TipoCombustible", tanque.TipoCombustible),
                new SqlParameter("@Activo", tanque.Activo),

            };

            var resultado = _conexion.EjecutarEscalar(query, parametros);
            return Convert.ToInt32(resultado);
        }

        /// <summary>
        /// Actualizar tanque
        /// </summary>
        /// <param name="tanque"></param>
        /// <returns></returns>
        public bool UpdateTanque(Tanque tanque)
        {
            string query = @"
                UPDATE Tanques 
                SET Capacidad = @Capacidad, 
                    Nivel = @Nivel, 
                    TipoCombustible = @TipoCombustible,
                    Activo = @Activo
                WHERE Id = @Id";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", tanque.Id),
                new SqlParameter("@Capacidad", tanque.Capacidad),
                new SqlParameter("@Nivel", tanque.Nivel),
                new SqlParameter("@TipoCombustible", tanque.TipoCombustible),
                new SqlParameter("@Activo", tanque.Activo),
            };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Desactivar tanque
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DesactivaTanque(int id)
        {
            string query = "UPDATE Tanques SET Activo = 0 WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Activar tanque
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ActivarTanque(int id)
        {
            string query = "UPDATE Tanques SET Activo = 1 WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Eliminar tanque físicamente (borrado permanente)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTanqueFisico(int id)
        {
            string query = "DELETE FROM Tanques WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Obtener cantidad total de Tanques
        /// </summary>
        /// <returns></returns>
        public int GetTotalTanques()
        {
            string query = "SELECT COUNT(*) FROM Tanques WHERE Activo >= 1";
            var resultado = _conexion.EjecutarEscalar(query);
            return Convert.ToInt32(resultado);
        }
    }
}