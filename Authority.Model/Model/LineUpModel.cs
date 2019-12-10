using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Model.Model
{
   public class LineUpModel
    {
        /// <summary>
        /// 用户唯一Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 请求排队的时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否绿通会员
        /// </summary>
        public bool IsVip { get; set; }
    }
}
