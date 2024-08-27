using DigitalHub_Task.Models.Entity;
using FluentValidation;


namespace DigitalHub_Task.Middleware
{
    public class TaskValidator : AbstractValidator<DigitalHub_Task.Models.Entity.Task>
    {
        public TaskValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(task => task.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(task => task.Status)
                .IsInEnum().WithMessage("Status must be a valid value.");

            RuleFor(task => task.DueDate)
                .Must(BeAValidDate).WithMessage("DueDate cannot be in the past.")
                .When(task => task.DueDate.HasValue);

            RuleFor(task => task.CreatedAt)
                .NotEmpty().WithMessage("CreatedAt is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedAt cannot be in the future.");
        }

        private bool BeAValidDate(DateTime? dueDate)
        {
            return dueDate == null || dueDate.Value >= DateTime.UtcNow;
        }
    }
}
