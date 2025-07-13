using HRManagementSystem.Application.Commands.Auth;
using HRManagementSystem.Common;
using HRManagementSystem.Common.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DBUtility.ErrorCode;

namespace HRManagementSystem.Controllers
{
    [ApiResult]
    [APIError]
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            IMediator mediator,
            IConfiguration config,
            UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _config = config;
            _userManager = userManager;
        }

        // 註冊 API
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterCommand command)
        {
            if (!ModelState.IsValid)
            {
                throw new APIError.ParamError();
            }

            var result = await _mediator.Send(command);

            if (result.ReturnCode == (int)ReturnCode.AuthorizationFailed)
            {
                throw new APIError.OperationFailed("此帳號已存在");
            }
            else if (result.ReturnCode == (int)ReturnCode.OperationFailed)
            {
                throw new APIError.OperationFailed(result.ErrorMessage);
            }
            return Ok("User created successfully");
        }

        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new APIError.OperationFailed();
            }
            var result = await _mediator.Send(new AuthDeleteCommand
            {
                Username = username
            });

            if (result.ReturnCode == (int)ReturnCode.DataNotFound)
            {
                throw new APIError.DataNotFound($"使用者 '{username}' 不存在");
            }
            else if (result.ReturnCode == (int)ReturnCode.OperationFailed)
            {
                throw new APIError.OperationFailed($"刪除失敗: {result.ErrorMessage}");
            }
            return Ok($"使用者 '{username}' 已成功刪除");
        }

        // 登入 API (用 BasicAuthentication 取得 User.Identity.Name，回傳 JWT)
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                throw new APIError.OperationFailed();
            }
            var result = await _mediator.Send(new AuthLoginCommand
            {
                Username = username
            });

            return Ok(result.Token);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("test")]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Auth/test 被呼叫");
            var user = User.Identity?.Name ?? "無登入使用者";
            return Ok($"登入使用者：{user}");
        }
    }
}
