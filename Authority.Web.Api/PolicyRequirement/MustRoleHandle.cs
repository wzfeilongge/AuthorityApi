using Authoritiy.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Authority.Web.Api.PolicyRequirement.PolicyRole;

namespace Authority.Web.Api.PolicyRequirement
{
    public class MustRoleHandle : AuthorizationHandler<PolicyRole>
    {
        public IAuthenticationSchemeProvider _Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserServices _userservices;

        public MustRoleHandle(IAuthenticationSchemeProvider Schemes, IHttpContextAccessor accessor, IUserServices userservices)
        {
            _Schemes = Schemes;
            _accessor = accessor;
            _userservices = userservices;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRole requirement)
        {
            //获取所有的用户的权限
            var data = _userservices.GetAll(u => u.Id > 0);
            var list = await (from item in data
                              orderby item.Id
                              select new UserPermission
                              {
                                  Policy = item.Role
                              }).ToListAsync();
            requirement.UserPermissions = list;
            var filterContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext);
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)?.HttpContext;
            if (httpContext == null)
            {
                httpContext = _accessor.HttpContext;
            }
            if (httpContext != null)
            {
                var questUrl = httpContext.Request.Path.Value.ToLower();
                //判断请求是否停止
                var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                foreach (var scheme in await _Schemes.GetRequestHandlerSchemesAsync())
                {
                    if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                    {
                        context.Fail();
                        return;
                    }
                }
                //判断请求是否拥有凭据，即有没有登录
                var defaultAuthenticate = await _Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    //result?.Principal不为空即登录成功
                    if (result?.Principal != null)
                    {
                        httpContext.User = result.Principal;
                        //权限中是否存在请求的角色类型
                        if (true)
                        {
                            // 获取当前用户的角色信息
                            var currentUserRoles = (from item in httpContext.User.Claims
                                                    where item.Type == requirement.ClaimType
                                                    select item.Value).ToList();
                            //验证权限
                            if (currentUserRoles.Count <= 0)
                            {
                                context.Fail();
                                return;
                            }
                        }
                        context.Succeed(requirement);
                        return;
                    }
                    else {
                        context.Fail();
                        return;

                    }
                }
                else {
                     //是登录的api请求
                    if (!questUrl.Equals(requirement.LoginPath.ToLower())) {

                        context.Succeed(requirement);
                        return;
                    }
                    context.Fail();               
                }
                //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
                //if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType))
                //{
                //    context.Fail();
                //    return;
                //}
            }
            context.Succeed(requirement);
            return;
        }
    }
}
