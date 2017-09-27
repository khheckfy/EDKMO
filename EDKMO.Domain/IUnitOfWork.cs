using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDKMO.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties

        Repositories.IEventRepository EventRepository { get; }
        Repositories.IEventTypeRepository EventTypeRepository { get; }
        Repositories.IUserRepository UserRepository { get; }
        Repositories.ITerritoryRepository TerritoryRepository { get; }

        #endregion

        #region Methods
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}