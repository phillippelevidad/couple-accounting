using Application;
using CSharpFunctionalExtensions;
using Domain;
using Persistence.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class PaymentSourceRepository : IPaymentSourceRepository
    {
        private readonly AccountingContext context;

        public PaymentSourceRepository(AccountingContext context)
        {
            this.context = context;
        }

        public async Task<Result> AddAsync(PaymentSource paymentSource, CancellationToken cancellationToken)
        {
            var model = DbPaymentSource.From(paymentSource);
            context.PaymentSources.Add(model);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var model = await context.PaymentSources.FindAsync(new object[] { id }, cancellationToken);
            model.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }

        public async Task<PaymentSource> FindAsync(Guid id, CancellationToken cancellationToken)
        {
            var model = await context.PaymentSources.FindAsync(new object[] { id }, cancellationToken);
            return model.ToDomainModel();
        }

        public async Task<Result> UpdateAsync(PaymentSource paymentSource, CancellationToken cancellationToken)
        {
            var model = await context.PaymentSources.FindAsync(new object[] { paymentSource.Id }, cancellationToken);
            model.UpdateFrom(paymentSource);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
