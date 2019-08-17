using CSharpFunctionalExtensions;

namespace Domain
{
    public class Money : ValueObject<Money>
    {
        public Money(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }

        protected override bool EqualsCore(Money other)
        {
            return Amount.Equals(other.Amount);
        }

        protected override int GetHashCodeCore()
        {
            return 1233020415 + Amount.GetHashCode();
        }

        public static Money operator +(Money a, Money b)
        {
            return new Money(a.Amount + b.Amount);
        }

        public static Money operator -(Money a, Money b)
        {
            return new Money(a.Amount - b.Amount);
        }

        public static Money operator *(Money a, Money b)
        {
            return new Money(a.Amount * b.Amount);
        }

        public static Money operator /(Money a, Money b)
        {
            return new Money(a.Amount / b.Amount);
        }
    }
}
