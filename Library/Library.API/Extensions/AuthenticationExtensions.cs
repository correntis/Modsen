using Library.API.Middleware;
using Library.Application.Services;
using Library.Core.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace Library.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddLibraryAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtOptions:Issuer"],
                        ValidAudience = configuration["JwtOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Secret"])),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {

                            if (context.Request.Cookies.TryGetValue("accessToken", out var accessToken))
                            {
                                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

                                if(jwtToken.ValidTo < DateTime.UtcNow)
                                {
                                    var tokenHandler = context.HttpContext.RequestServices.GetRequiredService<IRefreshTokenHandler>();
                                    var newAccessToken = tokenHandler.HandleUpdateAsync(context.HttpContext).Result;

                                    if (!string.IsNullOrEmpty(newAccessToken))
                                    {
                                        accessToken = newAccessToken;
                                    }
                                }

                                context.Token = accessToken;
                            }


                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}
