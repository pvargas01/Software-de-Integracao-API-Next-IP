using Microsoft.AspNetCore.Builder;
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
using System;

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
            // Configuração de CORS para permitir requisições da origem Flutter
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFlutterWeb", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Adicionar SignalR para comunicação em tempo real
            services.AddSignalR();

            // Adicionar controladores
            services.AddControllers();

            // Configurar o banco de dados PostgreSQL
            var connectionString = _configuration.GetConnectionString("PostgreSqlConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Configuração de CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFlutterApp", policy =>
                {
                    policy.WithOrigins("http://localhost:65053", "http://10.0.2.2:3000") // Flutter web / Emulador Android
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
        }

        // Configuração do pipeline de middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configurar CORS para todas as requisições
            app.UseCors("AllowFlutterWeb");

            // Configurar o roteamento
            app.UseRouting();

            // Autenticação e autorização
            app.UseAuthentication();
            app.UseAuthorization();

            // Configurar os endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
