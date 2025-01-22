using MediatR;

namespace HRManagementSystem.Domain.Events.Leave
{
    public class LeaveActionLogEvent : INotification
    {
        public LeaveActionLogEvent(int leaveId, string action, string oldValue, string newValue, int operatorId)
        {
            LeaveId = leaveId;
            Action = action;
            OldValue = oldValue;
            NewValue = newValue;
            OperatorId = operatorId;
        }

        public int LeaveId { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int OperatorId { get; set; }
    }
}
