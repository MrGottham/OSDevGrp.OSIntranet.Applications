export default class DateHelper {
    getCurrentDate() {
        return this.getDateOnly(new Date());
    }

    getDateOnly(date) {
        if (date === null || date === undefined) {
            throw new Error('Date is required.');
        }

        const dateOnly = new Date(date);
        dateOnly.setHours(0, 0, 0, 0);
        return dateOnly;
    }

    convertToIsoString(date) {
        if (date === null || date === undefined) {
            throw new Error('Date is required.');
        }

        if (!(date instanceof Date)) {
            throw new Error('Date must be a Date object.');
        }

        return date.toISOString();
    }
}