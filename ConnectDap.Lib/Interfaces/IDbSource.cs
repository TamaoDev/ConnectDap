using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace ConnectDap.Lib.Interfaces
{
    public interface IDbSource
    {

        Task<IEnumerable<T>> SelectData<T>(string query);
        Task<IEnumerable<T>> SelectData<T>(string query, DynamicParameters parameters);
        Task<IEnumerable<T>> SelectSp<T>(string procedure, DynamicParameters parameters);

        Task<T> FilterData<T>(string query);

        Task<T> FilterData<T>(string query, DynamicParameters parameters);
        Task<T> FilterSp<T>(string procedure, DynamicParameters parameters);

        Task<long> PostData(string query);
        Task<long> PostData(string query, DynamicParameters parameters);
        Task<long> PostSp(string procedure, DynamicParameters parameters);

        Task<long> PutData(string query);
        Task<long> PutData(string query, DynamicParameters parameters);
        Task<long> PutSp(string procedure, DynamicParameters parameters);

        Task<int> RemoveData(string query);
        Task<int> RemoveData(string query, DynamicParameters parameters);
        Task<int> RemoveSp(string procedure, DynamicParameters parameters);
    }
}
