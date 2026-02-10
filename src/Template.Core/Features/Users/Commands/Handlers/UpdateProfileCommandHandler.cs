using AutoMapper;
using MediatR;
using Template.Core.Abstracts.ApiAbstracts;
using Template.Core.Abstracts.CoreAbstracts.Services;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Bases.Responses;
using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users.Commands.RequestModels;
using Template.Core.Features.Users.Commands.Responses;

namespace Template.Core.Features.Users.Commands.Handlers {
    public class UpdateProfileCommandHandler : ResponseHandler, IRequestHandler<UpdateProfileCommand, Response<UpdateProfileResponse>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDomainUserService _domainUserService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainUserService domainUserService, ICurrentUserService currentUserService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _domainUserService = domainUserService;
            _currentUserService = currentUserService;

        }

        public async Task<Response<UpdateProfileResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken) {
            var userId = _currentUserService.UserId;

            var userFromDb = await _unitOfWork.Users.GetByIdAsync(userId!.Value, cancellationToken);
            if (userFromDb is null)
                return NotFound<UpdateProfileResponse>("User not found");

            userFromDb.UpdateInfo(
                firstName: request.FirstName,
                lastName: request.LastName,
                phoneNumber: request.PhoneNumber,
                address: request.Address
            );

            await _unitOfWork.Users.UpdateAsync(userFromDb, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userResponse = _mapper.Map<DomainUser, UpdateProfileResponse>(userFromDb);
            return Updated(userResponse);
        }

    }
}

