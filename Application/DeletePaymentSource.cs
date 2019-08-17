using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class DeletePaymentSource : IRequest<Result>
    {
        public DeletePaymentSource(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class DeletePaymentSourceHandler : IRequestHandler<DeletePaymentSource, Result>
    {
        private readonly IPaymentSourceRepository paymentSourceRepository;

        public DeletePaymentSourceHandler(IPaymentSourceRepository paymentSourceRepository)
        {
            this.paymentSourceRepository = paymentSourceRepository;
        }

        public async Task<Result> Handle(DeletePaymentSource request, CancellationToken cancellationToken)
        {
            return await paymentSourceRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
