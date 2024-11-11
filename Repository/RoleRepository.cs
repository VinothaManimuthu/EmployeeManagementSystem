using Employee_System.DAL;
using Employee_System.Models;
using Employee_System.Repository.GenericRepository;
using Employee_System.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Employee_System.Repository
{
    public class RoleRepository : GenericRepository<Role>
    {
        private readonly LoggerService _logger;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _logger = LoggerService.Instance; // Using singleton logger service
        }

        // Example of a custom method to get a role by name
        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            try
            {
                var role = await _context.Roles
                                         .FirstOrDefaultAsync(r => r.Name == roleName);

                if (role == null)
                {
                    _logger.Warn($"Role with name {roleName} not found.");
                }

                return role;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occurred while fetching role by name: {roleName}", ex);
                throw; // Re-throw the exception after logging
            }
        }
    }
}
