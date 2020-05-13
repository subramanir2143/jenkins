using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces
{
    public interface ICache
    {
        bool IsTracked(Guid id);
        void Set(Guid id, AggregateRoot aggregate);
        AggregateRoot Get(Guid id);
        void Remove(Guid id);
        void RegisterEvictionCallback(Action<Guid> action);
    }
}
