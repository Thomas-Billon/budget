namespace Budget.Server.Core.Shared
{
    public class DateOnlyRange
    {
        public DateOnly? StartDate { get; init; } = null;
        public DateOnly? EndDate { get; init; } = null;

        public DateOnlyRange(DateOnly? startDate, DateOnly? endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
