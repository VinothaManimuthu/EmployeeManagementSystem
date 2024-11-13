using Employee_System.Models;
using Employee_System.Strategy_Pattern.Interface;

namespace Employee_System.Services.Interface
{
    public interface IAttendanceService
    {
        Task<Attendance> GetAttendanceByIdAsync(int id);
        Task<IEnumerable<Attendance>> GetAllAttendancesAsync();

        Task<Attendance> AddAttendanceAsync(int employeeId, DateTime checkInTime, DateTime checkOutTime, IAttendanceTrackingStrategy strategy);
        Task UpdateAttendanceAsync(Attendance attendance);
        Task DeleteAttendanceAsync(int id);
    }
}
