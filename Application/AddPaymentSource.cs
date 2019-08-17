using CSharpFunctionalExtensions;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class AddPaymentSource : IRequest<Result>
    {
        public AddPaymentSource(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }

    public class AddPaymentSourceHandler : IRequestHandler<AddPaymentSource, Result>
    {
        private readonly IPaymentSourceRepository paymentSourceRepository;

        public AddPaymentSourceHandler(IPaymentSourceRepository paymentSourceRepository)
        {
            this.paymentSourceRepository = paymentSourceRepository;
        }

        public async Task<Result> Handle(AddPaymentSource request, CancellationToken cancellationToken)
        {
            var paymentSource = new PaymentSource(request.Id, request.Name);
            return await paymentSourceRepository.AddAsync(paymentSource, cancellationToken);
        }
    }
}
