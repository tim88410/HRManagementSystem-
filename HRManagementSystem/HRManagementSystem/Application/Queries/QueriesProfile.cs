using AutoMapper;
using HRManagementSystem.Application.Queries.Leaves;
using HRManagementSystem.Infrastructure.Models.Leaves;

namespace HRManagementSystem.Application.Queries
{
    public class QueriesProfile : Profile
    {
        public QueriesProfile()
        {

            CreateMap<LeavesResponse.LeavesInfo, LeavesQuery.LeavesDTO>().ReverseMap();
            CreateMap<LeavesRequest, LeavesQuery.LeavesQueryParameter>().ReverseMap();

        }
    }
}
