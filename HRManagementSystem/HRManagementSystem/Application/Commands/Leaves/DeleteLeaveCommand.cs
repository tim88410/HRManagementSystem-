using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Application.Commands.Leaves
{
    public class DeleteLeaveCommand : IRequest<int>
    {
        public int UserId { get; set; }
        [Required]
        public int Id { get; set; }
    }
}
