using FluentValidation;

namespace IdentityServer.AuthIdentity.Validators
{
    public class BaseValidator<T> : AbstractValidator<T> where T : class
    {
    }
}
