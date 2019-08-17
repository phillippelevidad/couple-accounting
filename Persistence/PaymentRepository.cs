using Application;
using CSharpFunctionalExtensions;
using Domain;
using Persistence.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AccountingContext context;

        public PaymentRepository(AccountingContext context)
        {
            this.context = context;
        }

        public async Task<Result> AddAsync(Payment payment, CancellationToken cancellationToken)
        {
            var model = DbPayment.From(payment);
            context.Payments.Add(model);
            return await context.SaveChangesWithResultAsync(cancellationToken);
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var model = await context.Payments.FindAsync(new[] { id }, cancellationToken);
            context.Payments.Remove(model);
            return await context.SaveChangesWithResultAsync(cancellationToken);
        }
    }
}
