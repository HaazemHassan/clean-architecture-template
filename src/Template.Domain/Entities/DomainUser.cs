using Template.Domain.Entities.Bases;

namespace Template.Domain.Entities {
    public class DomainUser : FullAuditableEntity<int> {
        public DomainUser() {
        }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }

        public string FullName => $"{FirstName} {LastName}";



        public void UpdateInfo(string? firstName = null, string? lastName = null, string? phoneNumber = null, string? address = null) {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName;

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;

            if (!string.IsNullOrWhiteSpace(address))
                Address = address;
        }


        public static DomainUser Create(string firstName, string lastName, string email, string phoneNumber, string? address = null) {
            return new DomainUser {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address
            };
        }
    }
}
