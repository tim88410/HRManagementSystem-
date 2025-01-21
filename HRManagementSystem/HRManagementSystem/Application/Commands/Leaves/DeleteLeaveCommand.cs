using MediatR;

namespace HRManagementSystem.Application.Commands.Leaves
{
    public class DeleteLeaveCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int Id { get; set; }
    }
}
