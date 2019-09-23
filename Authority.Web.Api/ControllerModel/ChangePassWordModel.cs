﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Web.Api.ControllerModel
{
    public class ChangePassWordModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
  
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassWord { get; set; }






    }
}
