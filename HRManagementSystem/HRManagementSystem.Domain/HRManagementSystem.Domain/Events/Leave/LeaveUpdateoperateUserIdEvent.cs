using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.Domain.Events.Leave
{
    public class LeaveUpdateoperateUserIdEvent : INotification
    {
        public LeaveUpdateoperateUserIdEvent(int LeaveId,
                                   int oldOperateUserId,
                                   int newOperateUserId)
        {
            this.LeaveId = LeaveId;
            this.OldOperateUserId = oldOperateUserId;
            this.NewOperateUserId = newOperateUserId;
        }
        public int LeaveId { get; set; }
        public int OldOperateUserId { get; set; }
        public int NewOperateUserId { get; set; }
    }
}
