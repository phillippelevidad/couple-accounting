using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class ChangeCategoryName : IRequest<Result>
    {
        public ChangeCategoryName(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }

        public Guid Id { get; }
        public string NewName { get; }
    }

    public class ChangeCategoryNameHandler : IRequestHandler<ChangeCategoryName, Result>
    {
        private readonly ICategoryRepository categoryRepository;

        public ChangeCategoryNameHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result> Handle(ChangeCategoryName request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.FindAsync(request.Id, cancellationToken);
            category.Name = request.NewName;
            return await categoryRepository.UpdateAsync(category, cancellationToken);
        }
    }
}
