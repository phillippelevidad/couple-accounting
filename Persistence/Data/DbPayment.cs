using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence.Data
{
    public class DbPayment : DbEntityBase<Payment>
    {
        public Guid Id { get; private set; }
        public Guid SourceId { get; private set; }
        public Guid CategoryId { get; private set; }
        public DateTimeOffset DateTime { get; private set; }
        public decimal Amount { get; private set; }
        public DbPaymentSource Source { get; private set; }
        public DbCategory Category { get; private set; }

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
                SourceId = payment.SourceId,
                CategoryId = payment.CategoryId,
                DateTime = payment.DateTime,
                Amount = payment.Amount,
                AggregateRoot = payment
            };
        }
    }

    internal class DbPaymentConfiguration : IDbEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DbPayment>().ToTable("Payments");
            entity.Property(e => e.Amount).HasColumnType(ModelConfiguration.MoneyColumnType);
            entity.HasOne(e => e.Source).WithMany(source => source.Payments).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Category).WithMany(category => category.Payments).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
