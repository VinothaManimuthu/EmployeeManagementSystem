using Employee_System.Models;

namespace Employee_System.Dto_s
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string RoleId { get; set; }
        
    }

       public class CreateEmployeeDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string RoleId { get; set; }
        
    }
}
