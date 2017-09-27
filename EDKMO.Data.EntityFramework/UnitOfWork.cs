using EDKMO.Data.EntityFramework.Repositories;
using EDKMO.Domain;
using EDKMO.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKMO.Data.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly Model _context;

        private IEventRepository _EventRepository;
        private IEventTypeRepository _EventTypeRepository;
        private IUserRepository _UserRepository;
        private ITerritoryRepository _TerritoryRepository;

        #endregion

        #region Constructors

        public UnitOfWork()
        {
            _context = new Model();
        }

        #endregion

        public IEventRepository EventRepository { get { return _EventRepository ?? (_EventRepository = new EventRepository(_context)); } }
        public IEventTypeRepository EventTypeRepository { get { return _EventTypeRepository ?? (_EventTypeRepository = new EventTypeRepository(_context)); } }
        public IUserRepository UserRepository { get { return _UserRepository ?? (_UserRepository = new UserRepository(_context)); } }
        public ITerritoryRepository TerritoryRepository { get { return _TerritoryRepository ?? (_TerritoryRepository = new TerritoryRepository(_context)); } }
        
        #region IUnitOfWork Methods

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sbError = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sbError.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                        sbError.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }
                throw new Exception(sbError.ToString(), e);
            }
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            try
            {
                return _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sbError = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sbError.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                        sbError.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }
                throw new Exception(sbError.ToString(), e);
            }
        }

        public Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region IDisposable Members

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}