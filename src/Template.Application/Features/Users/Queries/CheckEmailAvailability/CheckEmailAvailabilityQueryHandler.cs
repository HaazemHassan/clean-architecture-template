using MediatR;
using Template.Application.Common.Responses;
using Template.Domain.Abstracts.RepositoriesAbstracts;

namespace Template.Application.Features.Users.Queries.CheckEmailAvailability {
    public class CheckEmailAvailabilityQueryHandler : ResponseHandler, IRequestHandler<CheckEmailAvailabilityQuery, Response<CheckEmailAvailabilityQueryResponse>> {
        private readonly IUnitOfWork _unitOfWork;

        public CheckEmailAvailabilityQueryHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<CheckEmailAvailabilityQueryResponse>> Handle(CheckEmailAvailabilityQuery request, CancellationToken cancellationToken) {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);
            var response = new CheckEmailAvailabilityQueryResponse {
                IsAvailable = user is null
            };

            return Success(response, message: response.IsAvailable ? "Email is available." : "Email is not available.");
        }
    }
}
