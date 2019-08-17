using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Persistence.Data
{
    public class DbCategory : DbEntityBase<Category>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; internal set; }

        public ICollection<DbPayment> Payments { get; private set; } = new List<DbPayment>();

        public override Category ToDomainModel()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFrom(Category domainModel)
        {
            if (Name != domainModel.Name)
                Name = domainModel.Name;
        }

        internal static DbCategory From(Category domainModel)
        {
            return new DbCategory
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                AggregateRoot = domainModel
            };
        }
    }

    internal class DbCategoryConfiguration : IDbEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DbCategory>().ToTable("Categories");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        }
    }
}
