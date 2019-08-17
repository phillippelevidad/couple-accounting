using System;

namespace Domain
{
    public sealed class Payment : AggregateRoot
    {
        private Payment() { }

        private Payment(Guid id, Guid sourceId, Guid categoryId, DateTimeOffset dateTime, Money amount)
        {
            Id = id;
            SourceId = sourceId;
            CategoryId = categoryId;
            DateTime = dateTime;
            Amount = amount;
        }

        public static Payment Register(Guid id, Guid sourceId, Guid categoryId, DateTimeOffset dateTime, Money amount)
        {
            var payment = new Payment(id, sourceId, categoryId, dateTime, amount);
            payment.AddDomainEvent(new PaymentRegistered(payment));
            return payment;
        }

        public Guid SourceId { get; private set; }
        public Guid CategoryId { get; private set; }
        public DateTimeOffset DateTime { get; private set; }
        public Money Amount { get; private set; }
    }
}
