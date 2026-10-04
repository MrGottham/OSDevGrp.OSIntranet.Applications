import { parse as parseGuid } from 'uuid';

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

    convertToInteger(value) {
        if (value === undefined || value === null) {
            return null;
        }

        const numericValue = parseInt(value, 10);
        if (isNaN(numericValue)) {
            return null;
        }

        return numericValue;
    }

    convertToUuid(value) {
        if (value === undefined || value === null) {
            return null;
        }

        try {
            parseGuid(value);
            return value;
        } catch {
            return null;
        }
    }

    convertToString(value, handleEmptyAsNull = false) {
        if (value === undefined || value === null) {
            return null;
        }

        const stringValue = String(value);
        if (handleEmptyAsNull && stringValue === '') {
            return null;
        }

        return stringValue;
    }

    convertToDecimal(value, handleEmptyAsNull = false, handleZeroAsNull = false) {
        if (value === undefined || value === null) {
            return null;
        }

        const numericValue = parseFloat(value);
        if (isNaN(numericValue)) {
            if (handleEmptyAsNull) {
                return null;
            }
            return null;
        }

        if (numericValue === 0 && handleZeroAsNull) {
            return null;
        }

        return numericValue;
    }

    #currencyFormatter(value, replaceZeroWithEmpty = false) {
        if (value === 0) {
            return replaceZeroWithEmpty ? '' : value.toFixed(2);
        }

        return value.toFixed(2);
    }
}