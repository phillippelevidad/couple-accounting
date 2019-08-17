using Domain;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Data
{
    public abstract class DbEntityBase<TDomainModel> : IDbEntity<TDomainModel> where TDomainModel : class
    {
        [NotMapped]
        public AggregateRoot AggregateRoot { get; protected set; }

        public abstract TDomainModel ToDomainModel();
        public abstract void UpdateFrom(TDomainModel domainModel);
    }

    public interface IDbEntity
    {
        AggregateRoot AggregateRoot { get; }

    }

    public interface IDbEntity<TDomainModel> : IDbEntity where TDomainModel : class
    {
        TDomainModel ToDomainModel();
        void UpdateFrom(TDomainModel domainModel);
    }

    public interface IDbEntityConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
