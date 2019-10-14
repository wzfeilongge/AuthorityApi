using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Applicaion.ViewModel
{
    public class CreateViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = "XXX银行";

        /// <summary>
        /// 取号时间
        /// </summary>
        public DateTime TakeNumber { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 前面还有多少人
        /// </summary>
        public int ResultName { get; set; }





    }
}
