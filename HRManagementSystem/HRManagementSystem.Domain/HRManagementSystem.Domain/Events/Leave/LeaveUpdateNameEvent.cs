using MediatR;

namespace HRManagementSystem.Domain.Events.Leave
{
    public class LeaveUpdateNameEvent : INotification
    {
        public LeaveUpdateNameEvent(int LeaveId,
                                   string oldName,
                                   string newName)
        {
            this.LeaveId = LeaveId;
            this.OldName = oldName;
            this.NewName = newName;
        }
        public int LeaveId { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}
