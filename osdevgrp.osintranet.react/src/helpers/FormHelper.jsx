export default class FormHelper {
    asNullableString(value) {
        if (value === undefined || value === null) {
            return '';
        }

        return value;
    }

    asCurrency(value, replaceZeroWithEmpty = false) {
        if (value === undefined || value === null) {
            return this.#currencyFormatter(0, replaceZeroWithEmpty);
        }

        const numericValue = parseFloat(value);
        if (isNaN(numericValue)) {
            return this.#currencyFormatter(0, replaceZeroWithEmpty);
        }

        return this.#currencyFormatter(numericValue, replaceZeroWithEmpty);
    }

    #currencyFormatter(value, replaceZeroWithEmpty = false) {
        if (value === 0) {
            return replaceZeroWithEmpty ? '' : value.toFixed(2);
        }

        return value.toFixed(2);
    }
}