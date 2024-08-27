using FluentValidation;

namespace DigitalHub_Task.Middleware
{
    public class DtoValidator : AbstractValidator<DigitalHub_Task.Models.Entity.Dto>
    {
        public DtoValidator()
        {
            RuleFor(dto => dto.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(dto => dto.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(dto => dto.DueDate)
                .Must(BeAValidDate).WithMessage("DueDate cannot be in the past.")
                .When(task => task.DueDate.HasValue);

        }

        private bool BeAValidDate(DateTime? dueDate)
        {
            return dueDate == null || dueDate.Value >= DateTime.UtcNow;
        }
    }
}
