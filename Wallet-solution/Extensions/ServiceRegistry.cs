using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Wallet_solution.BackgroundWork;
using Wallet_solution.Filters;
using Wallet_solution.Models;
using Wallet_solution.Services;

namespace Wallet_solution.Extensions
{
    public static class ServiceRegistry
    {
        public static void ConfigureDbContext(this IServiceCollection service, IConfiguration config) =>
            service.AddDbContext<WalletDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Wallet-solution")));

        public static void ConfigureCors(this IServiceCollection service) =>
            service.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

        public static void RegisterServices(this IServiceCollection service)
        {
            service.AddTransient<AuthenticationService>();
            service.AddTransient<UserService>();
            service.AddTransient<TransactionService>();
            service.AddTransient<WalletService>();
            service.AddScoped<ValidationFilter>();
            service.AddTransient<InterestService>();
            service.AddTransient<Backgroundjob>();
            service.AddTransient<WalletDbContext>();
        }

        public static void ConfigureJWT(this IServiceCollection service, IConfiguration config)
        {
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWTSettings:Secret").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
        
    }
}
