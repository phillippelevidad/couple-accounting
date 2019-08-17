using CSharpFunctionalExtensions;
using Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public interface IPaymentRepository
    {
        Task<Result> AddAsync(Payment payment, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
