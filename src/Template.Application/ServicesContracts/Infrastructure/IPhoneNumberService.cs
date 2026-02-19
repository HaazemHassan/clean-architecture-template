namespace Template.Application.ServicesContracts.Infrastructure
{
    public interface IPhoneNumberService
    {
        bool IsValid(string phoneNumber);
        string Normalize(string phoneNumber);
    }

}
