using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Models;

namespace Library.Application.UseCases.Authors
{
    public class GetAllAuthorsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAuthorsUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Author>> ExecuteAsync()
        {
            var authors = await _unitOfWork.AuthorsRepository.GetAllAsync();

            return _mapper.Map<List<Author>>(authors);
        }
    }
}
