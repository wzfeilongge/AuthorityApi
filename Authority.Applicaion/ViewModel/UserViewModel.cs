using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Applicaion.ViewModel
{
   public class UserViewModel
    {     
        public int State { get; set; }        //是否可用

        public string UserName { get; set; }  //登录名      

        public string Role { get; set; }      //角色

        public string Department { get; set; } //部门

        public string Remarks { get; set; } //备注

        public string Token { get; set; } //token
    }
}
