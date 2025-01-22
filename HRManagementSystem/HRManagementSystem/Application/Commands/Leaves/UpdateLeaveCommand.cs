using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Application.Commands.Leaves
{
    public class UpdateLeaveCommand : IRequest<int>
    {
        public int UserId { get; set; }
        [Required]
        /// <summary>
        /// LeaveId
        /// </summary>
        public int Id { get; set; }
        public string LeaveName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double LeaveLimitHours { get; set; }
    }
}
