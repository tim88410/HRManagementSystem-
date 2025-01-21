using AutoMapper;
using HRManagementSystem.Infrastructure.Models.Leaves;
using HRManagementSystem.Infrastructure.Repositories.Leaves;
using MediatR;

namespace HRManagementSystem.Application.Queries.Leaves
{
    public class LeavesHandler : IRequestHandler<LeavesRequest, LeavesResponse?>
    {
        private readonly ILeavesQueryRepository leavesQueryRepository;
        private readonly IMapper mapper;
        public LeavesHandler(ILeavesQueryRepository leavesQueryRepository,
            IMapper mapper)
        {
            this.leavesQueryRepository = leavesQueryRepository;
            this.mapper = mapper;
        }

        public async Task<LeavesResponse?> Handle(LeavesRequest request, CancellationToken cancellationToken)
        {

            var leavesPara = mapper.Map<LeavesQuery.LeavesQueryParameter>(request);
            var leavesQuery = await leavesQueryRepository.GetAsync(leavesPara);
            //DBConnectError
            if (leavesQuery == null)
            {
                return null;
            }

            return new LeavesResponse
            {
                LeavesInfos = mapper.Map<List<LeavesResponse.LeavesInfo>>(leavesQuery.ToList()),
                Total = leavesQuery.Select(s => s.TotalItem).FirstOrDefault()
            };
        }
    }
}
