using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class DeleteCategory : IRequest<Result>
    {
        public DeleteCategory(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class DeleteCategoryHandler : IRequestHandler<DeleteCategory, Result>
    {
        private readonly ICategoryRepository categoryRepository;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result> Handle(DeleteCategory request, CancellationToken cancellationToken)
        {
            return await categoryRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
