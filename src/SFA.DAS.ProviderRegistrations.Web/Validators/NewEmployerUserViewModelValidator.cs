using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;

namespace SFA.DAS.ProviderRegistrations.Web.Validators
{
    public class NewEmployerUserViewModelValidator : AbstractValidator<NewEmployerUserViewModel>
    {
        public NewEmployerUserViewModelValidator()
        {
            RuleFor(model => model.ProviderEmailAddress).EmailAddress()
                .When(model => model.CopyEmailToProvider)
                .WithMessage(@"The {PropertyName} field is not a valid e-mail address.");

            RuleFor(model => model.ProviderEmailAddress).NotEmpty()
                .When(model => model.CopyEmailToProvider)
                .WithMessage(@"The {PropertyName} field is required.");
        }
    }
}
