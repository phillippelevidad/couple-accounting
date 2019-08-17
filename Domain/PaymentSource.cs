using System;

namespace Domain
{
    public class PaymentSource : AggregateRoot
    {
        private PaymentSource() { }

        public PaymentSource(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
    }
}
