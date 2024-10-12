using FluentValidation;
using SaitynaiBackend.Data.Models;

public class GamePostDtoValidator : AbstractValidator<Game.GamePostDto>
{
    public GamePostDtoValidator()
    {
        RuleFor(g => g.Title)
            .NotNull().WithMessage("Title is required.")
            .NotEmpty().WithMessage("Title cannot be empty.");

        RuleFor(g => g.Description)
            .NotNull().WithMessage("Description is required.")
            .NotEmpty().WithMessage("Description cannot be empty.");
    }
}

public class GamePutDtoValidator : AbstractValidator<Game.GamePutDto>
{
    public GamePutDtoValidator()
    {
        RuleFor(g => g.Title)
            .NotNull().WithMessage("Title is required.")
            .NotEmpty().WithMessage("Title cannot be empty.");

        RuleFor(g => g.Description)
            .NotNull().WithMessage("Description is required.")
            .NotEmpty().WithMessage("Description cannot be empty.");
    }
}
public class PublisherPostDtoValidator : AbstractValidator<Publisher.PublisherPostDto>
{
    public PublisherPostDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name cannot be empty.");

        RuleFor(p => p.Description)
            .NotNull().WithMessage("Description is required.")
            .NotEmpty().WithMessage("Description cannot be empty.");
    }
}

public class PublisherPutDtoValidator : AbstractValidator<Publisher.PublisherPutDto>
{
    public PublisherPutDtoValidator()
    {
        // Both Name and Description are required in PUT
        RuleFor(p => p.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name cannot be empty.");

        RuleFor(p => p.Description)
            .NotNull().WithMessage("Description is required.")
            .NotEmpty().WithMessage("Description cannot be empty.");
    }
}
public class ReviewPostDtoValidator : AbstractValidator<Review.ReviewPostDto>
{
    public ReviewPostDtoValidator()
    {
        RuleFor(r => r.Rating)
            .InclusiveBetween(1, 10).WithMessage("Rating must be between 1 and 10.");

        RuleFor(r => r.Comment)
                    .MaximumLength(1000).When(r => r.Comment != null).WithMessage("Comment cannot be longer than 1000 characters.");
    }
}

public class ReviewPutDtoValidator : AbstractValidator<Review.ReviewPutDto>
{
    public ReviewPutDtoValidator()
    {
        // Both Rating and Comment are required in PUT
        RuleFor(r => r.Rating)
            .NotNull().WithMessage("Rating is required.")
            .InclusiveBetween(1, 10).WithMessage("Rating must be between 1 and 10.");

        RuleFor(r => r.Comment)
                    .MaximumLength(1000).When(r => r.Comment != null).WithMessage("Comment cannot be longer than 1000 characters.");
    }
}
