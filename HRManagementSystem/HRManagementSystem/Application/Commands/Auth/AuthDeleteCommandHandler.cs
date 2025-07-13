using DBUtility;
using HRManagementSystem.Common.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Application.Commands.Auth
{
    public class AuthDeleteCommandHandler : IRequestHandler<AuthDeleteCommand, AuthDeleteResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthDeleteCommandHandler(
            UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;
        }

        public async Task<AuthDeleteResponse> Handle(AuthDeleteCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(command.Username);
            if (user == null)
            {
                return new AuthDeleteResponse
                {
                    ReturnCode = (int)ErrorCode.ReturnCode.DataNotFound
                };
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                List<string> errorMsg = new List<string>();
                foreach (var error in result.Errors)
                {
                    errorMsg.Add(error.Description);
                }
                return new AuthDeleteResponse
                {
                    ReturnCode = (int)ErrorCode.ReturnCode.OperationFailed,
                    ErrorMessage = errorMsg
                };
            }

            return new AuthDeleteResponse
            {
                ReturnCode = (int)ErrorCode.ReturnCode.OperationSuccessful
            };
        }
    }
}
