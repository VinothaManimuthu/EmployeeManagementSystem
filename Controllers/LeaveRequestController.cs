using AutoMapper;
using Employee_System.Dto_s;
using Employee_System.Models;
using Employee_System.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  // Ensure to import the right logging namespace
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaveRequestController> _logger;  // Inject ILogger<LeaveRequestController>

        public LeaveRequestController(ILeaveRequestService leaveRequestService, IMapper mapper, ILogger<LeaveRequestController> logger)
        {
            _leaveRequestService = leaveRequestService;
            _mapper = mapper;
            _logger = logger;  // Use the injected logger
        }

        // GET: api/LeaveRequest
        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveRequestDTO>>> GetAllLeaveRequests()
        {
            _logger.LogInformation("Fetching all leave requests.");
            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
            var leaveRequestDTOs = _mapper.Map<IEnumerable<LeaveRequestDTO>>(leaveRequests);
            return Ok(leaveRequestDTOs);
        }

        // GET: api/LeaveRequest/{id}
        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDTO>> GetLeaveRequestById(int id)
        {
            _logger.LogInformation($"Fetching leave request with ID {id}.");
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null)
            {
                _logger.LogWarning($"Leave request with ID {id} not found.");
                return NotFound();
            }

            var leaveRequestDTO = _mapper.Map<LeaveRequestDTO>(leaveRequest);
            return Ok(leaveRequestDTO);
        }

        // POST: api/LeaveRequest
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<ActionResult> CreateLeaveRequest([FromBody] PostLeaveRequestDTO leaveRequestDTO)
        {
            if (leaveRequestDTO == null)
            {
                _logger.LogError("Invalid leave request data provided.");
                return BadRequest();
            }

            var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestDTO);
            await _leaveRequestService.AddLeaveRequestAsync(
                leaveRequestDTO.LeaveType,
                leaveRequestDTO.EmployeeId,
                leaveRequestDTO.StartDate,
                leaveRequestDTO.EndDate,
                leaveRequestDTO.Reason,
                leaveRequestDTO.Status
            );

            _logger.LogInformation($"Leave request created for employee ID {leaveRequestDTO.EmployeeId}.");
            return CreatedAtAction(nameof(GetLeaveRequestById), new { id = leaveRequest.LeaveRequestId }, leaveRequestDTO);
        }

        // PUT: api/LeaveRequest/{id}
        [Authorize(Roles = "Admin,HR")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLeaveRequest(int id, [FromBody] LeaveRequestDTO leaveRequestDTO)
        {
            if (leaveRequestDTO == null || id != leaveRequestDTO.LeaveRequestId)
            {
                _logger.LogError("Invalid leave request data or mismatched ID.");
                return BadRequest();
            }

            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null)
            {
                _logger.LogWarning($"Leave request with ID {id} not found.");
                return NotFound();
            }

            _mapper.Map(leaveRequestDTO, leaveRequest);  // Map updated values

            await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);
            _logger.LogInformation($"Leave request with ID {id} updated.");
            return NoContent();
        }

        // DELETE: api/LeaveRequest/{id}
        [Authorize(Roles = "Admin,HR")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null)
            {
                _logger.LogWarning($"Leave request with ID {id} not found.");
                return NotFound();
            }

            await _leaveRequestService.DeleteLeaveRequestAsync(id);
            _logger.LogInformation($"Leave request with ID {id} deleted.");
            return NoContent();
        }
    }
}
