using Employee_System.Caching;
using Employee_System.Models;
using Employee_System.Repository.GenericRepository;
using Employee_System.Services.Interface;
using Employee_System.Strategy_Pattern.Interface;
using Employee_System.Unit_of_work;
using Employee_System.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee_System.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LoggerService _logger;
        private readonly AttendanceCacheService _cacheService;

        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = LoggerService.Instance;
            _cacheService = AttendanceCacheService.Instance; // Singleton Cache service
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id)
        {
            try
            {
                // First check cache
                var cachedAttendance = _cacheService.Get(id);
                if (cachedAttendance != null)
                {
                    return cachedAttendance;
                }

                // If not found in cache, fetch from DB
                var attendance = await _unitOfWork.Attendances.GetByIdAsync(id);

                if (attendance != null)
                {
                    // Cache the fetched attendance
                    _cacheService.Add(id, attendance);
                }

                return attendance;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while fetching attendance by ID.", ex);
                throw; // Re-throw the exception after logging
            }
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendancesAsync()
        {
            try
            {
                return await _unitOfWork.Attendances.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while fetching all attendances.", ex);
                throw;
            }
        }

        public async Task<Attendance> AddAttendanceAsync(int employeeId, DateTime checkInTime, DateTime checkOutTime, IAttendanceTrackingStrategy strategy)
        {
            try
            {
                if (checkInTime >= checkOutTime)
                {
                    throw new ArgumentException("Check-in time cannot be greater than or equal to check-out time.");
                }

                var attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    CheckInTime = checkInTime,
                    CheckOutTime = checkOutTime,
                    WorkHours = strategy.CalculateWorkHours(checkInTime, checkOutTime)
                };

                await _unitOfWork.Attendances.AddAsync(attendance);
                await _unitOfWork.SaveAsync();

                // Cache the newly created attendance
                _cacheService.Add(attendance.AttendanceId, attendance);

                if (attendance.AttendanceId == 0)
                {
                    throw new InvalidOperationException("Attendance record could not be created.");
                }

                return attendance;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while adding attendance.", ex);
                throw;
            }
        }

        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            try
            {
                await _unitOfWork.Attendances.UpdateAsync(attendance);
                await _unitOfWork.SaveAsync();

                // Update the cache with the new attendance
                _cacheService.Add(attendance.AttendanceId, attendance);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while updating attendance.", ex);
                throw;
            }
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            try
            {
                await _unitOfWork.Attendances.DeleteAsync(id);
                await _unitOfWork.SaveAsync();

                // Remove attendance from cache
                _cacheService.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while deleting attendance.", ex);
                throw;
            }
        }
    }
}
