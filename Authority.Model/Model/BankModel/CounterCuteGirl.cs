using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Authority.Model.Model.BankModel
{
    public class CounterCuteGirl
    {
        /// <summary>
        /// 员工唯一Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>      
        public string Name { get; set; }

        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// 是否可用 用于上班 或者暂停服务
        /// </summary>
        public bool IsTrue { get; set; }

        /// <summary>
        /// 柜台号
        /// </summary>
        public int CounterNumber { get; set; }

    }
}
