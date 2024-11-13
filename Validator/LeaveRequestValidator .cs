using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_System.Dto_s;
using FluentValidation;

namespace Employee_System.Validator
{
    public class LeaveRequestValidator : AbstractValidator<LeaveRequestDTO>
    {
        public LeaveRequestValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or on the end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or on the start date.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required.")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "Pending", "Approved", "Rejected" }.Contains(status))
                .WithMessage("Status must be one of the following: Pending, Approved, or Rejected.");

            RuleFor(x => x.LeaveType)
                .NotEmpty().WithMessage("Leave type is required.")
                .Must(type => new[] { "Sick", "Vacation", "Other" }.Contains(type))
                .WithMessage("Leave type must be one of the following: Sick, Vacation, or Other.");
        }
    }
}