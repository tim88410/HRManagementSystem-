using DBUtility;
using HRManagementSystem.Domain.AggregatesModel.LeaveAggregate;
using HRManagementSystem.Domain.Events.Leave;
using HRManagementSystem.Domain.SeedWork;
using HRManagementSystem.Infrastructure.Repositories.Leaves;
using MediatR;
using Newtonsoft.Json;

namespace HRManagementSystem.Application.Commands.Leaves
{
    public class UpdateLeaveCommandHandler : IRequestHandler<UpdateLeaveCommand, int>
    {
        private readonly ILeaveAggregateRepository leaveAggregateRepository;
        private readonly ILeavesQueryRepository leavesQueryRepository;
        private readonly IPublisher publisher;
        public UpdateLeaveCommandHandler(
            ILeaveAggregateRepository leaveAggregateRepository,
            ILeavesQueryRepository leavesQueryRepository,
            IPublisher publisher
            )
        {
            this.leaveAggregateRepository = leaveAggregateRepository;
            this.leavesQueryRepository = leavesQueryRepository;
            this.publisher = publisher;
        }

        public async Task<int> Handle(UpdateLeaveCommand command, CancellationToken cancellationToken)
        {
            List<Outcome> results = new List<Outcome>();
            string oldLeaveJson = string.Empty;
            Leave leave;

            // 自訂序列化設定
            var settings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.Default,
                Formatting = Formatting.Indented
            };

            if (command.Id == 0)
            {
                leave = Leave.CreateLeave();
            }
            else
            {
                var getResult = await leavesQueryRepository.GetOneAsync(command.Id);
                if (!getResult.Any())
                {
                    return (int)ErrorCode.ReturnCode.DataNotFound;
                }
                leave = getResult.First();
                oldLeaveJson =JsonConvert.SerializeObject(new
                                                {
                                                    LeaveId = leave.Id,
                                                    leave.LeaveName,
                                                    leave.Description,
                                                    leave.LeaveLimitHours,
                                                    leave.OperateUserId
                                                }, 
                                            settings);
            }

            results.Add(leave.UpdateLeaveName(command.LeaveName));
            results.Add(leave.UpdateLeaveDescription(command.Description));
            results.Add(leave.UpdateLeaveLimitHours(command.LeaveLimitHours));
            results.Add(leave.UpdateOperateUserId(command.UserId));

            if (results.Any(a => a.Failure))
            {
                return (int)ErrorCode.ReturnCode.OperationFailed;
            }

            if (leave.DomainEvents != null)
            {
                var dbResult = await leaveAggregateRepository.Upsert(leave);

                if (dbResult != ErrorCode.KErrNone)
                {
                    return ErrorCode.KErrDBError;
                }

                leave.AddDomainEvent(new LeaveActionLogEvent(
                    command.Id,
                    "UpsertLeave",
                    oldLeaveJson,
                    JsonConvert.SerializeObject(new
                    {
                        LeaveId = leave.Id,
                        leave.LeaveName,
                        leave.Description,
                        leave.LeaveLimitHours,
                        leave.OperateUserId
                    }, settings),
                    command.UserId)
                );
                foreach (var events in leave.DomainEvents)
                {
                    await publisher.Publish(events, cancellationToken);
                }
            }

            return ErrorCode.KErrNone;
        }
    }
}
