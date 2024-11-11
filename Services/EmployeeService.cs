using Employee_System.Caching;
using Employee_System.Models;
using Employee_System.Repository.GenericRepository;
using Employee_System.Services.Interface;
using Employee_System.Unit_of_work;
using Employee_System.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee_System.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LoggerService _logger;
        private readonly EmployeeCacheService _cacheService;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = LoggerService.Instance;
            _cacheService = EmployeeCacheService.Instance; // Singleton Cache service
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                // We assume here that we do not need to cache all employees as it might be too large. 
                return await _unitOfWork.Employees.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while fetching all employees.", ex);
                throw;
            }
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            try
            {
                // First check cache
                var cachedEmployee = _cacheService.Get(id);
                if (cachedEmployee != null)
                {
                    return cachedEmployee;
                }

                // If not found in cache, fetch from DB
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);
                
                if (employee != null)
                {
                    // Cache the fetched employee
                    _cacheService.Add(id, employee);
                }

                return employee;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occurred while fetching employee with ID: {id}", ex);
                throw;
            }
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            try
            {
                await _unitOfWork.Employees.AddAsync(employee);
                await _unitOfWork.SaveAsync();

                // Cache the newly created employee
                _cacheService.Add(employee.EmployeeId, employee);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while adding new employee.", ex);
                throw;
            }
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                await _unitOfWork.Employees.UpdateAsync(employee);
                await _unitOfWork.SaveAsync();

                // Update the cache with the updated employee
                _cacheService.Add(employee.EmployeeId, employee);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occurred while updating employee with ID: {employee.EmployeeId}", ex);
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found.");
                }

                await _unitOfWork.Employees.DeleteAsync(id);
                await _unitOfWork.SaveAsync();

                // Remove the employee from the cache
                _cacheService.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occurred while deleting employee with ID: {id}", ex);
                throw;
            }
        }
    }
}
