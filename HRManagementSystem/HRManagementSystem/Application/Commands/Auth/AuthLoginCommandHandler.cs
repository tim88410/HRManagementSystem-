using DBUtility;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace HRManagementSystem.Application.Commands.Auth
{
    public class AuthLoginCommandHandler : IRequestHandler<AuthLoginCommand, AuthLoginResponse>
    {
        private readonly IConfiguration _config;
        public AuthLoginCommandHandler(
            IConfiguration config
            )
        {
            _config = config;
        }

        public async Task<AuthLoginResponse> Handle(AuthLoginCommand command, CancellationToken cancellationToken)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, command.Username)
            };
            var jwtKey = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new AuthLoginResponse()
            {
                ReturnCode = (int)ErrorCode.ReturnCode.OperationSuccessful,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
