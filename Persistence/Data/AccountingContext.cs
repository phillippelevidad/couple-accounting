using CSharpFunctionalExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class AccountingContext : DbContext
    {
        private readonly IMediator mediator;

        public AccountingContext(IMediator mediator, DbContextOptions options) : base(options)
        {
            this.mediator = mediator;
        }

        public DbSet<DbCategory> Categories { get; set; }
        public DbSet<DbPayment> Payments { get; set; }
        public DbSet<DbPaymentSource> PaymentSources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var configurationInterface = typeof(IDbEntityConfiguration);
            var configurationTypes = GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => configurationInterface.IsAssignableFrom(type))
                .ToList();

            foreach (var type in configurationTypes)
            {
                var config = Activator.CreateInstance(type) as IDbEntityConfiguration;
                config.Configure(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public Result SaveChangesWithResult()
        {
            try
            {
                SaveChanges();
                DispatchEventsAsync().Wait();
                return Result.Ok();
            }
            catch (SqlException ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> SaveChangesWithResultAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await base.SaveChangesAsync(cancellationToken);
                await DispatchEventsAsync();
                return Result.Ok();
            }
            catch (SqlException ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        private async Task DispatchEventsAsync()
        {
            var aggregateRoots = ChangeTracker.Entries<IDbEntity>()
                .Select(entry => entry.Entity.AggregateRoot)
                .ToArray();

            foreach (var root in aggregateRoots)
                await DispatchEventsAsync(root);
        }

        private async Task DispatchEventsAsync(AggregateRoot aggregateRoot)
        {
            if (aggregateRoot == null)
                return;

            foreach (DomainEvent domainEvent in aggregateRoot.ConsumeDomainEvents())
                await mediator.Publish(domainEvent);
        }
    }
}
