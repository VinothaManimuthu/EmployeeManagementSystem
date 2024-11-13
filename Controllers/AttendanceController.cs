using AutoMapper;
using Employee_System.Dto_s;
using Employee_System.Models;
using Employee_System.Services.Interface;
using Employee_System.Strategy_Pattern;
using Employee_System.Logging; 
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Employee_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IMapper _mapper;

        // Use the singleton LoggerService instance
        private readonly LoggerService _logger = LoggerService.Instance;

        public AttendanceController(IAttendanceService attendanceService, IMapper mapper)
        {
            _attendanceService = attendanceService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceDTO>>> GetAllAttendancesAsync()
        {
            try
            {
                _logger.Info("Fetching all attendance records.");
                var attendances = await _attendanceService.GetAllAttendancesAsync();
                var attendanceDtos = _mapper.Map<IEnumerable<AttendanceDTO>>(attendances);
                return Ok(attendanceDtos);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while fetching all attendance records.", ex);
                return StatusCode(500, "Internal server error.");
            }
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceDTO>> GetAttendanceByIdAsync(int id)
        {
            try
            {
                _logger.Info($"Fetching attendance record for ID: {id}");
                var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
                if (attendance == null)
                {
                    _logger.Warn($"Attendance record with ID: {id} not found.");
                    return NotFound();
                }
                var attendanceDto = _mapper.Map<AttendanceDTO>(attendance);
                return Ok(attendanceDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while fetching attendance record with ID: {id}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<ActionResult<CreateAttendanceDTO>> AddAttendanceAsync([FromBody] CreateAttendanceDTO attendanceRequest)
        {
            try
            {
                _logger.Info("Adding a new attendance record.");
                var strategy = new HourlyAttendanceTrackingStrategy();

                var createdAttendance = await _attendanceService.AddAttendanceAsync(
                    attendanceRequest.EmployeeId,
                    attendanceRequest.CheckInTime,
                    attendanceRequest.CheckOutTime,
                    strategy
                );

                var attendanceDto = _mapper.Map<CreateAttendanceDTO>(createdAttendance);
                _logger.Info($"Attendance record added for Employee ID: {attendanceRequest.EmployeeId}");
                return Ok(attendanceDto);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while adding a new attendance record.", ex);
                return StatusCode(500, "Internal server error.");
            }
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendanceAsync(int id)
        {
            try
            {
                _logger.Info($"Deleting attendance record with ID: {id}");
                var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
                if (attendance == null)
                {
                    _logger.Warn($"Attendance record with ID: {id} not found.");
                    return NotFound();
                }

                await _attendanceService.DeleteAttendanceAsync(id);
                _logger.Info($"Attendance record with ID: {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while deleting attendance record with ID: {id}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
