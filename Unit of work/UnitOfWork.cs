using Employee_System.DAL;
using Employee_System.Repository;

namespace Employee_System.Unit_of_work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository Employees { get; }
        public RoleRepository Roles { get; }
        public LeaveRequestRepository LeaveRequests { get; }
        public AttendanceRepository Attendances { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Employees = new EmployeeRepository(_context);
            Roles = new RoleRepository(_context);
            LeaveRequests = new LeaveRequestRepository(_context);
            Attendances = new AttendanceRepository(_context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

