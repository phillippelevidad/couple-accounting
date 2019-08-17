using System;

namespace Domain
{
    public sealed class Category : AggregateRoot
    {
        private Category()
        {
        }

        public Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
    }
}
