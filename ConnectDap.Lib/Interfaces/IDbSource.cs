using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace ConnectDap.Lib.Interfaces
{
public interface IDbSource
{
    /// <summary>
    /// Ejecuta una consulta SQL y devuelve una colección de resultados de tipo T.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve una colección de resultados de tipo T.</returns>
    Task<IEnumerable<T>> SelectData<T>(string query);

    /// <summary>
    /// Ejecuta una consulta SQL con parámetros y devuelve una colección de resultados de tipo T.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <param name="parameters">Los parámetros de la consulta.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve una colección de resultados de tipo T.</returns>
    Task<IEnumerable<T>> SelectData<T>(string query, DynamicParameters parameters);

    /// <summary>
    /// Ejecuta un procedimiento almacenado con parámetros y devuelve una colección de resultados de tipo T.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado.</param>
    /// <param name="parameters">Los parámetros del procedimiento almacenado (opcional).</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve una colección de resultados de tipo T.</returns>
    Task<IEnumerable<T>> SelectSp<T>(string procedure, DynamicParameters parameters = null);

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el primer resultado de tipo T.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el primer resultado de tipo T.</returns>
    Task<T> FilterData<T>(string query);

    /// <summary>
    /// Ejecuta una consulta SQL con parámetros y devuelve el primer resultado de tipo T.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <param name="parameters">Los parámetros de la consulta.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el primer resultado de tipo T.</returns>
    Task<T> FilterData<T>(string query, DynamicParameters parameters);

    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve el primer resultado de tipo T.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el primer resultado de tipo T.</returns>
    Task<T> FilterSp<T>(string procedure);

    /// <summary>
    /// Ejecuta un procedimiento almacenado con parámetros y devuelve el primer resultado de tipo T.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado esperado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado.</param>
    /// <param name="parameters">Los parámetros del procedimiento almacenado.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el primer resultado de tipo T.</returns>
    Task<T> FilterSp<T>(string procedure, DynamicParameters parameters);

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el identificador de inserción.
    /// </summary>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el identificador de inserción.</returns>
    Task<long> PostData(string query);

    /// <summary>
    /// Ejecuta una consulta SQL con parámetros y devuelve el identificador de inserción.
    /// </summary>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <param name="parameters">Los parámetros de la consulta.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el identificador de inserción.</returns>
    Task<long> PostData(string query, DynamicParameters parameters);

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el número de filas afectadas.
    /// </summary>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
    Task<long> PutData(string query);

    /// <summary>
    /// Ejecuta una consulta SQL con parámetros y devuelve el número de filas afectadas.
    /// </summary>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <param name="parameters">Los parámetros de la consulta.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
    Task<long> PutData(string query, DynamicParameters parameters);

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el número de filas afectadas.
    /// </summary>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
    Task<int> RemoveData(string query);

    /// <summary>
    /// Ejecuta una consulta SQL con parámetros y devuelve el número de filas afectadas.
    /// </summary>
    /// <param name="query">La consulta SQL a ejecutar.</param>
    /// <param name="parameters">Los parámetros de la consulta.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
    Task<int> RemoveData(string query, DynamicParameters parameters);

    /// <summary>
    /// Ejecuta un procedimiento almacenado con parámetros y devuelve el número de filas afectadas.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado.</param>
    /// <param name="parameters">Los parámetros del procedimiento almacenado.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
    Task<int> RemoveSp(string procedure, DynamicParameters parameters);

    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve el número de filas afectadas.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
    Task<int> ExecuteSp(string procedure);

    /// <summary>
    /// Ejecuta un procedimiento almacenado con parámetros y devuelve el número de filas afectadas.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado.</param>
    /// <param name="parameters">Los parámetros del procedimiento almacenado.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el número de filas afectadas.</returns>
    Task<int> ExecuteSp(string procedure, DynamicParameters parameters);
    }
}
