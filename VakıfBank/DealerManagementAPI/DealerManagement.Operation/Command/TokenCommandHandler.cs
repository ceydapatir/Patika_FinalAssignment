using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DealerManagement.Base.Encryption;
using DealerManagement.Base.Response;
using DealerManagement.Base.Token;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static DealerManagement.Operation.Cqrs.TokenCqrs;

namespace DealerManagement.Operation.Command
{
    public class TokenCommandHandler :
        IRequestHandler<CreateTokenCommand, ApiResponse<TokenResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly JwtConfig jwtConfig;
        public TokenCommandHandler(DealerManagementDBContext dbContext, Microsoft.Extensions.Options.IOptionsMonitor<JwtConfig> jwtConfig)
        {
            this.dbContext = dbContext;
            this.jwtConfig = jwtConfig.CurrentValue;
        }

        public async Task<ApiResponse<TokenResponse>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<Employee>().AsQueryable<Employee>().FirstOrDefaultAsync(x => x.EmployeeNumber == request.Model.EmployeeNumber, cancellationToken);
            if (entity == null)
            {
                return new ApiResponse<TokenResponse>("Invalid user informations");
            }

            var md5 = Md5.Create(request.Model.Password);
            if (entity.Password != md5)
            {
                return new ApiResponse<TokenResponse>("Invalid user informations");
            }

            string token = Token(entity);
            TokenResponse tokenResponse = new()
            {
                Token = token,
                EmployeeNumber = entity.EmployeeNumber
            };
            
            return new ApiResponse<TokenResponse>(tokenResponse);
        }

        private string Token(Employee user)
        {
            Claim[] claims = GetClaims(user);
            var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            var jwtToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }

        private Claim[] GetClaims(Employee user)
        {
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("EmployeeNumber", user.EmployeeNumber.ToString()),
                new Claim("CompanyId", user.CompanyId.ToString()),
                new Claim("Role", user.Role),
                new Claim("FullName", $"{user.Name}"+ " " +$"{user.Surname}"),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return claims;
        }
    }
}