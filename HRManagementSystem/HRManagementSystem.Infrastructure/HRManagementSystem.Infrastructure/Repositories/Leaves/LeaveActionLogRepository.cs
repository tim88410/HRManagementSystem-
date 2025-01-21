using DBUtility;
using System.Data;
using System.Data.SqlClient;
using static HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave;

namespace HRManagementSystem.Infrastructure.Repositories.Leaves
{
    public interface ILeaveActionLogRepository
    {
        public Task<int> InsertAsync(LeaveDomainEvent leave);
    }
    public class LeaveActionLogRepository : ILeaveActionLogRepository
    {
        private readonly IDataBaseUtility dataBaseUtility;
        public LeaveActionLogRepository(IDataBaseUtility dataBaseUtility)
        {
            this.dataBaseUtility = dataBaseUtility;
        }

        public async Task<int> InsertAsync(LeaveDomainEvent leave)
        {
            string sql = @"
INSERT INTO [LeaveActionLog]
(
    [LeaveId],
    [Action],
    [OldValue],
    [NewValue],
    [OperatorId]
)
VALUES
(
    @LeaveId,
    @Action,
    @OldValue,
    @NewValue,
    @OperatorId
)
";
            List<SqlParameter> sqlParams = new()
            {
                new SqlParameter("LeaveId", leave.LeaveId) { SqlDbType = SqlDbType.Int },
                new SqlParameter("Action", leave.Action) { SqlDbType = SqlDbType.VarChar },
                new SqlParameter("OldValue", leave.OldValue) { SqlDbType = SqlDbType.NVarChar },
                new SqlParameter("NewValue", leave.NewValue) { SqlDbType = SqlDbType.NVarChar },
                new SqlParameter("OperatorId", leave.OperatorId) { SqlDbType = SqlDbType.Int }
            };
            return await dataBaseUtility.UpdateAsync(sql, sqlParams, CommandType.Text);
        }
    }
}
