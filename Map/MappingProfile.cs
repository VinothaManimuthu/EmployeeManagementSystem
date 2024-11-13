using AutoMapper;
using Employee_System.Dto_s;
using Employee_System.Models;

namespace Employee_System.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Employee, CreateEmployeeDTO>().ReverseMap();
            CreateMap<Employee, GetByIdEmployeeDTO>()
    .ForMember(dest => dest.LeaveRequests, opt => opt.MapFrom(src => src.LeaveRequests));

            CreateMap<LeaveRequest, LeaveRequestDTO>().ReverseMap();
            CreateMap<LeaveRequest, PostLeaveRequestDTO>().ReverseMap();
            CreateMap<Attendance, AttendanceDTO>().ReverseMap();
            CreateMap<Attendance, CreateAttendanceDTO>().ReverseMap();

        }
    }
}
