using CSharpFunctionalExtensions;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class RegisterPayment : IRequest<Result>
    {
        public RegisterPayment(Guid id, Guid sourceId, DateTimeOffset dateTime, Money amount)
        {
            Id = id;
            SourceId = sourceId;
            DateTime = dateTime;
            Amount = amount;
        }

        public Guid Id { get; }
        public Guid SourceId { get; }
        public DateTimeOffset DateTime { get; }
        public Money Amount { get; }
    }
    
    public class RegisterPaymentHandler : IRequestHandler<RegisterPayment, Result>
    {
        private readonly IPaymentRepository paymentRepository;

        public RegisterPaymentHandler(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<Result> Handle(RegisterPayment request, CancellationToken cancellationToken)
        {
            var payment = Payment.Register(request.Id, request.SourceId, request.DateTime, request.Amount);
            return await paymentRepository.AddAsync(payment, cancellationToken);
        }
    }
}
