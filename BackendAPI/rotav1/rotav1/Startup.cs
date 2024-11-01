﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using rotav1.Data;
using rotav1.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
namespace rotav1
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurar o banco de dados PostgreSQL
            var connectionString = _configuration.GetConnectionString("PostgreSqlConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Configuração de CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFlutterApp", policy =>
                {
                    policy.WithOrigins("http://localhost:63520", "http://10.0.2.2:3000") // Flutter web / Emulador Android
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Permitir autenticação via token/cookies
                });
            });


            // Configurar autenticação JWT
            var secretKey = "your-very-long-and-very-secret-key"; // Use uma chave segura
            var key = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero // Evitar problemas de sincronização de horário
                    };
                });

            
            services.AddAuthorization();

            // Registrar TokenService para injeção de dependências
            services.AddSingleton<TokenService>();

            // Adicionar controladores
            services.AddControllers();
        }
        // Configuração do pipeline de middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("AllowFlutterApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
