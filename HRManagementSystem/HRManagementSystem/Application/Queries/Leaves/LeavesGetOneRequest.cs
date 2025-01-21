using MediatR;

namespace HRManagementSystem.Application.Queries.Leaves
{
    public class LeavesGetOneRequest : IRequest<IEnumerable<LeavesResponse.LeavesInfo>?>
    {

        /// <summary>
        /// 查詢對象流水號
        /// </summary>
        public int Id { get; set; }
    }
}
