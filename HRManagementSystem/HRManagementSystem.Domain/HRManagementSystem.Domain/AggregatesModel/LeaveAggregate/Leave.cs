using HRManagementSystem.Domain.Events.Leave;
using HRManagementSystem.Domain.SeedWork;
using static HRManagementSystem.Domain.SeedWork.DomainError;

namespace HRManagementSystem.Domain.AggregatesModel.LeaveAggregate
{
    public class Leave : Entity, IAggregateRoot
    {
        public int Id { get; private set; }
        public string LeaveName { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public double LeaveLimitHours { get; private set; }
        public int OperateUserId { get; private set; }

        public Leave()
        {

        }

        public static Leave CreateLeave()
        {
            return InitLeave();
        }

        private static Leave InitLeave()
        {
            return new Leave()
            {
                Id = 0
            };
        }

        public class LeaveDomainEvent
        {
            public int LeaveId { get; set; }
            public string Action { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public int OperatorId { get; set; }
        }

        public Outcome UpdateLeaveName(string leaveName)
        {
            if (string.IsNullOrEmpty(leaveName))
            {
                return Outcome.Fail(LeaveDomainError.Leave_Name);
            }
            if (string.Compare(LeaveName, leaveName) != 0)
            {
                AddDomainEvent(new LeaveUpdateNameEvent(Id, LeaveName, leaveName));
                LeaveName = leaveName;
            }
            return Outcome.Success();
        }

        public Outcome UpdateLeaveDescription(string leaveDescription)
        {
            if (string.Compare(Description, leaveDescription) != 0)
            {
                AddDomainEvent(new LeaveUpdateDescriptionEvent(Id, Description, leaveDescription));
                Description = leaveDescription;
            }
            return Outcome.Success();
        }

        public Outcome UpdateLeaveLimitHours(double leaveLimitHours)
        {
            if (leaveLimitHours == 0)
            {
                return Outcome.Fail(LeaveDomainError.Leave_LeaveLimitHours);
            }
            if (LeaveLimitHours != leaveLimitHours)
            {
                AddDomainEvent(new LeaveUpdateLeaveLimitHoursEvent(Id, LeaveLimitHours, leaveLimitHours));
                LeaveLimitHours = leaveLimitHours;
            }
            return Outcome.Success();
        }

        public Outcome UpdateOperateUserId(int operateUserId)
        {
            if (operateUserId == 0)
            {
                return Outcome.Fail(LeaveDomainError.Leave_OperateUserId);
            }
            if (OperateUserId != operateUserId)
            {
                AddDomainEvent(new LeaveUpdateoperateUserIdEvent(Id, OperateUserId, operateUserId));
                OperateUserId = operateUserId;
            }
            return Outcome.Success();
        }

    }
}
