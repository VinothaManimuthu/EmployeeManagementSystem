using Employee_System.Logging;
using Employee_System.Models;
using Employee_System.DAL;
using System.Threading.Tasks;
using Employee_System.Repository.GenericRepository;

namespace Employee_System.Repository
{
    public class AttendanceRepository : GenericRepository<Attendance>
    {
        private readonly LoggerService _logger;

        public AttendanceRepository(ApplicationDbContext context) : base(context)
        {
            // Using LoggerService for logging
            _logger = LoggerService.Instance;
        }

        // Example method to add attendance
        public async Task AddAttendanceAsync(Attendance attendance)
        {
            try
            {
                await AddAsync(attendance);  // Base method from GenericRepository
                _logger.Info($"Attendance added for Employee ID: {attendance.EmployeeId}");
            }
            catch (Exception ex)
            {
                _logger.Error("Error while adding attendance", ex);
                throw;
            }
        }

        // Example method to update attendance
        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            try
            {
                await UpdateAsync(attendance);  // Base method from GenericRepository
                _logger.Info($"Attendance updated for Employee ID: {attendance.EmployeeId}");
            }
            catch (Exception ex)
            {
                _logger.Error("Error while updating attendance", ex);
                throw;
            }
        }

        // Example method to delete attendance
        public async Task DeleteAttendanceAsync(int id)
        {
            try
            {
                await DeleteAsync(id);  // Base method from GenericRepository
                _logger.Info($"Attendance deleted for Employee ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while deleting attendance for Employee ID: {id}", ex);
                throw;
            }
        }
    }
}
