using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class ChangePaymentSourceName : IRequest<Result>
    {
        public ChangePaymentSourceName(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }

        public Guid Id { get; }
        public string NewName { get; }
    }

    public class ChangePaymentSourceNameHandler : IRequestHandler<ChangePaymentSourceName, Result>
    {
        private readonly IPaymentSourceRepository paymentSourceRepository;

        public ChangePaymentSourceNameHandler(IPaymentSourceRepository paymentSourceRepository)
        {
            this.paymentSourceRepository = paymentSourceRepository;
        }

        public async Task<Result> Handle(ChangePaymentSourceName request, CancellationToken cancellationToken)
        {
            var paymentSource = await paymentSourceRepository.FindAsync(request.Id, cancellationToken);
            paymentSource.Name = request.NewName;
            return await paymentSourceRepository.UpdateAsync(paymentSource, cancellationToken);
        }
    }
}
