using Employee_System.Caching;
using Employee_System.Models;
using Employee_System.Services.Interface;
using Employee_System.Unit_of_work;
using Employee_System.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Employee_System.Factory_Pattern;

namespace Employee_System.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LoggerService _logger;
        private readonly LeaveRequestCacheService _cacheService;

        public LeaveRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = LoggerService.Instance;
            _cacheService = LeaveRequestCacheService.Instance; // Singleton Cache service
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            try
            {
                // Assuming that we don't cache all leave requests to avoid excessive memory use.
                return await _unitOfWork.LeaveRequests.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while fetching all leave requests.", ex);
                throw;
            }
        }

        public async Task<LeaveRequest> GetLeaveRequestByIdAsync(int id)
        {
            try
            {
                // First check cache
                var cachedLeaveRequest = _cacheService.Get(id);
                if (cachedLeaveRequest != null)
                {
                    return cachedLeaveRequest;
                }

                // If not found in cache, fetch from DB
                var leaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(id);
                
                if (leaveRequest != null)
                {
                    // Cache the fetched leave request
                    _cacheService.Add(id, leaveRequest);
                }

                return leaveRequest;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occurred while fetching leave request with ID: {id}", ex);
                throw;
            }
        }

        public async Task AddLeaveRequestAsync(string leaveType, int employeeId, DateTime startDate, DateTime endDate, string reason, string status)
        {
            try
            {
                // Validate leave request
                if (startDate > endDate)
                {
                    throw new ArgumentException("Start date cannot be after end date.");
                }

                // Validate if employee exists
                var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} does not exist.");
                }

                // Use the factory to create the leave request
                var leaveRequest = LeaveRequestFactory.CreateLeaveRequest(leaveType, employeeId, startDate, endDate, reason, status);

                await _unitOfWork.LeaveRequests.AddAsync(leaveRequest);
                await _unitOfWork.SaveAsync();

                // Cache the newly created leave request
                _cacheService.Add(leaveRequest.LeaveRequestId, leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while adding a new leave request.", ex);
                throw;
            }
        }

        public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            try
            {
                await _unitOfWork.LeaveRequests.UpdateAsync(leaveRequest);
                await _unitOfWork.SaveAsync();

                // Update the cache with the updated leave request
                _cacheService.Add(leaveRequest.LeaveRequestId, leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occurred while updating leave request with ID: {leaveRequest.LeaveRequestId}", ex);
                throw;
            }
        }

        public async Task DeleteLeaveRequestAsync(int id)
        {
            try
            {
                var leaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(id);
                if (leaveRequest == null)
                {
                    throw new KeyNotFoundException($"Leave request with ID {id} does not exist.");
                }

                await _unitOfWork.LeaveRequests.DeleteAsync(id);
                await _unitOfWork.SaveAsync();

                // Remove the leave request from the cache
                _cacheService.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occurred while deleting leave request with ID: {id}", ex);
                throw;
            }
        }
    }
}
