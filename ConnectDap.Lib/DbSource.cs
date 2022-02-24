using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ConnectDap.Lib.Interfaces;
using Dapper;

namespace ConnectDap.Lib
{
    public class DbSource : IDbSource
    {
        private readonly IDbConnection _connection;

        public DbSource(IDbConnection connection)
        {
            _connection = connection;

        }

        public async Task<IEnumerable<T>> SelectData<T>(string query)
        {
            return await ExecuteQuery<T>(query);
        }

        public async Task<IEnumerable<T>> SelectData<T>(string query, DynamicParameters parameters)
        {
            return await ExecuteQuery<T>(query, parameters, false, true);
        }

        public async Task<IEnumerable<T>> SelectSp<T>(string procedure, DynamicParameters parameters)
        {
            return await ExecuteQuery<T>(procedure, parameters, true, true);
        }

        public async Task<T> FilterData<T>(string query)
        {
            return await ExecuteFirst<T>(query);
        }
        public async Task<T> FilterData<T>(string query, DynamicParameters parameters)
        {
            return await ExecuteFirst<T>(query, parameters, false, true);
        }
        public async Task<T> FilterSp<T>(string procedure, DynamicParameters parameters)
        {
            return await ExecuteFirst<T>(procedure, parameters, true, true);
        }

        public async Task<long> PostData(string query)
        {
            return await ExecuteIdentity(query);
        }


        public async Task<long> PostData(string query, DynamicParameters parameters)
        {
            return await ExecuteIdentity(query, parameters, false, true);
        }
        public async Task<long> PostSp(string query, DynamicParameters parameters)
        {
            return await ExecuteSingle(query, parameters, true, true);
        }

        public async Task<long> PutData(string query)
        {
            return await ExecuteSingle(query);
        }
        public async Task<long> PutData(string query, DynamicParameters parameters)
        {
            return await ExecuteSingle(query, parameters, false, true);
        }
        public async Task<long> PutSp(string procedure, DynamicParameters parameters)
        {
            return await ExecuteSingle(procedure, parameters, true, true);
        }
        public async Task<int> RemoveData(string query)
        {
            return await ExecuteSingle(query);
        }
        public async Task<int> RemoveData(string query, DynamicParameters parameters)
        {
            return await ExecuteSingle(query, parameters, false, true);
        }
        public async Task<int> RemoveSp(string procedure, DynamicParameters parameters)
        {
            return await ExecuteSingle(procedure, parameters, true, true);
        }

        protected async Task<long> TransactionData<T, T2>(string queryMaestro, string queryDetalle, T maestro, List<T2> detalle, long idMaestro = 0)
        {

            _connection.Open();

            using var transaction = _connection.BeginTransaction();
            try
            {
                DynamicParameters parametersMaestro = new DynamicParameters(maestro);
                queryMaestro = queryMaestro.ToUpper().Contains("INSERT INTO") ? queryMaestro + ";select cast(SCOPE_IDENTITY() as bigint)" : queryMaestro;
                long id = idMaestro == 0 ? await _connection.QuerySingleAsync<int>(queryMaestro, parametersMaestro, transaction: transaction, commandType: CommandType.Text).ConfigureAwait(false) :
                    await _connection.ExecuteAsync(queryMaestro, parametersMaestro, transaction: transaction, commandType: CommandType.Text).ConfigureAwait(false);
                id = idMaestro == 0 ? id : idMaestro;
                foreach (T2 item in detalle)
                {
                    DynamicParameters parametersDetalle = new DynamicParameters(item);
                    parametersDetalle.Add("@idMaestro", id);
                    await _connection.ExecuteAsync(queryDetalle, parametersDetalle, transaction: transaction, commandType: CommandType.Text).ConfigureAwait(false);
                }

                transaction.Commit();
                _connection.Close();
                return id;
            }
            catch (Exception)
            {

                transaction.Rollback();
                _connection.Close();
                return 0;

            }
        }
        protected async Task<IEnumerable<T>> ExecuteQuery<T>(string query, DynamicParameters parameters = null, bool iSprocedure = false, bool withParameters = false)
        {
            _connection.Open();
            var objetoreturn = await _connection.QueryAsync<T>(query, param: withParameters ? parameters : null,
              commandType: iSprocedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(false);
            _connection.Close();
            return objetoreturn;
        }
        protected async Task<T> ExecuteFirst<T>(string query, DynamicParameters parameters = null, bool iSprocedure = false, bool withParameters = false)
        {
            _connection.Open();
            var objetoreturn = await _connection.QuerySingleAsync<T>(query, param: withParameters ? parameters : null,
                 commandType: iSprocedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(false);
            _connection.Close();

            return objetoreturn;
        }

        protected async Task<long> ExecuteIdentity(string query, DynamicParameters parameters = null, bool iSprocedure = false, bool withParameters = false)
        {
            _connection.Close();

            query = query.ToUpper().Contains("INSERT INTO") ? query + ";select cast(SCOPE_IDENTITY() as bigint)" : query;
            var objetoreturn = await _connection.QuerySingleAsync<int>(query, param: withParameters ? parameters : null,
                                commandType: iSprocedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(false);
            _connection.Close();
            return objetoreturn;

        }
        protected async Task<int> ExecuteSingle(string query, DynamicParameters parameters = null, bool iSprocedure = false, bool withParameters = false)
        {
            _connection.Open();
            var objetoreturn = await _connection.ExecuteAsync(query, param: withParameters ? parameters : null,
                commandType: iSprocedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(false);
            _connection.Close();
            return objetoreturn;
        }
    }
}
