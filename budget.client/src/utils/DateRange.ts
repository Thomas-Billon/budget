import { DateRange } from "@/enums/DateRange";
import { Month } from "@/enums/Month";
import { getIsoDay } from "@/utils/Utils";

const lastDayOfPreviousMonth: number = 0;

const getLocalDatesFromDateRange = (dateRange: DateRange): { startDate: Date, endDate: Date } => {
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
            startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - getIsoDay(now));
            endDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() + (6 - getIsoDay(now)));
            break;

        case DateRange.LastWeek:
            startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - getIsoDay(now) - 7);
            endDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - getIsoDay(now) - 1);
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

const getUtcDatesFromDateRange = (dateRange: DateRange): { startDate: Date, endDate: Date } => {
    let { startDate, endDate } = getLocalDatesFromDateRange(dateRange);

    startDate = new Date(startDate.getTime() - startDate.getTimezoneOffset() * 60000);
    endDate = new Date(endDate.getTime() - endDate.getTimezoneOffset() * 60000);

    return { startDate, endDate };
};

export { getLocalDatesFromDateRange, getUtcDatesFromDateRange };
