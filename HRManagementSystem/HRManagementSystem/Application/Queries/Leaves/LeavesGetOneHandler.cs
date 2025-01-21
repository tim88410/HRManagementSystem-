using AutoMapper;
using HRManagementSystem.Infrastructure.Repositories.Leaves;
using MediatR;

namespace HRManagementSystem.Application.Queries.Leaves
{
    public class LeavesGetOneHandler : IRequestHandler<LeavesGetOneRequest, IEnumerable<LeavesResponse.LeavesInfo>?>
    {
        private readonly ILeavesQueryRepository leavesQueryRepository;
        private readonly IMapper mapper;
        public LeavesGetOneHandler(ILeavesQueryRepository leavesQueryRepository, IMapper mapper)
        {
            this.leavesQueryRepository = leavesQueryRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<LeavesResponse.LeavesInfo>?> Handle(LeavesGetOneRequest request, CancellationToken cancellationToken)
        {
            var coindeskQuery = await leavesQueryRepository.GetOneAsync(request.Id);

            if (coindeskQuery == null)
            {
                return null;
            }
            return mapper.Map<IEnumerable<LeavesResponse.LeavesInfo>>(coindeskQuery.ToList());
        }
    }
}
