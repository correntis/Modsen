using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Models;

namespace Library.Application.UseCases.Authors
{
    public class GetAuthorsPageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorsPageUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Author>> ExecuteAsync(int pageIndex, int pageSize)
        {
            var authors = await _unitOfWork.AuthorsRepository.GetPageAsync(pageIndex, pageSize);

            return _mapper.Map<List<Author>>(authors);
        }
    }
}
