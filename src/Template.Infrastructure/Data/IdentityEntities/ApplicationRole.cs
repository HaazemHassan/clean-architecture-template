using Microsoft.AspNetCore.Identity;

namespace Template.Core.Entities.IdentityEntities {
    public class ApplicationRole : IdentityRole<int> {
        public ApplicationRole() {

        }
        public ApplicationRole(string role) {
            Name = role;
        }
    }
}
