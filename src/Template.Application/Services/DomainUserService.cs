using Template.Application.Contracts.Services.Application;
using Template.Domain.Abstracts.RepositoriesAbstracts;

namespace Template.Application.Services {
    public class DomainUserService : IDomainUserService {
        private readonly IUnitOfWork _unitOfWork;

        public DomainUserService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }


    }
}
