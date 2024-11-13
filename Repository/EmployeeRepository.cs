using Employee_System.Logging;
using Employee_System.Models;
using Employee_System.DAL;
using System.Threading.Tasks;
using Employee_System.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Employee_System.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>
    {
        private readonly LoggerService _logger;

        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            // Using LoggerService for logging
            _logger = LoggerService.Instance;
        }

        // Example method to add an employee
        public async Task AddEmployeeAsync(Employee employee)
        {
            try
            {
                await AddAsync(employee);  // Base method from GenericRepository
                _logger.Info($"Employee added: {employee.EmployeeId}, ID: {employee.EmployeeId}");
            }
            catch (Exception ex)
            {
                _logger.Error("Error while adding employee", ex);
                throw;
            }
        }

        // Example method to update an employee
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                await UpdateAsync(employee);  // Base method from GenericRepository
                _logger.Info($"Employee updated: {employee.EmployeeId}, ID: {employee.EmployeeId}");
            }
            catch (Exception ex)
            {
                _logger.Error("Error while updating employee", ex);
                throw;
            }
        }

        // Example method to delete an employee
        public async Task DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                await DeleteAsync(employeeId);  // Base method from GenericRepository
                _logger.Info($"Employee deleted: ID: {employeeId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while deleting employee with ID: {employeeId}", ex);
                throw;
            }
        }

        public async Task<Employee> GetEmployeeWithDetailsAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.LeaveRequests)
                .Include(e => e.Attendances)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

//         public async Task<Employee> GetEmployeeWithDetailsUsingJoinAsync(int id)
// {
//     var employeeWithDetails = await _context.Employees
//         .Where(e => e.EmployeeId == id)
//         .GroupJoin(
//             _context.LeaveRequests,
//             employee => employee.EmployeeId,
//             leaveRequest => leaveRequest.EmployeeId,
//             (employee, leaveRequests) => new { employee, leaveRequests }
//         )
//         .SelectMany(
//             empWithLeaveRequests => empWithLeaveRequests.leaveRequests.DefaultIfEmpty(),
//             (empWithLeaveRequests, leaveRequest) => new { empWithLeaveRequests.employee, leaveRequest }
//         )
//         .GroupJoin(
//             _context.Attendances,
//             empWithLeaveRequests => empWithLeaveRequests.employee.EmployeeId,
//             attendance => attendance.EmployeeId,
//             (empWithLeaveRequest, attendances) => new { empWithLeaveRequest.employee, empWithLeaveRequest.leaveRequest, attendances }
//         )
//         .ToListAsync();
 
//     // Convert the result into a fully populated Employee object, if needed
//     // Since this is more complex, typically the `Include` approach is preferred.
//     return employeeWithDetails.FirstOrDefault()?.employee;
// }
    }
}
