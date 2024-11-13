using Employee_System.DAL;
using Employee_System.Models;
using Employee_System.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Employee_System.Logging;

namespace Employee_System.Repository
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>
    {
        private readonly LoggerService _logger;

        public LeaveRequestRepository(ApplicationDbContext context) : base(context)
        {
            _logger = LoggerService.Instance; // Use the singleton LoggerService
        }

        // Override GetAllAsync to include the Employee
        public override async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            try
            {
                return await _context.Set<LeaveRequest>()
                                     .Include(lr => lr.Employee) // Include the related Employee
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while fetching all leave requests", ex);
                throw; // Re-throw the exception after logging
            }
        }

        // Override GetByIdAsync to include the Employee
        public override async Task<LeaveRequest> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<LeaveRequest>()
                                     .Include(lr => lr.Employee) // Include the related Employee
                                     .FirstOrDefaultAsync(lr => lr.LeaveRequestId == id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occurred while fetching leave request with ID {id}", ex);
                throw; // Re-throw the exception after logging
            }
        }
    }
}
