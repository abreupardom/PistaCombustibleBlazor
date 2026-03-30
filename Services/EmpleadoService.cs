using Microsoft.Data.SqlClient;
using PistaCombustible.Data;
using PistaCombustible.Models;
using System.Data;

namespace PistaCombustible.Services
{
    public class EmpleadoService
    {
        private readonly ConexionSQL _conexion;

        public EmpleadoService(ConexionSQL conexion)
        {
            _conexion = conexion;
        }

        /// <summary>
        /// Obtener todos los empleados
        /// </summary>
        /// <returns></returns>
        public List<Empleado> GetEmpleados()
        {
            string query = @"
                SELECT 
                    Id,
                    Nombre,
                    Apellido,
                    Email,
                    FechaNacimiento, 
                    Salario,
                    Activo,
                    FechaCreacion
                FROM Empleados 
                WHERE
                    Activo > 0
                ORDER BY Id DESC";

            var dataTable = _conexion.EjecutarConsulta(query);
            var empleados = new List<Empleado>();

            foreach (DataRow row in dataTable.Rows)
            {
                empleados.Add(new Empleado
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    Apellido = row["Apellido"].ToString(),
                    Email = row["Email"].ToString(),
                    FechaNacimiento = row["FechaNacimiento"] == DBNull.Value ? null : Convert.ToDateTime(row["FechaNacimiento"]),
                    Salario = row["Salario"] == DBNull.Value ? null : Convert.ToDecimal(row["Salario"]),
                    Activo = Convert.ToBoolean(row["Activo"]),
                    FechaCreacion = Convert.ToDateTime(row["FechaCreacion"])
                });
            }

            return empleados;
        }

        /// <summary>
        /// Obtener empleado por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Empleado GetEmpleadoById(int id)
        {
            string query = @"
                SELECT Id, Nombre, Apellido, Email, FechaNacimiento, 
                       Salario, Activo, FechaCreacion
                FROM Empleados 
                WHERE Id = @Id";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = _conexion.EjecutarConsulta(query, parametros);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            return new Empleado
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Email = row["Email"].ToString(),
                FechaNacimiento = row["FechaNacimiento"] == DBNull.Value ? null : Convert.ToDateTime(row["FechaNacimiento"]),
                Salario = row["Salario"] == DBNull.Value ? null : Convert.ToDecimal(row["Salario"]),
                Activo = Convert.ToBoolean(row["Activo"]),
                FechaCreacion = Convert.ToDateTime(row["FechaCreacion"])
            };
        }

        /// <summary>
        /// Agregar nuevo empleado
        /// </summary>
        /// <param name="empleado"></param>
        /// <returns></returns>
        public int AddEmpleado(Empleado empleado)
        {
            string query = @"
                INSERT INTO Empleados (Nombre, Apellido, Email, FechaNacimiento, Salario, Activo, FechaCreacion)
                VALUES (@Nombre, @Apellido, @Email, @FechaNacimiento, @Salario, @Activo, GETDATE());
                SELECT SCOPE_IDENTITY();";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Apellido", empleado.Apellido),
                new SqlParameter("@Email", empleado.Email),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento ?? (object)DBNull.Value),
                new SqlParameter("@Salario", empleado.Salario ?? (object)DBNull.Value),
                new SqlParameter("@Activo", empleado.Activo)
            };

            var resultado = _conexion.EjecutarEscalar(query, parametros);
            return Convert.ToInt32(resultado);
        }

        /// <summary>
        /// Actualizar empleado
        /// </summary>
        /// <param name="empleado"></param>
        /// <returns></returns>
        public bool UpdateEmpleado(Empleado empleado)
        {
            string query = @"
                UPDATE Empleados 
                SET Nombre = @Nombre, 
                    Apellido = @Apellido, 
                    Email = @Email, 
                    FechaNacimiento = @FechaNacimiento, 
                    Salario = @Salario, 
                    Activo = @Activo
                WHERE Id = @Id";

            var parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", empleado.Id),
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Apellido", empleado.Apellido),
                new SqlParameter("@Email", empleado.Email),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento ?? (object)DBNull.Value),
                new SqlParameter("@Salario", empleado.Salario ?? (object)DBNull.Value),
                new SqlParameter("@Activo", empleado.Activo)
            };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Desactivar empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DesactivarEmpleado(int id)
        {
            string query = "UPDATE Empleados SET Activo = 0 WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Activar empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ActivarEmpleado(int id)
        {
            string query = "UPDATE Empleados SET Activo = 1 WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Eliminar empleado físicamente (borrado permanente)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEmpleadoFisico(int id)
        {
            string query = "DELETE FROM Empleados WHERE Id = @Id";
            var parametros = new SqlParameter[] { new SqlParameter("@Id", id) };

            int filasAfectadas = _conexion.EjecutarComando(query, parametros);
            return filasAfectadas > 0;
        }

        /// <summary>
        /// Obtener cantidad total de empleados
        /// </summary>
        /// <returns></returns>
        public int GetTotalEmpleados()
        {
            string query = "SELECT COUNT(*) FROM Empleados WHERE Activo => 1";
            var resultado = _conexion.EjecutarEscalar(query);
            return Convert.ToInt32(resultado);
        }
    }
}