using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence.Base
{
    public interface IUpdatableAsyncRepository<T> : IReadableAsyncRepository<T> where T : EntityBase
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
