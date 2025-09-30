import { DateRange } from "@/enums/DateRange";
import { Month } from "@/enums/Month";

const lastDayOfPreviousMonth: number = 0;

const getDatesFromDateRange = (dateRange: DateRange): { startDate: Date, endDate: Date } => {
    const now = new Date();

    let startDate: Date;
    let endDate: Date;

    switch (dateRange) {
        case DateRange.Today:
            startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            endDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            break;

        case DateRange.Yesterday:
            startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 1);
            endDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 1);
            break;

        case DateRange.CurrentWeek:
            startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - now.getDay());
            endDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() + (6 - now.getDay()));
            break;

        case DateRange.LastWeek:
            startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - now.getDay() - 7);
            endDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - now.getDay() - 1);
            break;

        case DateRange.CurrentMonth:
            startDate = new Date(now.getFullYear(), now.getMonth(), 1);
            endDate = new Date(now.getFullYear(), now.getMonth() + 1, lastDayOfPreviousMonth);
            break;

        case DateRange.LastMonth:
            startDate = new Date(now.getFullYear(), now.getMonth() - 1, 1);
            endDate = new Date(now.getFullYear(), now.getMonth(), lastDayOfPreviousMonth);
            break;

        case DateRange.CurrentYear:
            startDate = new Date(now.getFullYear(), Month.January, 1);
            endDate = new Date(now.getFullYear() + 1, Month.January, lastDayOfPreviousMonth);
            break;

        case DateRange.LastYear:
            startDate = new Date(now.getFullYear() - 1, Month.January, 1);
            endDate = new Date(now.getFullYear(), Month.January, lastDayOfPreviousMonth);
            break;

        case DateRange.AllTime:
            startDate = new Date(0);
            endDate = new Date(now);
            break;

        default:
            throw new Error("Error: Invalid date range.");
    }

    return { startDate, endDate };
};

export { getDatesFromDateRange };
