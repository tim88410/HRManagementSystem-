using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace DBUtility
{
    public interface IDataBaseUtility 
    {
        public Task<IEnumerable<T>> QueryAsync<T>(string sqlcmd, List<SqlParameter> sqlParameters,
            CommandType commandType = CommandType.Text);
        public Task<int> UpdateAsync(string sqlcmd, List<SqlParameter> sqlParameters, CommandType commandType = CommandType.Text);
        public Task<int> DeleteAsync(string sqlcmd, List<SqlParameter> sqlParameters, CommandType commandType = CommandType.Text);
    }
    public class DataBaseUtility : IDataBaseUtility
    {
        private readonly string connectionString = string.Empty;
        public DataBaseUtility(string connectionString)
        {
            this.connectionString = connectionString;
        }
        private DynamicParameters SqlParametersToDynamicParameters(List<SqlParameter> parameters)
        {
            var args = new DynamicParameters(new { });
            parameters.ForEach(p =>
            {
                if (p.SqlDbType == SqlDbType.Structured && p.Value is DataTable dataTable)
                {
                    args.Add(p.ParameterName, dataTable.AsTableValuedParameter(p.TypeName));
                }
                else
                {
                    args.Add(p.ParameterName, p.Value, p.DbType);
                }
            });

            return args;
        }



        public async Task<IEnumerable<T>> QueryAsync<T>(string sqlcmd, List<SqlParameter> sqlParameters, CommandType commandType = CommandType.Text)
        {
            try
            {
                string connectionString = this.connectionString;

                using var conn = new SqlConnection(connectionString);
                return await conn.QueryAsync<T>(sqlcmd, SqlParametersToDynamicParameters(sqlParameters), commandType: commandType);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return null;
#endif
            }
        }

        public async Task<int> UpdateAsync(string sqlcmd, List<SqlParameter> sqlParameters, CommandType commandType = CommandType.Text)
        {
            int ret = ErrorCode.KErrNone;
            try
            {
                string connectionString = this.connectionString;
                using var conn = new SqlConnection(connectionString);
                _ = await conn.ExecuteAsync(sqlcmd, SqlParametersToDynamicParameters(sqlParameters), commandType: commandType);
                return ret;
            }
            catch (Exception ex)
            {
            }
            return await Task.FromResult(ErrorCode.KErrDBError);

        }

        public async Task<int> DeleteAsync(string sqlcmd, List<SqlParameter> sqlParameters, CommandType commandType = CommandType.Text)
        {
            int ret = ErrorCode.KErrNone;
            try
            {
                string connectionString = this.connectionString;
                using var conn = new SqlConnection(connectionString);
                _ = await conn.ExecuteAsync(sqlcmd, SqlParametersToDynamicParameters(sqlParameters), commandType: commandType);

                return ret;
            }
            catch (Exception ex)
            {
            }
            return await Task.FromResult(ErrorCode.KErrDBError);
        }
    }
}
