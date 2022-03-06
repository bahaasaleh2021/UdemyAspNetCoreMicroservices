﻿using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence.Base
{
    public interface IDeletableAsyncRepository<T> : IUpdatableAsyncRepository<T> where T : EntityBase
    {
        Task DeleteAsync(T entity);
    }
}
