using Budget.Server.Core.Enums;

namespace Budget.Server.Core.Helpers
{
    public class DateOnlyRange
    {
        public DateOnly? StartDate { get; set; } = null;
        public DateOnly? EndDate { get; set; } = null;
        public DateRangePreset Preset { get; set; } = DateRangePreset.None;

        public bool IsCustom => Preset == DateRangePreset.None;


        public DateOnlyRange(DateOnly? startDate, DateOnly? endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateOnlyRange(DateRangePreset preset)
        {
            Preset = preset;
        }
    }
}
