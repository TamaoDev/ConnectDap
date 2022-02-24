using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectDap.Lib.Interfaces
{
    public interface IAction<T> where T : class 
    {
        Task<IEnumerable<T>> Select();
        Task<T> Find();
        Task<long> AddOrUpdate(T collection);
        Task<int> Remove();
    }
}
