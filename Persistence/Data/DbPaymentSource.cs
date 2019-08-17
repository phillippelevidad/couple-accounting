using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Persistence.Data
{
    public class DbPaymentSource : DbEntityBase<PaymentSource>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsDeleted { get; internal set; }
        public ICollection<DbPayment> Payments { get; private set; } = new List<DbPayment>();

        public override PaymentSource ToDomainModel()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFrom(PaymentSource domainModel)
        {
            if (Name != domainModel.Name)
                Name = domainModel.Name;
        }

        internal static DbPaymentSource From(PaymentSource domainModel)
        {
            return new DbPaymentSource
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                AggregateRoot = domainModel
            };
        }
    }

    internal class DbPaymentSourceConfiguration : IDbEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DbPaymentSource>().ToTable("PaymentSources");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        }
    }
}
