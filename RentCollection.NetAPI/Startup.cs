using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RentCollection.NetAPI.Authentication;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.Security;

namespace RentCollection.NetAPI
{
    public class Startup
    {
        private string AuthenticationKey;

        private readonly IKeyVaultManager SecretManager;

        public Startup(IConfiguration configuration, IKeyVaultManager secretManager)
        {
            this.SecretManager = secretManager;
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddAzureClients(azureClientFactoryBuilder =>
            {
                azureClientFactoryBuilder.AddSecretClient(
                Configuration.GetSection("KeyVault"));
            });

            services.AddSingleton<IKeyVaultManager, KeyVaultManager>();
            services.AddHttpContextAccessor();

            // Database
            services.AddDbContext<RentCollectionContext>(async options => options.UseSqlServer(await this.SecretManager.GetSecret("RentCollectionDatabaseConnectionString")));
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            this.AuthenticationKey = await this.SecretManager.GetSecret("AuthenticationKey");

            // JWT Authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.AuthenticationKey)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(this.AuthenticationKey));

            // Add Cors
            services.AddCors();

            // Controllers
            services.AddControllers();


            // Add Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Rent Collection Application",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rent Collection Application V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}

