using Employee_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Employee_System.DAL
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Employee>()
                   .HasOne(e => e.Role)
                   .WithMany(r => r.Employees)
                   .HasForeignKey(e => e.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LeaveRequest>()
                   .HasOne(lr => lr.Employee)
                   .WithMany(e => e.LeaveRequests)
                   .HasForeignKey(lr => lr.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attendance>()
                   .HasOne(a => a.Employee)
                   .WithMany(e => e.Attendances)
                   .HasForeignKey(a => a.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Employee>()
        .HasMany(e => e.LeaveRequests)
        .WithOne(lr => lr.Employee)
        .HasForeignKey(lr => lr.EmployeeId)
        .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Employee>()
                .HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
