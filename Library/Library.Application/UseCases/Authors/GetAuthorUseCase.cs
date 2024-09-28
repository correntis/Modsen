using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.UseCases.Authors
{
    public class GetAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Author> ExecuteAsync(Guid id)
        {
            var authorEnity = await _unitOfWork.AuthorsRepository.GetAsync(id)
                ?? throw new NotFoundException();

            return _mapper.Map<Author>(authorEnity);
        }
    }
}
