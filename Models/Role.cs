using Microsoft.AspNetCore.Identity;

namespace Employee_System.Models
{
    public class Role : IdentityRole
    {
        public ICollection<Employee> Employees { get; set; }
    }
}
