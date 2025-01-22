using AutoMapper;
using DBUtility;
using HRManagementSystem.Application.Commands.Leaves;
using HRManagementSystem.Application.Queries.Leaves;
using HRManagementSystem.Infrastructure.Models.Leaves;
using HRManagementSystem.Infrastructure.Repositories.Leaves;
using MediatR;
using Moq;

namespace HRManagementSystemTest.Application.Commands.Leaves
{
    public class DeleteLeaveHandlerTest
    {

        private class FakeLeaveAggregateRepositoryDeleteSuccessful : ILeaveAggregateRepository
        {
            //本次測試未用到
            public Task<int> Upsert(HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave leave)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
            public Task<int> DeleteAsync(int Id)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
        }

        private class FakeLeaveAggregateRepositoryDeleteFailed : ILeaveAggregateRepository
        {
            //本次測試未用到
            public Task<int> Upsert(HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave leave)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
            public Task<int> DeleteAsync(int Id)
            {
                return Task.FromResult(ErrorCode.KErrDBError);
            }
        }


        private class FakeLeavesQueryRepositoryGetOneAsyncSuccessful : ILeavesQueryRepository
        {
            public Task<IEnumerable<LeavesQuery.LeavesDTO>> GetAsync(LeavesQuery.LeavesQueryParameter request)
            {
                List<LeavesQuery.LeavesDTO> leavesQuery = new List<LeavesQuery.LeavesDTO> { new LeavesQuery.LeavesDTO {
                    Id = 1,
                    LeaveName = "事假",
                    Description = "最小請假單位半小時,不分年資",
                    LeaveLimitHours = 112,
                    CreateDate = DateTime.Now,
                    OperateUserId = 1,
                    TotalItem = 1
                }};
                IEnumerable<LeavesQuery.LeavesDTO> test = leavesQuery;
                return Task.FromResult(test);
            }

            public Task<IEnumerable<HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave>> GetOneAsync(int Id)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave, LeavesResponse.LeavesInfo>().ReverseMap();
                });

                var mapper = config.CreateMapper();
                List<LeavesResponse.LeavesInfo> leavesQuery = new List<LeavesResponse.LeavesInfo> { new LeavesResponse.LeavesInfo {
                    Id = 1,
                    LeaveName = "事假",
                    Description = "最小請假單位半小時,不分年資",
                    LeaveLimitHours = 112,
                    CreateDate = DateTime.Now,
                    OperateUserId = 1
                }};

                var test = mapper.Map<IEnumerable<HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave>>(leavesQuery.ToList());
                return Task.FromResult(test);
            }
        }

        private class FakeLeavesQueryRepositoryGetOneAsyncIsNull : ILeavesQueryRepository
        {
            public Task<IEnumerable<LeavesQuery.LeavesDTO>> GetAsync(LeavesQuery.LeavesQueryParameter request)
            {
                List<LeavesQuery.LeavesDTO> leavesQuery = new List<LeavesQuery.LeavesDTO> { new LeavesQuery.LeavesDTO {
                    Id = 1,
                    LeaveName = "事假",
                    Description = "最小請假單位半小時,不分年資",
                    LeaveLimitHours = 112,
                    CreateDate = DateTime.Now,
                    OperateUserId = 1,
                    TotalItem = 1
                }};
                IEnumerable<LeavesQuery.LeavesDTO> test = leavesQuery;
                return Task.FromResult(test);
            }

            public Task<IEnumerable<HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave>> GetOneAsync(int Id)
            {

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave, LeavesResponse.LeavesInfo>().ReverseMap();
                });

                var mapper = config.CreateMapper();
                List<LeavesResponse.LeavesInfo> leavesQuery = new List<LeavesResponse.LeavesInfo> { };

                var test = mapper.Map<IEnumerable<HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave>>(leavesQuery.ToList());
                return Task.FromResult(test);
            }
        }

        [Fact]
        public async Task DeleteLeaveIdIsZero()
        {
            var mediatorMock = new Mock<IPublisher>();
            DeleteLeaveCommandHandler deleteLeaveHandler = new DeleteLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryDeleteSuccessful(),
                new FakeLeavesQueryRepositoryGetOneAsyncIsNull(),
                mediatorMock.Object
            );

            var result = await deleteLeaveHandler.Handle(new DeleteLeaveCommand
            {
                Id = 0,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal((int)ErrorCode.ReturnCode.OperationFailed, result);
        }

        [Fact]
        public async Task DeleteLeaveIdNotFound()
        {
            var mediatorMock = new Mock<IPublisher>();
            DeleteLeaveCommandHandler deleteLeaveHandler = new DeleteLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryDeleteSuccessful(),
                new FakeLeavesQueryRepositoryGetOneAsyncIsNull(),
                mediatorMock.Object
            );

            var result = await deleteLeaveHandler.Handle(new DeleteLeaveCommand
            {
                Id = 1,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal((int)ErrorCode.ReturnCode.DataNotFound, result);
        }

        [Fact]
        public async Task DeleteLeaveIdDBFailed()
        {
            var mediatorMock = new Mock<IPublisher>();
            DeleteLeaveCommandHandler deleteLeaveHandler = new DeleteLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryDeleteFailed(),
                new FakeLeavesQueryRepositoryGetOneAsyncSuccessful(),
                mediatorMock.Object
            );

            var result = await deleteLeaveHandler.Handle(new DeleteLeaveCommand
            {
                Id = 1,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal((int)ErrorCode.ReturnCode.DBConnectError, result);
        }

        [Fact]
        public async Task DeleteLeaveIdSuccessful()
        {
            var mediatorMock = new Mock<IPublisher>();
            DeleteLeaveCommandHandler deleteLeaveHandler = new DeleteLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryDeleteSuccessful(),
                new FakeLeavesQueryRepositoryGetOneAsyncSuccessful(),
                mediatorMock.Object
            );

            var result = await deleteLeaveHandler.Handle(new DeleteLeaveCommand
            {
                Id = 1,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrNone, result);
        }
    }
}
