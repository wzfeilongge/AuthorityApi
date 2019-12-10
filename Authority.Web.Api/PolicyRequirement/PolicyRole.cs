using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Web.Api.PolicyRequirement
{
    public class PolicyRole : IAuthorizationRequirement
    {

        /// <summary>
        /// 用户权限承载实体
        /// </summary>
        public class UserPermission
        {
            /// <summary>
            /// Policy权限
            /// </summary>
            public string Policy { get; set; }

        }

        public string ClaimType { internal get; set; }

        /// <summary>
        /// 用户权限集合
        /// </summary>
        public List<UserPermission> UserPermissions { get; set; }
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; }


        public string LoginPath { get; set; } = "/Api/Login";
        /// <summary>
        /// 构造
        /// </summary>
        public PolicyRole(string ClaimType)
        {
            //没有权限则跳转到这个路由
            DeniedAction = new PathString(LoginPath);
            //用户有权限访问的路由配置,当然可以从数据库获取
            UserPermissions = new List<UserPermission> {
                              new UserPermission { Policy=Permissions.Name},
            };
            this.ClaimType = ClaimType;
        }






    }
}
