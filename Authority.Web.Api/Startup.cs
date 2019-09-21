using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authoritiy.IServices;
using Authority.IRepository;
using Authority.IRepository.Base;
using Authority.Model.Model;
using Authority.repository;
using Authority.repository.EF;
using Authority.Repository;
using Authority.Services;
using Authority.Web.Api.AOP.Filter;
using Authority.Web.Api.JwtHelper;
using CompareMoney.Core.Api.Log;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using NLog.Web;
using Web.Api.Log;

namespace Authority.Web.Api
{
    public class Startup
    {

        public static ILoggerRepository Repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Repository = LogManager.CreateRepository("Web.Api");
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionFilter)); //注入异常
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region Token注入           
            services.AddSingleton<IJwtInterface, JwtHelpers>(); //注入jwt


            services.AddAuthorization(options =>
            {
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("管理员", "Boss"));
                options.AddPolicy("All", policy => policy.RequireRole("管理员", "Boss", "test", "Guest"));
                options.AddPolicy("testOrGuest", policy => policy.RequireRole("test", "Guest").Build());
                options.AddPolicy("Guest", policy => policy.RequireRole("Guest").Build());
            });


            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,//还是从 appsettings.json 拿到的
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//发行人
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            services.AddAuthentication("Bearer")
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
            });
            #endregion

            #region 注入DB
            services.AddScoped<DbcontextRepository>();
            #endregion

            #region 注入全局异常的日志
            services.AddSingleton<ILoggerHelper, LogHelper>(); //注入全局日志
            #endregion

            #region 注入servvices
            services.AddSingleton<IUserServices, UserServices>(); //User
            services.AddSingleton<IDepartmentService, DepartmentServices>(); //Department

            #endregion

            #region 注入Repositiory
            services.AddSingleton<IUserRepository, UserRepository>(); //User
            services.AddSingleton<IDepartmentRepository, DepartmentsRepository>();//Department




            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            #region Contrllers Log
            app.UseStaticFiles();
            //使用NLog作为日志记录工具
            loggerFactory.AddNLog();
            //引入Nlog配置文件
            env.ConfigureNLog("NIog.config");
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region  Token 中间件
            app.UseAuthentication();
            #endregion

            #region 解决跨域问题
            app.Use(async (context, next) =>
            {
                context.Response.Headers.SetCommaSeparatedValues("Access-Control-Allow-Origin", "*");
                await next();
            });

            #endregion

            #region 短板中间件
            app.UseMvc();

            #endregion
        }
    }
}
