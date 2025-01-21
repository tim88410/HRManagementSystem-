using DBUtility;
using HRManagementSystem.Domain.SeedWork;
using System.Data.SqlClient;

namespace HRManagementSystem.Infrastructure.Repositories.Leaves
{
    public interface ILeaveAggregateRepository
    {
        public Task<int> Upsert(Domain.AggregatesModel.LeaveAggregate.Leave leave);
        public Task<int> DeleteAsync(int Id);
    }
    public class LeaveAggregateRepository : IRepository<Domain.AggregatesModel.LeaveAggregate.Leave>, ILeaveAggregateRepository
    {
        private readonly IDataBaseUtility dataBaseUtility;

        public LeaveAggregateRepository(IDataBaseUtility dataBaseUtility)
        {
            this.dataBaseUtility = dataBaseUtility;
        }

        public async Task<int> Upsert(Domain.AggregatesModel.LeaveAggregate.Leave leave)
        {
            string sql = @"
MERGE INTO Leave AS T
USING (
    SELECT 
		@Id AS Id,
		@LeaveName AS LeaveName,
		@Description AS Description,
		@LeaveLimitHours AS LeaveLimitHours,
		@OperateUserId AS OperateUserId
) AS S ON T.Id = S.Id
WHEN MATCHED THEN
    UPDATE SET
        T.LeaveName = S.LeaveName,
        T.Description = S.Description, 
        T.LeaveLimitHours = S.LeaveLimitHours, 
		T.OperateUserId = S.OperateUserId
WHEN NOT MATCHED THEN
    INSERT (LeaveName, Description, LeaveLimitHours, OperateUserId)
    VALUES (S.LeaveName, S.Description, S.LeaveLimitHours, S.OperateUserId);
";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = leave.Id },
                new SqlParameter("LeaveName", System.Data.SqlDbType.NVarChar) { Value = leave.LeaveName },
                new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = leave.Description },
                new SqlParameter("LeaveLimitHours", System.Data.SqlDbType.Decimal) { Value = leave.LeaveLimitHours },
                new SqlParameter("OperateUserId", System.Data.SqlDbType.Int) { Value = leave.OperateUserId }
            };

            return await dataBaseUtility.UpdateAsync(sql, sqlParams);
        }

        public async Task<int> DeleteAsync(int Id)
        {
            string sql = @"
DELETE FROM Leave WHERE Id = @Id
";
            List<SqlParameter> sqlParameter = new()
            {
                new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = Id }
            };

            return await dataBaseUtility.DeleteAsync(sql, sqlParameter);
        }
    }
}
