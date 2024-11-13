using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_System.Dto_s;
using Employee_System.Models;
using FluentValidation;

namespace Employee_System.Validator
{
    public class EmployeeValidator : AbstractValidator<EmployeeDTO>
    {
        private readonly string[] allowedDomains = { "gmail.com", "noblq.com" };
        public EmployeeValidator()
        {
             Console.WriteLine("EmployeeValidator is executing...");

            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                 .Must(HaveAllowedDomain).WithMessage("Email domain is not allowed.");

            RuleFor(e => e.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");

            RuleFor(e => e.DateOfJoining)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of joining cannot be in the future");

            RuleFor(e => e.RoleId)
                .NotEmpty().WithMessage("RoleId is required");
        }
        private bool HaveAllowedDomain(string email)
        {
            var domainPart = email.Split('@').LastOrDefault();
            return domainPart != null && allowedDomains.Contains(domainPart.ToLower());
        }
    }
}