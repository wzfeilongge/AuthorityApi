using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Web.Api.ControllerModel
{
    public class ChangeDepartmentModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 旧部门
        /// </summary>
        public string OldDepartment { get; set; }
        /// <summary>
        /// 新部门
        /// </summary>
        public string NewDepartment { get; set; }


    }
}
