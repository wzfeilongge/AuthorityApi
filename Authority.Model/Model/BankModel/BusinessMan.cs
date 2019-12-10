using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Authority.Model.BankModel
{
   public class BusinessMan
    {
        /// <summary>
        /// 凭条票据号
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

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

        /// <summary>
        /// 是否完成,票据作废
        /// </summary>
        public bool Istrue { get; set; }

    }
}
