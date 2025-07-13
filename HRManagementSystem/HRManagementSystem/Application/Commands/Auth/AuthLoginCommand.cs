using MediatR;

namespace HRManagementSystem.Application.Commands.Auth
{
    public class AuthLoginCommand : IRequest<AuthLoginResponse>
    {
        public string Username { get; set; } = string.Empty;

    }
    public class AuthLoginResponse
    {
        public int ReturnCode { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
