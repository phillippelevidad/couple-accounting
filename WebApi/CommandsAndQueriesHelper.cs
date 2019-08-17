using CSharpFunctionalExtensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi
{
    public class Commands
    {
        private readonly IMediator mediator;

        public Commands(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Result> SendAsync(IRequest<Result> command, CancellationToken cancellationToken = default)
        {
            return await mediator.Send(command, cancellationToken);
        }
    }

    public class Queries
    {
        private readonly IMediator mediator;

        public Queries(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TResult> RunAsync<TResult>(IRequest<TResult> query, CancellationToken cancellationToken = default)
        {
            return await mediator.Send(query, cancellationToken);
        }
    }
}
