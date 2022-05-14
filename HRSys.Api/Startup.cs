using System;
using System.Text;
using HRSys.Api.Exception;
using HRSys.Logger;
using HRSys.Model;
using HRSys.Repositories.Generic;
using HRSys.Repositories.Generic.Interface;
using HRSys.Services.App_Users;
using HRSys.Services.Common;
using HRSys.Services.Common.Interface;
using HRSys.Services.Lookup;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace HRSys.Api
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

            services.AddDbContextPool<HRSysContext>(options =>
            {
                options.EnableSensitiveDataLogging(true);
                //options.UseLoggerFactory(MyLoggerFactory);
                options.ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuting, LogLevel.Trace)));
                options.UseSqlServer(Configuration.GetConnectionString("HRSysConnection"),
                x => x.EnableRetryOnFailure());
            }
);
            services.AddSession();

            LogConfiguration.ConnectionString = Configuration.GetConnectionString("HRSysConnection");
            LogConfiguration.TableName = "Logs";
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapper.AutoMapperConfiguration>();
            })
                 .CreateMapper());

            var SecretKey = Encoding.ASCII.GetBytes("HRSysY2020HRSysY2020");
            //Configure JWT Token Authentication - JRozario
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                    ValidateIssuer = true,
                    //Usually this is your application base URL - JRozario
                    ValidIssuer = "http://localhost/HRSys.Api/",
                    ValidateAudience = true,
                    //Here we are creating and using JWT within the same application. In this case base URL is fine - JRozario
                    //If the JWT is created using a web service then this could be the consumer URL - JRozario
                    ValidAudience = "http://localhost/HRSys.Api/",
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<ICustomSuccess, CustomSuccess>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISendSMS_Service, SendSMS_Service>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                 .Where(a => a.FullName.Contains($"{nameof(HRSys.Services)}") ||
                             a.FullName.Contains($"{nameof(HRSys.Repositories)}"));

            services.Scan(scan =>
                scan.FromAssemblies(assemblies)
                    .AddClasses()
                    .AsMatchingInterface()
                    .WithScopedLifetime());

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<HRSysContext>();
            services.AddSingleton<IConfiguration>(Configuration);

            // services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //services.AddControllers()
            // .AddJsonOptions(options =>
            // {
            //     options.JsonSerializerOptions.PropertyNamingPolicy = null;
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseSession();
            app.UseMvc();
        }
    }
}
