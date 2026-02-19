using AutoMapper;
using ErrorOr;
using MediatR;
using Template.Application.ServicesContracts.Infrastructure;
using Template.Domain.Common.Constants;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ErrorOr<UpdateProfileCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberService _phoneNumberService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<UpdateProfileCommandResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            string normalizedPhoneNumber;

            var userFromDb = await _unitOfWork.Users.GetByIdAsync(request.OwnerUserId, cancellationToken);
            if (userFromDb is null)
                return Error.NotFound(description: "User not found");


            if (request.PhoneNumber is not null)
            {
                normalizedPhoneNumber = _phoneNumberService.Normalize(request.PhoneNumber);

                var isPhoneExists = await _unitOfWork.Users.AnyAsync(u =>
                            u.PhoneNumber == normalizedPhoneNumber
                           && u.Id != userFromDb.Id, cancellationToken);

                if (isPhoneExists)
                    return Error.Conflict(code: ErrorCodes.User.PhoneAlreadyExists, description: "Phone number already exists");

            }

            userFromDb.UpdateInfo(
                firstName: request.FirstName,
                lastName: request.LastName,
                phoneNumber: request.PhoneNumber,
                address: request.Address
            );

            await _unitOfWork.Users.UpdateAsync(userFromDb, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userResponse = _mapper.Map<DomainUser, UpdateProfileCommandResponse>(userFromDb);
            return userResponse;
        }
    }
}

