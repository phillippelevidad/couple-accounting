using CSharpFunctionalExtensions;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class AddCategory : IRequest<Result>
    {
        public AddCategory(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }

    public class AddCategoryHandler : IRequestHandler<AddCategory, Result>
    {
        private readonly ICategoryRepository categoryRepository;

        public AddCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result> Handle(AddCategory request, CancellationToken cancellationToken)
        {
            var category = new Category(request.Id, request.Name);
            return await categoryRepository.AddAsync(category, cancellationToken);
        }
    }
}
