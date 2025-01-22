using DBUtility;
using HRManagementSystem.Domain.SeedWork;
using HRManagementSystem.Infrastructure.Models.Leaves;
using System.Data;
using System.Data.SqlClient;

namespace HRManagementSystem.Infrastructure.Repositories.Leaves
{
    public interface ILeavesQueryRepository
    {
        public Task<IEnumerable<LeavesQuery.LeavesDTO>> GetAsync(LeavesQuery.LeavesQueryParameter request);

        public Task<IEnumerable<Domain.AggregatesModel.LeaveAggregate.Leave>> GetOneAsync(int Id);
    }
    public class LeavesQueryRepository : IRepository<Domain.AggregatesModel.LeaveAggregate.Leave>, ILeavesQueryRepository
    {

        private readonly IDataBaseUtility dataBaseUtility;
        public LeavesQueryRepository(IDataBaseUtility dataBaseUtility) 
        {
            this.dataBaseUtility = dataBaseUtility;
        }

        public async Task<IEnumerable<LeavesQuery.LeavesDTO>> GetAsync(LeavesQuery.LeavesQueryParameter request)
        {
            string sql = @"
WITH LeavesSort AS
(                               
    SELECT 
        Id,
		LeaveName,
		Description,
		LeaveLimitHours,
		CreateDate,
		OperateUserId,
        CASE @SortBy
			WHEN 'Id ASC' THEN ROW_NUMBER() OVER (ORDER BY Id)
			WHEN 'Id DESC' THEN ROW_NUMBER() OVER (ORDER BY Id DESC)
			WHEN 'LeaveName ASC' THEN ROW_NUMBER() OVER (ORDER BY LeaveName)
			WHEN 'LeaveName DESC' THEN ROW_NUMBER() OVER (ORDER BY LeaveName DESC)
			WHEN 'Description ASC' THEN ROW_NUMBER() OVER (ORDER BY Description)
			WHEN 'Description DESC' THEN ROW_NUMBER() OVER (ORDER BY Description DESC)
			WHEN 'LeaveLimitHours ASC' THEN ROW_NUMBER() OVER (ORDER BY LeaveLimitHours)
			WHEN 'LeaveLimitHours DESC' THEN ROW_NUMBER() OVER (ORDER BY LeaveLimitHours DESC)
			WHEN 'CreateDate ASC' THEN ROW_NUMBER() OVER (ORDER BY CreateDate)
			WHEN 'CreateDate DESC' THEN ROW_NUMBER() OVER (ORDER BY CreateDate DESC)
		END AS RowNo
    FROM
        (
            SELECT
				Id,
				LeaveName,
				Description,
				LeaveLimitHours,
				CreateDate,
				OperateUserId
            FROM
                Leave
            WHERE
                (@LeaveName = '' OR Leave.LeaveName = @LeaveName)
		) LeavesInfo
), LeavesCount AS (SELECT COUNT(1) AS TotalItem FROM LeavesSort)

SELECT     
	Id,
	LeaveName,
	Description,
	LeaveLimitHours,
	CreateDate,
	OperateUserId,
	TotalItem
FROM        
	LeavesSort, LeavesCount
WHERE 
	RowNo between @StartIndex and @EndIndex
ORDER BY 
	RowNo
";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "LeaveName", Value = request.LeaveName ?? string.Empty, SqlDbType = SqlDbType.NVarChar },
                new SqlParameter { ParameterName = "StartIndex", Value = request.StartIndex, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "EndIndex", Value = request.EndIndex, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "SortBy", Value = request.SortBy ?? string.Empty, SqlDbType = SqlDbType.VarChar }
            };

            return await dataBaseUtility.QueryAsync<LeavesQuery.LeavesDTO>(sql, sqlParams);
        }
        public async Task<IEnumerable<Domain.AggregatesModel.LeaveAggregate.Leave>> GetOneAsync(int Id)
        {
            string sql = @"
SELECT     
    Id,
    LeaveName,
    Description,
    LeaveLimitHours,
    OperateUserId,
    CreateDate
FROM        
	Leave
WHERE 
	Id = @Id
";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "Id", Value = Id, SqlDbType = SqlDbType.Int }
            };

            return await dataBaseUtility.QueryAsync<Domain.AggregatesModel.LeaveAggregate.Leave>(sql, sqlParams);
        }
    }
}
