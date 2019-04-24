using System;
using System.Runtime.Serialization;

namespace BullOak.Repositories.EventStore.Test.Integration.Ids
{
    [DataContract]
    public struct ExplicitlySerializableId : IEquatable<ExplicitlySerializableId>, ISerializeToStreamName
    {
        private const string IdPrefix = "ExplicitlySerializableId";
        [DataMember] public Guid Id { get; private set; }

        public ExplicitlySerializableId(Guid id) => Id = id;

        public static ExplicitlySerializableId NewId() => new ExplicitlySerializableId(Guid.NewGuid());

        public static implicit operator Guid(ExplicitlySerializableId id) => id.Id;
        public static explicit operator ExplicitlySerializableId(Guid id) => new ExplicitlySerializableId(id);

        public static bool operator !=(ExplicitlySerializableId left, ExplicitlySerializableId right) =>
            left.Id != right.Id;

        public static bool operator ==(ExplicitlySerializableId left, ExplicitlySerializableId right) =>
            left.Id == right.Id;

        public bool Equals(ExplicitlySerializableId other) => Id == other.Id;
        public override bool Equals(object obj) => obj is ExplicitlySerializableId id && Equals(id);

        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => Id.ToString();
        public string Serialize()
        {
            return $"{IdPrefix}-{Id}";
        }
    }
}