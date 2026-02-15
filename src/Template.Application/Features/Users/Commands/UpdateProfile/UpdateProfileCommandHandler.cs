using AutoMapper;
using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Api;
using Template.Domain.Abstracts.RepositoriesAbstracts;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.UpdateProfile {
    public class UpdateProfileCommandHandler : ResponseHandler, IRequestHandler<UpdateProfileCommand, Response<UpdateProfileCommandResponse>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;

        }

        public async Task<Response<UpdateProfileCommandResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken) {
            var userId = _currentUserService.UserId;

            var userFromDb = await _unitOfWork.Users.GetByIdAsync(userId!.Value, cancellationToken);
            if (userFromDb is null)
                return NotFound<UpdateProfileCommandResponse>("User not found");

            userFromDb.UpdateInfo(
                firstName: request.FirstName,
                lastName: request.LastName,
                phoneNumber: request.PhoneNumber,
                address: request.Address
            );

            await _unitOfWork.Users.UpdateAsync(userFromDb, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userResponse = _mapper.Map<DomainUser, UpdateProfileCommandResponse>(userFromDb);
            return Updated(userResponse);
        }

    }
}

