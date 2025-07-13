using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Application.Commands.Auth
{
    public class AuthDeleteCommand : IRequest<AuthDeleteResponse>
    {
        [Required]
        public string Username { get; set; } = string.Empty;
    }
    public class AuthDeleteResponse
    {
        public int ReturnCode { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
