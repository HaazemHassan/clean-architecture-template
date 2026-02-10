using Template.Core.Abstracts.CoreAbstracts.Services;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;

namespace Template.Core.Services {
    public class DomainUserService : IDomainUserService {
        private readonly IUnitOfWork _unitOfWork;

        public DomainUserService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }


    }
}
