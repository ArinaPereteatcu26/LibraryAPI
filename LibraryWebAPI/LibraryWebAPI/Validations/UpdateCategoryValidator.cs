using FluentValidation;
using LibraryWebAPI.Dtos.Categories;

namespace LibraryWebAPI.Validations
{
    public class UpdateCategoryModelValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid category ID");

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
