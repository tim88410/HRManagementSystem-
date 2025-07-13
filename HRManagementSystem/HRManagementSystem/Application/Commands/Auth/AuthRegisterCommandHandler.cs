using DBUtility;
using HRManagementSystem.Common.Data;
using HRManagementSystem.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRManagementSystem.Application.Commands.Auth
{
    public class AuthRegisterCommandHandler : IRequestHandler<AuthRegisterCommand, AuthRegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthRegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthRegisterResponse> Handle(AuthRegisterCommand command, CancellationToken cancellationToken)
        {

            var existingUser = await _userManager.FindByNameAsync(command.Username);
            if (existingUser != null)
            {
                return new AuthRegisterResponse
                {
                    ReturnCode = (int)ErrorCode.ReturnCode.AuthorizationFailed
                };
            }

            var user = new ApplicationUser
            {
                UserName = command.Username,
                Email = command.Email
            };

            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                List<string> errorMsg = new List<string>();
                foreach (var error in result.Errors)
                {
                    errorMsg.Add(error.Description);
                }
                return new AuthRegisterResponse
                {
                    ReturnCode = (int)ErrorCode.ReturnCode.OperationFailed,
                    ErrorMessage = errorMsg
                };
            }

            // 設定預設角色
            var defaultRole = UserRole.User;
            var roleName = defaultRole.ToString();

            // 先檢查角色是否存在，若無則建立
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // 加入角色
            await _userManager.AddToRoleAsync(user, roleName);

            // 設定基本 Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, defaultRole.ToString())
            };
            await _userManager.AddClaimsAsync(user, claims);

            return new AuthRegisterResponse
            {
                ReturnCode = (int)ErrorCode.ReturnCode.OperationSuccessful
            };
        }
    }
}
