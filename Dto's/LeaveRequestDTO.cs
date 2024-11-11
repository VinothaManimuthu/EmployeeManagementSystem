namespace Employee_System.Dto_s
{
    public class LeaveRequestDTO
    {
        public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string LeaveType { get; set; }
    }

       public class PostLeaveRequestDTO
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string LeaveType { get; set; }
    }
}
