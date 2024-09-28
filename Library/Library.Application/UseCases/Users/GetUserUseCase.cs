using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.UseCases.Users
{
    public class GetUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<User> ExecuteAsync(Guid id)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(id)
                ?? throw new NotFoundException();

            return _mapper.Map<User>(userEntity);
        }
    }
}
