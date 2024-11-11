using System.Text.Json.Serialization;

namespace Employee_System.Models
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }
        public string LeaveType { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }

    }
}
