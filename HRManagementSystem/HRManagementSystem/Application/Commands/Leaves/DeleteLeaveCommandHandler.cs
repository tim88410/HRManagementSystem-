using DBUtility;
using HRManagementSystem.Domain.AggregatesModel.LeaveAggregate;
using HRManagementSystem.Domain.Events.Leave;
using HRManagementSystem.Domain.SeedWork;
using HRManagementSystem.Infrastructure.Repositories.Leaves;
using MediatR;

namespace HRManagementSystem.Application.Commands.Leaves
{
    public class DeleteLeaveCommandHandler : IRequestHandler<DeleteLeaveCommand, int>
    {
        private readonly ILeaveAggregateRepository leaveAggregateRepository;
        private readonly ILeavesQueryRepository leavesQueryRepository;
        private readonly IPublisher publisher;
        public DeleteLeaveCommandHandler(
            ILeaveAggregateRepository leaveAggregateRepository, 
            ILeavesQueryRepository leavesQueryRepository,
            IPublisher publisher
            )
        {
            this.leaveAggregateRepository = leaveAggregateRepository;
            this.leavesQueryRepository = leavesQueryRepository;
            this.publisher = publisher;
        }
        
        public async Task<int> Handle(DeleteLeaveCommand command, CancellationToken cancellationToken)
        {
            List<Outcome> results = new List<Outcome>();
            Leave leave;

            if (command.Id == 0)
            {
                return (int)ErrorCode.ReturnCode.OperationFailed;
            }

            var getResult = await leavesQueryRepository.GetOneAsync(command.Id);
            if (!getResult.Any())
            {
                return (int)ErrorCode.ReturnCode.DataNotFound;
            }
            leave = getResult.First();

            var deleteResult = await leaveAggregateRepository.DeleteAsync(leave.Id);
            if (deleteResult == ErrorCode.KErrDBError)
            {
                return (int)ErrorCode.ReturnCode.DBConnectError;
            }

            leave.AddDomainEvent(new LeaveActionLogEvent(command.Id, "DeleteLeave", string.Empty, string.Empty, command.UserId));
            await publisher.Publish(leave.DomainEvents.First(), cancellationToken);

            return ErrorCode.KErrNone;
        }
    }
}
