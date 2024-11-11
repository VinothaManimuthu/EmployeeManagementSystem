using Employee_System.Strategy_Pattern.Interface;

namespace Employee_System.Strategy_Pattern
{
    public class DailyAttendanceTrackingStrategy : IAttendanceTrackingStrategy
    {
        public double CalculateWorkHours(DateTime checkInTime, DateTime checkOutTime)
        {
            // Assuming a fixed number of hours for a full workday
            return 8.0;
        }
    }
}
