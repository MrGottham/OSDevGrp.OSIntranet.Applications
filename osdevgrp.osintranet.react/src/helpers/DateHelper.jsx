export default class DateHelper {
    getCurrentDate() {
        return this.getDateOnly(new Date());
    }

    getDateOnly(date) {
        const dateOnly = new Date(date);
        dateOnly.setHours(0, 0, 0, 0);
        return dateOnly;
    }
}