using Employee_System.Strategy_Pattern.Interface;

namespace Employee_System.Strategy_Pattern
{
    public class HourlyAttendanceTrackingStrategy : IAttendanceTrackingStrategy
    {
        public double CalculateWorkHours(DateTime checkInTime, DateTime checkOutTime)
        {
            // Calculate the total hours worked
            return (checkOutTime - checkInTime).TotalHours;
        }
    }
}
