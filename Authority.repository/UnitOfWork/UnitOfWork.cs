using Authority.IRepository.IUnitOfWord;
using Authority.repository.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected  readonly DbcontextRepository _db = DbcontextRepository.Context;


        public void BeginTran()
        {
            _db.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _db.Database.CommitTransaction();

            }
            catch {

                _db.Database.RollbackTransaction();
            }
        }

        public void Rollback()
        {
            _db.Database.RollbackTransaction();
        }
    }
}
