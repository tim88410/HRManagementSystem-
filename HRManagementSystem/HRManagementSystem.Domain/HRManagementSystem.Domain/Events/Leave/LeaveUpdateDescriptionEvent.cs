using MediatR;

namespace HRManagementSystem.Domain.Events.Leave
{
    public class LeaveUpdateDescriptionEvent : INotification
    {
        public LeaveUpdateDescriptionEvent(int LeaveId,
                                   string oldDescription,
                                   string newDescription)
        {
            this.LeaveId = LeaveId;
            this.OldDescription = oldDescription;
            this.NewDescription = newDescription;
        }
        public int LeaveId { get; set; }
        public string OldDescription { get; set; }
        public string NewDescription { get; set; }
    }
}
