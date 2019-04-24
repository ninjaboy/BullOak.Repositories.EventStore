using System;
using BullOak.Repositories.EventStore.Test.Integration.Ids;

namespace BullOak.Repositories.EventStore.Test.Integration.Contexts
{
    public class StreamNameContext
    {
        public Guid LastGeneratedId { get; set; }
        public ExplicitlySerializableId ExplicitlySerializableId { get; set; }
        public ImplicitlySerializableId ImplicitlySerializableId { get; set; }        
        public bool ExplicitlySerializableIdExpected { get; set; }

        public object GetExpectedId() =>
            ExplicitlySerializableIdExpected ? (object) ExplicitlySerializableId : ImplicitlySerializableId;
    }
}