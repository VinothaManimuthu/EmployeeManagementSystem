using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_System.Dto_s;
using Employee_System.Models;
using FluentValidation;

namespace Employee_System.Validator
{
    public class AttendanceValidator : AbstractValidator<CreateAttendanceDTO>
    {
        public AttendanceValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0.");

            RuleFor(x => x.CheckInTime)
                .NotEmpty().WithMessage("Check-in time is required.")
                .LessThan(x => x.CheckOutTime).WithMessage("Check-in time must be before check-out time.");

            RuleFor(x => x.CheckOutTime)
                .NotEmpty().WithMessage("Check-out time is required.")
                .GreaterThan(x => x.CheckInTime).WithMessage("Check-out time must be after check-in time.");

            // RuleFor(x => x.WorkHours)
            //     .GreaterThanOrEqualTo(0).WithMessage("Work hours cannot be negative.")
            //     .LessThanOrEqualTo(24).WithMessage("Work hours cannot exceed 24 hours.")
            //     .Equal(x => (x.CheckOutTime - x.CheckInTime).TotalHours)
            //     .WithMessage("Work hours must match the difference between check-in and check-out times.");
            
            var attendance = new AttendanceDTO();

            var checkInTime = attendance.CheckInTime;
            var checkOutTime = attendance.CheckOutTime;

            // Calculate the duration in hours
            var durationTime = (checkOutTime - checkInTime).TotalHours;
            Console.WriteLine("WorkHours duration",durationTime);

        }
    }
}