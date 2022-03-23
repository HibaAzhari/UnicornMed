using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UnicornMed.Common.Helpers.API.BookingHelper;
using UnicornMed.Common.Helpers.API.AuthHelper;
using UnicornMed.Common.Helpers.API.DoctorHelper;
using UnicornMed.Common.Helpers.API.PatientHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicornMed.Common.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UnicornMed.Common.Helpers.JwtAuthenticationManager;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using UnicornMed.Common.Helpers.API.AdminHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using UnicornMed.Authentication;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Builder;
using UnicornMed.Bots.Bot;
using UnicornMed.Api;
using UnicornMed.BotLibrary.Helpers;
using UnicornMed.BotLibrary.Dialogs;

namespace UnicornMed
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
            //services.AddCors(options => options.AddDefaultPolicy(
            //  builder =>
            //  {
            //      builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            //  }));

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                Configuration["ConnectionStrings:DB"], b => b.MigrationsAssembly("UnicornMed.Api")));

            //services.AddHttpClient().AddControllers().AddNewtonsoftJson();

            services.AddScoped<IConversationReferenceHelper, ConversationReferenceHelper>();

            services.AddScoped<INotificationHelper, NotificationHelper>();
            services.AddScoped<IEmailPromptHelper, EmailPromptHelper>();

            // Create the Bot Framework Authentication to be used with the Bot Adapter.
            services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

            // Create the Bot Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();

            // Create the User state. (Used in this bot's Dialog implementation.)
            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();

            // The Dialog that will be run by the bot.
            services.AddSingleton<RootDialog>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, Bot>();

            services.AddScoped<IBookingHelper, BookingHelper>();
            services.AddScoped<IAuthHelper, AuthHelper>();
            services.AddScoped<IAdminHelper, AdminHelper>();
            services.AddScoped<IPatientHelper, PatientHelper>();
            services.AddScoped<IDoctorHelper, DoctorHelper>();
            services.AddMvc();
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Token Authentication API",
                    Description = "ASP.NET Core 5.0 Web API"
                });
                //To Enable authorization using Swagger (JWT)
                //swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                //});
                //swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //          new OpenApiSecurityScheme
                //            {
                //                Reference = new OpenApiReference
                //                {
                //                    Type = ReferenceType.SecurityScheme,
                //                    Id = "Bearer"
                //                }
                //            },
                //            new string[] {}
                //    }
                //});
            });

            services.AddAuthentication(Configuration);

            services.AddControllers();

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseDefaultFiles()
               .UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
