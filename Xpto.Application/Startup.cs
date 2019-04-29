using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json.Linq;

using Swashbuckle.AspNetCore.Swagger;

using Xpto.Data.Context;
using Xpto.Data.Repository.EntityFramework;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Services;

namespace Xpto.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Using in memory database for testing purpose
            services.AddDbContext<XptoContext>(opts => opts.UseInMemoryDatabase("MainDatabase"));

            services.AddScoped<IMapPointService, MapPointService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IKnownRouteService, KnownRouteService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IMapPointRepository, MapPointRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IKnownRouteRepository, KnownRouteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddMvc().ConfigureApiBehaviorOptions(o =>
            {
                o.InvalidModelStateResponseFactory = context =>
                {
                var error = new
                {
                Detail = "Custom error"
                    };

                    return new BadRequestObjectResult(error);
                };
            });

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes("_appSettings.Secret");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
                {
                    bearerOptions.RequireHttpsMetadata = false;
                    bearerOptions.SaveToken = true;
                    bearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }
            );

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                        Title = "Xpto API",
                        Description = "Xpto Delivery Web API",
                        Contact = new Contact
                        {
                            Name = "Níckolas Goline",
                                Email = string.Empty,
                                Url = "https://linkedin.com/in/ngoline"
                        },
                        License = new License
                        {
                            Name = "Use under MIT",
                                Url = "https://choosealicense.com/licenses/mit/"
                        }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                    { { "Bearer", new string[] { } },
                    };
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // global cors policy
            app.UseCors(c => c
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Xpto API V1");
            });

            app.UseMvc();
        }
    }
}
