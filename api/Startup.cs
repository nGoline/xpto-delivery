using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using domain.contexts;
using Microsoft.AspNetCore.Identity;
using domain.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using domain.services;
using Microsoft.AspNetCore.Authorization;

namespace api
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
            services.AddDbContext<MainContext>(opts => opts.UseInMemoryDatabase("MainDatabase"));

            services.AddScoped<MapPointService>();
            services.AddScoped<RouteService>();
            services.AddScoped<UserService>();
            
            services.AddScoped<MapPointRepository>();
            services.AddScoped<RouteRepository>();
            services.AddScoped<UserRepository>();

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

            // services.AddIdentity<ApplicationUser, IdentityRole>()
            //     .AddEntityFrameworkStores<ApplicationDbContext>()
            //     .AddDefaultTokenProviders();

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
            // .AddOAuth("GitHub", options =>
            // {
            //     options.ClientId = Configuration["GitHub:ClientId"];
            //     options.ClientSecret = Configuration["GitHub:ClientSecret"];
            //     options.CallbackPath = new PathString("/signin-github");

            //     options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            //     options.TokenEndpoint = "https://github.com/login/oauth/access_token";
            //     options.UserInformationEndpoint = "https://api.github.com/user";

            //     options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            //     options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            //     options.ClaimActions.MapJsonKey("urn:github:login", "login");
            //     options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
            //     options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

            //     options.Events = new OAuthEvents
            //     {
            //         OnCreatingTicket = async context =>
            //         {
            //             var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            //             request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            //             var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            //             response.EnsureSuccessStatusCode();

            //             var user = JObject.Parse(await response.Content.ReadAsStringAsync());

            //             context.RunClaimActions(user);
            //         },
            //         OnRemoteFailure = context =>
            //         {
            //             context.HandleResponse();
            //             context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
            //             return Task.FromResult(0);
            //         }
            //     };
            // });

            // services.AddAuthorization(auth => {
            //     auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            //         .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            //         .RequireAuthenticatedUser()
            //         .Build());
            // });
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

            app.UseMvc();
        }
    }
}
