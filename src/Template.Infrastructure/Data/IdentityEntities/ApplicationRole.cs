using Microsoft.AspNetCore.Identity;

namespace Template.Infrastructure.Data.IdentityEntities {
    public class ApplicationRole : IdentityRole<int> {
        public ApplicationRole() {

        }
        public ApplicationRole(string role) {
            Name = role;
        }
    }
}
