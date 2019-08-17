using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class DeletePayment : IRequest<Result>
    {
        public DeletePayment(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class DeletePaymentHandler : IRequestHandler<DeletePayment, Result>
    {
        private readonly IPaymentRepository paymentRepository;

        public DeletePaymentHandler(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<Result> Handle(DeletePayment request, CancellationToken cancellationToken)
        {
            return await paymentRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
