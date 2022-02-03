using AutoMapper;
using JobsityChatAPI.Data;
using JobsityChatAPI.Data.Contracts;
using JobsityChatAPI.Hubs;
using JobsityChatAPI.Identity;
using JobsityChatAPI.Identity.Models;
using JobsityChatAPI.Models;
using JobsityChatAPI.Services.Messages;
using JobsityChatAPI.Services.Messages.Contracts;
using JobsityChatAPI.Services.Stocks;
using JobsityChatAPI.Services.Stocks.Contracts;
using JobsityChatAPI.Services.Users;
using JobsityChatAPI.Services.Users.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace JobsityChatAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<BaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<LocalUser, IdentityRole>().AddEntityFrameworkStores<BaseContext>().AddDefaultTokenProviders();

            var signingConfigurations = new SigningConfigurations();

            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });


            services.AddControllers();
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IStocksBotService, StocksBotService>();
            services.AddTransient<HttpClient>();
            services.AddCors(options =>
            {
                options.AddPolicy("allowSpecificOrigins", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
                });
            });
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BaseContext context, UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("allowSpecificOrigins");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/MessageHub");
            });

            new IdentityInitializer(context, userManager, roleManager)
                .Initialize();
        }
    }
}
