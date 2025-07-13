using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Application.Commands.Auth
{
    public class AuthRegisterCommand : IRequest<AuthRegisterResponse>
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
    public class AuthRegisterResponse
    {
        public int ReturnCode { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>(); 
    }
}
