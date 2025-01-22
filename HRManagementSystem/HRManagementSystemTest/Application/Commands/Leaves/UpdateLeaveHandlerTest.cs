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
    public class UpdateLeaveHandlerTest
    {
        private class FakeLeaveAggregateRepositoryUpdateSuccessful : ILeaveAggregateRepository
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

        private class FakeLeaveAggregateRepositoryUpdateFailed : ILeaveAggregateRepository
        {
            //本次測試未用到
            public Task<int> Upsert(HRManagementSystem.Domain.AggregatesModel.LeaveAggregate.Leave leave)
            {
                return Task.FromResult(ErrorCode.KErrDBError);
            }
            public Task<int> DeleteAsync(int Id)
            {
                return Task.FromResult(ErrorCode.KErrNone);
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
        public async Task UpdateLeaveNotFound()
        {
            var mediatorMock = new Mock<IPublisher>();
            UpdateLeaveCommandHandler updateLeaveHandler = new UpdateLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryUpdateSuccessful(),
                new FakeLeavesQueryRepositoryGetOneAsyncIsNull(),
                mediatorMock.Object
            );

            var result = await updateLeaveHandler.Handle(new UpdateLeaveCommand
            {
                Id = 1,
                LeaveName = "test",
                Description = "test",
                LeaveLimitHours = 80,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal((int)ErrorCode.ReturnCode.DataNotFound, result);
        }

        [Fact]
        public async Task UpdateLeaveNameFailed()
        {
            var mediatorMock = new Mock<IPublisher>();
            var mapperMock = new Mock<IMapper>();
            UpdateLeaveCommandHandler updateLeaveHandler = new UpdateLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryUpdateSuccessful(),
                new FakeLeavesQueryRepositoryGetOneAsyncSuccessful(),
                mediatorMock.Object
            );

            var result = await updateLeaveHandler.Handle(new UpdateLeaveCommand
            {
                Id = 1,
                LeaveName = "",
                Description = "test",
                LeaveLimitHours = 80,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal((int)ErrorCode.ReturnCode.OperationFailed, result);
        }

        [Fact]
        public async Task InsertLeaveNameFailed()
        {
            var mediatorMock = new Mock<IPublisher>();
            var mapperMock = new Mock<IMapper>();
            UpdateLeaveCommandHandler updateLeaveHandler = new UpdateLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryUpdateSuccessful(),
                new FakeLeavesQueryRepositoryGetOneAsyncSuccessful(),
                mediatorMock.Object
            );

            var result = await updateLeaveHandler.Handle(new UpdateLeaveCommand
            {
                Id = 0,
                LeaveName = "",
                Description = "test",
                LeaveLimitHours = 80,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal((int)ErrorCode.ReturnCode.OperationFailed, result);
        }

        [Fact]
        public async Task UpdateLeaveUpsertDBFailed()
        {
            var mediatorMock = new Mock<IPublisher>();
            var mapperMock = new Mock<IMapper>();
            UpdateLeaveCommandHandler updateLeaveHandler = new UpdateLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryUpdateFailed(),
                new FakeLeavesQueryRepositoryGetOneAsyncSuccessful(),
                mediatorMock.Object
            );

            var result = await updateLeaveHandler.Handle(new UpdateLeaveCommand
            {
                Id = 1,
                LeaveName = "test",
                Description = "test",
                LeaveLimitHours = 80,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrDBError, result);
        }
        [Fact]
        public async Task UpdateLeaveUpsertSuccessful()
        {
            var mediatorMock = new Mock<IPublisher>();
            var mapperMock = new Mock<IMapper>();
            UpdateLeaveCommandHandler updateLeaveHandler = new UpdateLeaveCommandHandler(
                new FakeLeaveAggregateRepositoryUpdateSuccessful(),
                new FakeLeavesQueryRepositoryGetOneAsyncSuccessful(),
                mediatorMock.Object
            );

            var result = await updateLeaveHandler.Handle(new UpdateLeaveCommand
            {
                Id = 1,
                LeaveName = "test",
                Description = "test",
                LeaveLimitHours = 80,
                UserId = 1
            }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrNone, result);
        }
    }
}
