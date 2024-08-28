using FluentValidation;
using LibraryWebAPI.Dtos.Books;

namespace LibraryWebAPI.Validations
{
    public class UpdateBookModelValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid book ID");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(50)
                .WithMessage("Title maximum length is 50");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("Invalid author ID");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Invalid category ID");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Invalid price");
        }
    }
}
