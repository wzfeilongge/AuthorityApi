using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Authority.Model.Model
{
   public class Sale
    {
        /// <summary>
        /// 销售记录表主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 记录单价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 出售价格
        /// </summary>
        public double TotalPrice { get; set; }

        /// <summary>
        /// 出售日期
        /// </summary>
        public string SaleDate { get; set; }

        /// <summary>
        /// 销售员Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int ProductId { get; set; }
    }
}
