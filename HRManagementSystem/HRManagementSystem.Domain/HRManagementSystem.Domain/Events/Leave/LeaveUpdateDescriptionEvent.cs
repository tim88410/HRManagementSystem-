using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
