using CSharpFunctionalExtensions;
using Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public interface IPaymentSourceRepository
    {
        Task<Result> AddAsync(PaymentSource paymentSource, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<PaymentSource> FindAsync(Guid id, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(PaymentSource paymentSource, CancellationToken cancellationToken);
    }
}
