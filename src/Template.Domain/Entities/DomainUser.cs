using Template.Domain.Entities.Base;

namespace Template.Domain.Entities
{
    public sealed class DomainUser : FullAuditableEntity<int>
    {
        public DomainUser(string firstName, string lastName, string email, string? phoneNumber = null, string? address = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string? Address { get; private set; }

        public string FullName => $"{FirstName} {LastName}";



        public void UpdateInfo(string? firstName = null, string? lastName = null, string? phoneNumber = null, string? address = null)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName;

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;

            if (!string.IsNullOrWhiteSpace(address))
                Address = address;
        }



    }
}
