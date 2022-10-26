using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Services
{
    public class AuthenticationService
    {

        private readonly WalletDbContext _dbContext;
        public AuthenticationService(IConfiguration configuration, WalletDbContext _dbContext)
        {
            this._configuration = configuration;
            this._dbContext = _dbContext;
        }


        private readonly IConfiguration _configuration;

        public string CreateToken(UserLoginCommand userLogin)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email.Equals(userLogin.Email));

            List<Claim> userClaims = new List<Claim>();

            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            return new JwtSecurityTokenHandler().WriteToken(GetTokenOptions(userClaims));
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("JWTSettings");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("Secret").Value));

            return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GetTokenOptions(List<Claim> userClaims)
        {
            var jwtSettings = _configuration.GetSection("JWTSettings");

            var securityToken = new JwtSecurityToken
                (
                    issuer: jwtSettings.GetSection("Issuer").Value,
                    audience: jwtSettings.GetSection("ValidAudience").Value,
                    claims: userClaims,
                    signingCredentials: GetSigningCredentials(),
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("ExpiryMinutes").Value))
                );

            return securityToken;
        }

        public bool VerifyPasswordHash(UserLoginCommand userLogin)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email.Equals(userLogin.Email));

            if (user is null) return false;

            using(var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLogin.Password));

                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }
    }
}
