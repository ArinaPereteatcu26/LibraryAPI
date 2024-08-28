using FluentValidation;
using LibraryWebAPI.Dtos.Author;

namespace LibraryWebAPI.Validations
{
    public class UpdateAuthorModelValidator : AbstractValidator<UpdateAuthorDto>
    {
        public UpdateAuthorModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid author ID");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .NotNull()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name maximum length is 50");
        }
    }
}
