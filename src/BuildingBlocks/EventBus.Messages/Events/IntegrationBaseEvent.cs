using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public Guid Id { get; private set; }
        public DateTimeOffset CreationDate { get; private set; }

        public IntegrationBaseEvent()
        {
            Id=Guid.NewGuid();
            CreationDate = DateTimeOffset.UtcNow;
        }
        public IntegrationBaseEvent(Guid id, DateTimeOffset creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
