using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Authority.Model.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }           //角色唯一序列号

        public int State { get; set; }        //是否可用

        public string UserName { get; set; }  //登录名

        public string PassWord { get; set; }  //登录密码

        public string Role { get; set; }      //角色

        public string Department { get; set; } //部门

        public string Remarks { get; set; } //备注

        public string Token { get; set; } //token

        public string StartTime { get; set; } //开始日期

        public string EndTime { get; set; } //结束日期

        public string HospitalName { get; set; } //无用

        public string AdminPassword { get; set; } //无用
    }
}
