using Employee_System.Models;

namespace Employee_System.Services.Interface
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int id);
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
        
        Task AddLeaveRequestAsync(string leaveType, int employeeId, DateTime startDate, DateTime endDate, string reason, string status);
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task DeleteLeaveRequestAsync(int id);
    }
}


