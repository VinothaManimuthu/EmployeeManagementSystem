namespace Employee_System.Strategy_Pattern.Interface
{
    public interface IAttendanceTrackingStrategy
    {
        double CalculateWorkHours(DateTime checkInTime, DateTime checkOutTime);
    }
}
