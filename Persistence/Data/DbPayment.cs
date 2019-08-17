using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence.Data
{
    public class DbPayment : DbEntityBase<Payment>
    {
        public Guid Id { get; private set; }
        public Guid SourceId { get; private set; }
        public DateTimeOffset DateTime { get; private set; }
        public Money Amount { get; private set; }
        public DbPaymentSource Source { get; private set; }

        public override Payment ToDomainModel()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFrom(Payment domainModel)
        {
            throw new NotImplementedException();
        }

        internal static DbPayment From(Payment payment)
        {
            return new DbPayment
            {
                Id = payment.Id,
                Amount = payment.Amount,
                DateTime = payment.DateTime,
                SourceId = payment.SourceId,
                AggregateRoot = payment
            };
        }
    }

    internal class DbPaymentConfiguration : IDbEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DbPayment>().ToTable("Payments");
            entity.Property(e => e.Amount).HasColumnType(ModelConfiguration.MoneyColumnTypeName);
            entity.HasOne(e => e.Source).WithMany(source => source.Payments).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
