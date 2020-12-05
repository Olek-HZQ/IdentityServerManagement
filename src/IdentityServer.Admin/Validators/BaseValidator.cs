using FluentValidation;

namespace IdentityServer.Admin.Validators
{
    public class BaseValidator<T> : AbstractValidator<T> where T : class
    {
    }
}
