using System;
using System.Runtime.Serialization;

namespace BullOak.Repositories.EventStore.Test.Integration.Ids
{
    [DataContract]
    public struct ImplicitlySerializableId : IEquatable<ImplicitlySerializableId>
    {
        private const string IdPrefix = "ImplicitlySerializableId";
        [DataMember] public Guid Id { get; private set; }

        public ImplicitlySerializableId(Guid id) => Id = id;

        public static ImplicitlySerializableId NewId() => new ImplicitlySerializableId(Guid.NewGuid());

        public static implicit operator Guid(ImplicitlySerializableId id) => id.Id;
        public static explicit operator ImplicitlySerializableId(Guid id) => new ImplicitlySerializableId(id);

        public static bool operator !=(ImplicitlySerializableId left, ImplicitlySerializableId right) =>
            left.Id != right.Id;

        public static bool operator ==(ImplicitlySerializableId left, ImplicitlySerializableId right) =>
            left.Id == right.Id;

        public bool Equals(ImplicitlySerializableId other) => Id == other.Id;
        public override bool Equals(object obj) => obj is ImplicitlySerializableId id && Equals(id);

        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => Id.ToString();

        public string Serialize()
        {
            return $"{IdPrefix}-{Id}";
        }
    }
}