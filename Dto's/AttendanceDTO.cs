namespace Employee_System.Dto_s
{
    public class AttendanceDTO
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public double WorkHours { get; set; }
    }
     public class CreateAttendanceDTO
    {
        public int EmployeeId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
}
