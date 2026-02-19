using FluentValidation;
using FluentValidation.Results;
using PhoneNumbers;
using Template.Application.ServicesContracts.Infrastructure;

namespace Template.Infrastructure.Services
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private const string DefaultRegion = "EG";

        private readonly PhoneNumberUtil _util;
        public PhoneNumberService()
        {
            _util = PhoneNumberUtil.GetInstance();

        }


        public bool IsValid(string input)
        {
            try
            {
                var parsed = _util.Parse(input, DefaultRegion);
                return _util.IsValidNumber(parsed);
            }
            catch
            {
                return false;
            }
        }

        public string Normalize(string phoneNumber)

        {
            if (phoneNumber is null)
                throw new ValidationException("Phone number cannot be null", [new ValidationFailure()]);

            var parsed = _util.Parse(phoneNumber, DefaultRegion);

            if (!_util.IsValidNumber(parsed))
                throw new ValidationException("Invalid phone number", [new ValidationFailure()]);

            return _util.Format(parsed, PhoneNumberFormat.E164);
        }
    }
}
