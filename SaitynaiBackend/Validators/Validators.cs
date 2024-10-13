using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SaitynaiBackend.Data.Models;

public class GamePostDtoValidator : AbstractValidator<Game.GamePostDto>
{
    public GamePostDtoValidator()
    {
        RuleFor(g => g.Title)
            .NotNull().WithMessage("Title is required.").WithErrorCode("422")
            .NotEmpty().WithMessage("Title cannot be empty.").WithErrorCode("422");

        RuleFor(g => g.Description)
            .NotNull().WithMessage("Description is required.").WithErrorCode("422")
            .NotEmpty().WithMessage("Description cannot be empty.");
    }
}

public class GamePutDtoValidator : AbstractValidator<Game.GamePutDto>
{
    public GamePutDtoValidator()
    {
        RuleFor(g => g.Title)
            .NotNull().WithMessage("Title is required.").WithErrorCode("422")
            .NotEmpty().WithMessage("Title cannot be empty.").WithErrorCode("422");

        RuleFor(g => g.Description)
            .NotNull().WithMessage("Description is required.").WithErrorCode("422")
            .NotEmpty().WithMessage("Description cannot be empty.").WithErrorCode("422");
    }
}
public class PublisherPostDtoValidator : AbstractValidator<Publisher.PublisherPostDto>
{
    public PublisherPostDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotNull().WithMessage("Name is required.").WithErrorCode("422")
            .NotEmpty().WithMessage("Name cannot be empty.").WithErrorCode("422");

        RuleFor(p => p.Description)
            .NotNull().WithMessage("Description is required.").WithErrorCode("422")
            .NotEmpty().WithMessage("Description cannot be empty.").WithErrorCode("422");
    }
}

public class PublisherPutDtoValidator : AbstractValidator<Publisher.PublisherPutDto>
{
    public PublisherPutDtoValidator()
    {
        // Both Name and Description are required in PUT
        RuleFor(p => p.Name)
            .NotNull().WithMessage("Name is required.").WithErrorCode("422")
            .NotEmpty().WithMessage("Name cannot be empty.").WithErrorCode("422");

        RuleFor(p => p.Description)
            .NotNull().WithMessage("Description is required.").WithErrorCode("422")
            .NotEmpty().WithMessage("Description cannot be empty.").WithErrorCode("422");
    }
}
public class ReviewPostDtoValidator : AbstractValidator<Review.ReviewPostDto>
{
    public ReviewPostDtoValidator()
    {
        //RuleFor(r => r.Rating)
        //    .InclusiveBetween(1, 10).WithMessage("Rating must be between 1 and 10.").WithErrorCode("422");

    }
}

public class ReviewPutDtoValidator : AbstractValidator<Review.ReviewPutDto>
{
    public ReviewPutDtoValidator()
    {
        // Both Rating and Comment are required in PUT
        RuleFor(r => r.Rating)
            .NotNull().WithMessage("Rating is required.");

        RuleFor(r => r.Comment)
                    .MaximumLength(1000).When(r => r.Comment != null).WithMessage("Comment cannot be longer than 1000 characters.");
    }
}
public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            Console.WriteLine("Invalid");
            var validationErrors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage)
                }).ToList();

            var errorResponse = new
            {
                Message = "Validation failed",
                Errors = validationErrors
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 422  // Return 422 for validation errors
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

