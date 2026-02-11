using MediatR;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Bases.Responses;
using Template.Core.Features.Users.Queries.Models;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Features.Users.Queries.Handlers {
    public class CheckEmailAvailabilityQueryHandler : ResponseHandler, IRequestHandler<CheckEmailAvailabilityQuery, Response<CheckEmailAvailabilityResponse>> {
        private readonly IUnitOfWork _unitOfWork;

        public CheckEmailAvailabilityQueryHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<CheckEmailAvailabilityResponse>> Handle(CheckEmailAvailabilityQuery request, CancellationToken cancellationToken) {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);
            var response = new CheckEmailAvailabilityResponse {
                IsAvailable = user is null
            };

            return Success(response, message: response.IsAvailable ? "Email is available." : "Email is not available.");
        }
    }
}
