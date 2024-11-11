using System.Data;

namespace Employee_System.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
