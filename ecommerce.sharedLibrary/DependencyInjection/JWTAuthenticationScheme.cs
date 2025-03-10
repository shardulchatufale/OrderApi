using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.sharedLibrary.DependencyInjection
{
    public static  class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJWTAuthanticationScheme(this IServiceCollection services,IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearer", options =>
            {
                var Key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
                string issuer = config.GetSection("Authentication:Issuer").Value!;
                string audience = config.GetSection("Authentication:Audience").Value!;

                options.RequireHttpsMetadata = false;
                options.SaveToken = true; options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            }
            );return services;
        }
    }
}
