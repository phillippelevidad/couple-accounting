using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<DomainEvent> domainEvents = new List<DomainEvent>();

        protected void AddDomainEvent(DomainEvent newEvent)
        {
            domainEvents.Add(newEvent);
        }

        public IReadOnlyList<DomainEvent> ConsumeDomainEvents()
        {
            var copy = domainEvents.ToList().AsReadOnly();
            domainEvents.Clear();
            return copy;
        }
    }

    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }

    public abstract class DomainEvent : INotification
    {
    }
}
