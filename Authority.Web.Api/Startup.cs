using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Authority.Applicaion.AutoMapper;
using Authority.Business;
using Authority.Business.Business;
using Authority.Common.ReadisHelper.ReadisInterface;
using Authority.IRepository;
using Authority.IRepository.Base;
using Authority.Model.Model;
using Authority.Web.Api.AOP;
using Authority.Web.Api.AOP.Filter;
using Authority.Web.Api.JwtHelper;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
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
using Swashbuckle.AspNetCore.Swagger;
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region  MVC
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionFilter)); //注入全局异常过滤
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion

          


            #region  配置跨域
            services.AddCors(c =>
            {

                c.AddPolicy("LimitRequests", policy =>
                {
                    // 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    // 注意，http://127.0.0.1:1818 和 http://localhost:1818 是不一样的，尽量写两个
                    policy
                    .WithOrigins("http://127.0.0.1:1818", "http://localhost:8080", "http://localhost:8021"
                    , "http://localhost:8081", "http://localhost:1818"
                    , "http://localhost:9001", "http://localhost:1090"
                    ,"http://localhost:5000","http://localhost:5001"
                    )
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                });


            });


            #endregion

            services.AddAutoMapper(typeof(Startup));

          


            #region 注入Redis
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();//这里说下，如果是自己的项目，个人更建议使用单例模式 
            #endregion

            #region 注入Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "Authority.API",
                    Description = "框架说明文档",
                    TermsOfService = "None"
                });

                var basePaths = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePaths, "Authority.Web.Api.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
            });

            #endregion

            #region 注入Token           
            services.AddSingleton<IJwtInterface, JwtHelpers>(); //注入jwt

            

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("管理员", "Boss"));
                options.AddPolicy("All", policy => policy.RequireRole("管理员", "普通用户", "test", "Guest"));
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

            

            #region 使用 AutoFac 第三方IOC接管 core内置DI容器 实现解耦
            var builder = new ContainerBuilder();
            builder.RegisterType<LogHelper>().As<ILoggerHelper>(); //日志过滤器(全局异常日志过滤)

            #region 容器 Aop
            builder.RegisterType<TranAOP>();                       //日志AOP拦截器
            builder.RegisterType<AuthorityRedisAOP>();             //RedisAop拦截器
            #endregion 

            builder.RegisterType<BankBusiness>().As<IBankHandle>();//自己随便写着玩的
            builder.RegisterType<AutoMapperDto>();
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;//获取项目路径

            var SerivcesDllFile = Path.Combine(basePath, "Authority.Services.dll");//获取注入项目绝对路径         
            var assemblysServices = Assembly.LoadFrom(SerivcesDllFile);//直接采用加载文件的方法
            //指定已扫描程序集中的类型注册为提供所有其实现的接口 实现AOP      ;//注入repository
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TranAOP), typeof(AuthorityRedisAOP)); //指定已扫描程序集中的类型注册为提供所有其实现的接口 实现AOP  

            var RepositoryDllFile = Path.Combine(basePath, "Authority.Repository.dll");//获取注入项目绝对路径           
            var assemblysRepositorys = Assembly.LoadFrom(RepositoryDllFile);//直接采用加载文件的方法
            builder.RegisterAssemblyTypes(assemblysRepositorys).AsImplementedInterfaces(); //指定已扫描程序集中的类型注册为提供所有其实现的接口 实现AOP  


            builder.Populate(services);
            var AppBuilder = builder.Build();
            return new AutofacServiceProvider(AppBuilder); //第三方IOC接管 core内置DI容器

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

            #region 处理环境
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            #endregion

            #region  中间件Common
            app.UseCookiePolicy();    // 使用cookie
            app.UseStatusCodePages();//把错误码返回前台，比如是404
            #endregion

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");

                c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件，
            });
            #endregion

            #region  Token 中间件
            app.UseAuthentication();
            #endregion

            #region 解决跨域问题
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.SetCommaSeparatedValues("Access-Control-Allow-Origin", "*");
            //    await next();
            //});

            #endregion

            #region 短板中间件
            app.UseMvc();

            #endregion
        }
    }
}
