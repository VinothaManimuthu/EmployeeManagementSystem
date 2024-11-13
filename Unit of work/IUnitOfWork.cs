using Employee_System.Repository;

namespace Employee_System.Unit_of_work
{
    public interface IUnitOfWork : IDisposable
    {
        EmployeeRepository Employees { get; }
        RoleRepository Roles { get; }
        LeaveRequestRepository LeaveRequests { get; }
        AttendanceRepository Attendances { get; }
        Task SaveAsync();
    }
}
