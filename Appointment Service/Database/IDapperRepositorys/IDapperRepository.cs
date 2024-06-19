using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace Appointment_Service.Database.IDapperRepositorys
{
    public interface IDapperRepository
    {
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null);
        Task<T> QuerySingleAsync<T>(string sql, object parameters = null);
        Task<T> GetAsync<T>(string sql, object parameters = null);
        Task<IEnumerable<T>> GetAllAsync<T>(string sql, object parameters = null);
        Task<int> ExecuteAsync(string sql, object parameters = null, CommandType? commandType = null);
        //Methods for Stored Procedure
        Task<T> ExecuteStoredProcedureAsync<T>(string storedProcedureName, object parameters = null);
        

    }
}
