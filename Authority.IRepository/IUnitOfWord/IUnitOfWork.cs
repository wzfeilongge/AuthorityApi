using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.IRepository.IUnitOfWord
{
   public interface IUnitOfWork
    {
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran();

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();

    }
}
