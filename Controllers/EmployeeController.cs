using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Employee_System.Dto_s;
using Employee_System.Models;
using Employee_System.Services;
using Employee_System.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Employee_System.Logging;  // Add the namespace for NLogSingleton
using NLog;

namespace Employee_System.Controllers
{
    //[Authorize(Roles = "Admin,HR,Employee")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly LoggerService _logger = LoggerService.Instance; // Injected logger

        public EmployeeController(RoleManager<IdentityRole> roleManager, IEmployeeService employeeService, IMapper mapper)
        {
            _roleManager = roleManager;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // GET: api/Employee
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            _logger.Info("Fetching all employees");
            var employees = await _employeeService.GetAllEmployeesAsync();
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
            return Ok(employeeDTOs);
        }

        // GET: api/Employee/{id}
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            _logger.Info($"Fetching employee with ID: {id}");
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                _logger.Warn($"Employee with ID {id} not found");
                return NotFound();
            }

            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return Ok(employeeDTO);
        }

        // POST: api/Employee
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDTO employeeDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.Error("Invalid employee data received for creation");
                return BadRequest(ModelState);
            }

            // Validate RoleId
            if (string.IsNullOrWhiteSpace(employeeDTO.RoleId))
            {
                _logger.Warn("RoleId cannot be null or empty.");
                return BadRequest("RoleId cannot be null or empty.");
            }

            // Check if the provided RoleId is valid
            var role = await _roleManager.FindByIdAsync(employeeDTO.RoleId);
            if (role == null)
            {
                _logger.Warn($"Invalid RoleId: {employeeDTO.RoleId} provided.");
                return BadRequest("Invalid RoleId provided.");
            }

            // Map DTO to Employee model
            var employee = _mapper.Map<Employee>(employeeDTO);

            // Assign the RoleId to the Employee
            employee.RoleId = role.Id;

            // Reset EmployeeId to 0 to ensure the database generates the ID
            employee.EmployeeId = 0;

            // Add the employee to the database
            await _employeeService.AddEmployeeAsync(employee);

            _logger.Info($"Employee with ID {employee.EmployeeId} created successfully.");

            // Map the newly created Employee to a DTO for the response
            var createdEmployeeDTO = _mapper.Map<CreateEmployeeDTO>(employee);

            // Return the created employee with the appropriate route
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, createdEmployeeDTO);
        }

        // PUT: api/Employee/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDTO employeeDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.Error("Invalid employee data received for update");
                return BadRequest(ModelState);
            }

            // Check if the provided RoleId is valid
            var role = await _roleManager.FindByIdAsync(employeeDTO.RoleId);
            if (role == null)
            {
                _logger.Warn($"Invalid RoleId: {employeeDTO.RoleId} provided.");
                return BadRequest("Invalid RoleId provided.");
            }

            var existingEmployee = await _employeeService.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                _logger.Warn($"Employee with ID {id} not found for update.");
                return NotFound();
            }

            // Map updated values from DTO to the existing employee
            _mapper.Map(employeeDTO, existingEmployee);

            // Update the RoleId
            existingEmployee.RoleId = role.Id;

            // Update the employee in the database
            await _employeeService.UpdateEmployeeAsync(existingEmployee);

            _logger.Info($"Employee with ID {id} updated successfully.");

            return NoContent();
        }

        // DELETE: api/Employee/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                _logger.Warn($"Employee with ID {id} not found for deletion.");
                return NotFound();
            }

            await _employeeService.DeleteEmployeeAsync(id);

            _logger.Info($"Employee with ID {id} deleted successfully.");

            return NoContent();
        }
    }
}
