using Microsoft.Data.SqlClient;
using System.Data;

namespace PistaCombustible.Data
{
    public class ConexionSQL
    {
        private readonly string _cadenaConexion;

        public ConexionSQL(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        /// <summary>
        /// Método para obtener la conexión abierta
        /// </summary>
        /// <returns></returns>
        public SqlConnection ObtenerConexion()
        {
            var conexion = new SqlConnection(_cadenaConexion);
            conexion.Open();
            return conexion;
        }

        /// <summary>
        ///  Método para ejecutar consultas (SELECT)
        /// </summary>
        /// <param name="queryparam"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public DataTable EjecutarConsulta(string query, SqlParameter[] parametros = null)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                using (var comando = new SqlCommand(query, conexion))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }

                    var dataTable = new DataTable();
                    var adapter = new SqlDataAdapter(comando);
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        /// <summary>
        ///  Método para ejecutar comandos (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="queryparam"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public int EjecutarComando(string query, SqlParameter[] parametros = null)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand(query, conexion))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }
                    return comando.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///  Método para obtener un solo valor (COUNT, SUM, etc.)
        /// </summary>
        /// <param name="queryparam"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public object EjecutarEscalar(string query, SqlParameter[] parametros = null)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand(query, conexion))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }
                    return comando.ExecuteScalar();
                }
            }
        }
    }
}