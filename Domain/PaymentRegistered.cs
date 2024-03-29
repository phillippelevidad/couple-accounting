﻿namespace Domain
{
    public sealed class PaymentRegistered : DomainEvent
    {
        public PaymentRegistered(Payment payment)
        {
            Payment = payment;
        }

        public Payment Payment { get; }
    }
}
