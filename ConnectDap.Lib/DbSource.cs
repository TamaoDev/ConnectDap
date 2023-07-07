using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ConnectDap.Lib.Interfaces;
using Dapper;

namespace ConnectDap.Lib
{
    /// <summary>
    /// Clase DbSource para la conexión a Dapper usando IDbConnection para crear la connection.
    /// </summary>
    public class DbSource : IDbSource
    {
        private readonly IDbConnection _connection;

        /// <summary>
        /// Constructor de la clase DbSource.
        /// </summary>
        /// <typeparam name="SqlConnection">"Server=miServidor;Database=miBaseDeDatos;User Id=miUsuario;Password=miContraseña;"</typeparam>
        /// <typeparam name="MySqlConnection">"Server=miServidor;Database=miBaseDeDatos;Uid=miUsuario;Pwd=miContraseña;"</typeparam>
        /// <typeparam name="OracleConnection">"Data Source=miServidor;User Id=miUsuario;Password=miContraseña;"</typeparam>
        /// <typeparam name="NpgsqlConnection">"Server=miServidor;Port=5432;Database=miBaseDeDatos;User Id=miUsuario;Password=miContraseña;"</typeparam>
        /// <typeparam name="SQLiteConnection">"Data Source=rutaAlArchivo.db;"</typeparam>

        /// <param name="connection">El IDbConnection conexión a la base de las siguientes Bases de datos.</param>
        public DbSource(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Ejecuta una consulta SQL y devuelve una colección de resultados de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve una colección de resultados de tipo T.</returns>
        public async Task<IEnumerable<T>> SelectData<T>(string query)
        {
            return await ExecuteQuery<T>(query);
        }

        /// <summary>
        /// Ejecuta una consulta SQL con parámetros y devuelve una colección de resultados de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <param name="parameters">Los parámetros de la consulta.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve una colección de resultados de tipo T.</returns>
        public async Task<IEnumerable<T>> SelectData<T>(string query, DynamicParameters parameters)
        {
            return await ExecuteQuery<T>(query, parameters, false, true);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado con parámetros y devuelve una colección de resultados de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
        /// <param name="procedure">El nombre del procedimiento almacenado.</param>
        /// <param name="parameters">Los parámetros del procedimiento almacenado (opcional).</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve una colección de resultados de tipo T.</returns>
        public async Task<IEnumerable<T>> SelectSp<T>(string procedure, DynamicParameters parameters = null)
        {
            return await ExecuteQuery<T>(procedure, parameters, true, parameters != null);
        }

        /// <summary>
        /// Ejecuta una consulta SQL y devuelve el primer resultado de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el primer resultado de tipo T.</returns>
        public async Task<T> FilterData<T>(string query)
        {
            return await ExecuteFirst<T>(query);
        }

        /// <summary>
        /// Ejecuta una consulta SQL con parámetros y devuelve el primer resultado de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <param name="parameters">Los parámetros de la consulta.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el primer resultado de tipo T.</returns>
        public async Task<T> FilterData<T>(string query, DynamicParameters parameters )
        {
            return await ExecuteFirst<T>(query, parameters, false, true);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y devuelve el primer resultado de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
        /// <param name="procedure">El nombre del procedimiento almacenado.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el primer resultado de tipo T.</returns>
        public async Task<T> FilterSp<T>(string procedure)
        {
            return await ExecuteFirst<T>(procedure, null, true);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado con parámetros y devuelve el primer resultado de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
        /// <param name="procedure">El nombre del procedimiento almacenado.</param>
        /// <param name="parameters">Los parámetros del procedimiento almacenado.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el primer resultado de tipo T.</returns>
        public async Task<T> FilterSp<T>(string procedure, DynamicParameters parameters)
        {
            return await ExecuteFirst<T>(procedure, parameters, true, true);
        }

        /// <summary>
        /// Ejecuta una consulta SQL y devuelve el identificador de inserción.
        /// </summary>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el identificador de inserción.</returns>
        public async Task<long> PostData(string query)
        {
            return await ExecuteIdentity(query);
        }

        /// <summary>
        /// Ejecuta una consulta SQL con parámetros y devuelve el identificador de inserción.
        /// </summary>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <param name="parameters">Los parámetros de la consulta.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el identificador de inserción.</returns>
        public async Task<long> PostData(string query, DynamicParameters parameters)
        {
            return await ExecuteIdentity(query, parameters, false, true);
        }

        /// <summary>
        /// Ejecuta una consulta SQL y devuelve el número de filas afectadas.
        /// </summary>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
        public async Task<long> PutData(string query)
        {
            return await ExecuteSingle(query);
        }

        /// <summary>
        /// Ejecuta una consulta SQL con parámetros y devuelve el número de filas afectadas.
        /// </summary>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <param name="parameters">Los parámetros de la consulta.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
        public async Task<long> PutData(string query, DynamicParameters parameters)
        {
            return await ExecuteSingle(query, parameters, false, true);
        }

        /// <summary>
        /// Ejecuta una consulta SQL y devuelve el número de filas afectadas.
        /// </summary>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
        public async Task<int> RemoveData(string query)
        {
            return await ExecuteSingle(query);
        }

        /// <summary>
        /// Ejecuta una consulta SQL con parámetros y devuelve el número de filas afectadas.
        /// </summary>
        /// <param name="query">La consulta SQL a ejecutar.</param>
        /// <param name="parameters">Los parámetros de la consulta.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
        public async Task<int> RemoveData(string query, DynamicParameters parameters)
        {
            return await ExecuteSingle(query, parameters, false, true);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado con parámetros y devuelve el número de filas afectadas.
        /// </summary>
        /// <param name="procedure">El nombre del procedimiento almacenado.</param>
        /// <param name="parameters">Los parámetros del procedimiento almacenado.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
        public async Task<int> RemoveSp(string procedure, DynamicParameters parameters)
        {
            return await ExecuteSingle(procedure, parameters, true, true);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y devuelve el número de filas afectadas.
        /// </summary>
        /// <param name="procedure">El nombre del procedimiento almacenado.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
        public async Task<int> ExecuteSp(string procedure)
        {
            return await ExecuteSingle(procedure, null, true);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado con parámetros y devuelve el número de filas afectadas.
        /// </summary>
        /// <param name="procedure">El nombre del procedimiento almacenado.</param>
        /// <param name="parameters">Los parámetros del procedimiento almacenado.</param>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
        public async Task<int> ExecuteSp(string procedure, DynamicParameters parameters)
        {
            return await ExecuteSingle(procedure, parameters, true, true);
        }

        /// <summary>
        /// Realiza una transacción que consiste en la inserción de un registro maestro y varios registros de detalle.
        /// </summary>
        /// <typeparam name="T">El tipo de objeto para el registro maestro.</typeparam>
        /// <typeparam name="T2">El tipo de objeto para los registros de detalle.</typeparam>
        /// <param name="queryMaestro">La consulta SQL para insertar el registro maestro.</param>
        /// <param name="queryDetalle">La consulta SQL para insertar los registros de detalle.</param>
        /// <param name="maestro">El objeto que representa el registro maestro.</param>
        /// <param name="detalle">La lista de objetos que representan los registros de detalle.</param>
        /// <param name="idMaestro">El ID del registro maestro, si es diferente de cero.</param>
        /// /// <remarks>
        /// Para que los registros de detalle tomen el ID del maestro, asegúrate de que tu consulta SQL para insertar los registros de detalle
        /// incluya un parámetro @idMaestro como FK de maestro en el detalle.
        /// </remarks>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve el ID del registro maestro insertado.</returns>
        public async Task<long> TransactionData<T, T2>(string queryMaestro, string queryDetalle, T maestro, List<T2> detalle, long idMaestro = 0)
        {
            using (var cn = _connection)
            {
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters parametersMaestro = new DynamicParameters(maestro);
                        queryMaestro = queryMaestro.ToUpper().Contains("INSERT INTO") ? queryMaestro + ";select cast(SCOPE_IDENTITY() as bigint)" : queryMaestro;
                        long id = idMaestro == 0
                            ? await cn.QuerySingleAsync<long>(queryMaestro, parametersMaestro, transaction: transaction, commandType: CommandType.Text).ConfigureAwait(false)
                            : await cn.ExecuteAsync(queryMaestro, parametersMaestro, transaction: transaction, commandType: CommandType.Text).ConfigureAwait(false);
                        id = idMaestro == 0 ? id : idMaestro;
                        foreach (T2 item in detalle)
                        {
                            DynamicParameters parametersDetalle = new DynamicParameters(item);
                            parametersDetalle.Add("@idMaestro", id);
                            await cn.ExecuteAsync(queryDetalle, parametersDetalle, transaction: transaction, commandType: CommandType.Text).ConfigureAwait(false);
                        }

                        transaction.Commit();
                        return id;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        // Otros métodos y funciones auxiliares...
        private async Task<IEnumerable<T>> ExecuteQuery<T>(string query, DynamicParameters parameters = null, bool iSprocedure = false, bool withParameters = false)
        {
            using (var cn = _connection)
            {
                var objetoreturn = await cn.QueryAsync<T>(query, param: withParameters ? parameters : null,
                    commandType: iSprocedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(false);
                return objetoreturn;
            }
        }

        private async Task<T> ExecuteFirst<T>(string query, DynamicParameters parameters = null, bool iSprocedure = false, bool withParameters = false)
        {
            using (var cn = _connection)
            {
                var objetoreturn = await cn.QuerySingleAsync<T>(query, param: withParameters ? parameters : null,
                    commandType: iSprocedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(false);

                return objetoreturn;
            }
        }

        private async Task<long> ExecuteIdentity(string query, DynamicParameters parameters = null, bool iSprocedure = false, bool withParameters = false)
        {
            using (var cn = _connection)
            {
                query = query.ToUpper().Contains("INSERT INTO") ? query + ";select cast(SCOPE_IDENTITY() as bigint)" : query;
                var objetoreturn = await cn.QueryFirstOrDefaultAsync<int>(query, param: withParameters ? parameters : null,
                    commandType: iSprocedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(false);
                return objetoreturn;
            }
        }

        private async Task<int> ExecuteSingle(string query, DynamicParameters parameters = null, bool iSprocedure = false, bool withParameters = false)
        {
            using (var cn = _connection)
            {
                var objetoreturn = await cn.ExecuteAsync(query, param: withParameters ? parameters : null,
                    commandType: iSprocedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(false);
                return objetoreturn;
            }
        }
    }
}