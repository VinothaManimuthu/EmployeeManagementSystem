using Employee_System.Models;

namespace Employee_System.Factory_Pattern
{
    public static class LeaveRequestFactory
    {
        public static LeaveRequest CreateLeaveRequest(string leaveType, int employeeId, DateTime startDate, DateTime endDate, string reason, string status)
        {
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate,
                Reason = reason,
                Status = status,
                LeaveType = leaveType
            };

            // You can add custom logic here based on the leave type
            switch (leaveType.ToLower())
            {
                case "sick":
                    leaveRequest.Reason = reason ?? "Sick Leave"; // Default reason if not provided
                    leaveRequest.Status = status ?? "Pending";
                    break;

                case "vacation":
                    leaveRequest.Reason = reason ?? "Vacation Leave"; // Default reason if not provided
                    leaveRequest.Status = status ?? "Pending";
                    break;
                case "Casual":
                    leaveRequest.Reason = reason ?? "Casual Leave"; // Default reason if not provided
                    leaveRequest.Status = status ?? "Pending";
                    break;


                default:
                    throw new ArgumentException("Invalid leave type");
            }

            return leaveRequest;
        }
    }
}

