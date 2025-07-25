import { setLocale, string, number } from 'yup';
import { da } from 'yup-locales';
import ValidationRuleSetHelper from "./ValidationRuleSetHelper";
import { parse } from '@fortawesome/fontawesome-svg-core';

export default class ValidationSchemaHelper {
    #validationRuleSetHelper = new ValidationRuleSetHelper();

    constructor() {
        setLocale(da);
    }

    forString(validationRuleSet, validationFor, options) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (options === undefined || options === null) {
            throw new Error('Options is required.');
        }

        let schema = string();

        schema = this.#withRequiredValueRule(validationRuleSet, validationFor, schema, options.withRequiredValueRule);
        schema = this.#withMinLengthRule(validationRuleSet, validationFor, schema, options.withMinLengthRule);
        schema = this.#withMaxLengthRule(validationRuleSet, validationFor, schema, options.withMaxLengthRule);
        schema = this.#withPatternRule(validationRuleSet, validationFor, schema, options.withPatternRule);
        schema = this.#withOneOfRule(validationRuleSet, validationFor, schema, options.withOneOfRule, value => value);

        return schema;
    }

    forInteger(validationRuleSet, validationFor, options) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (options === undefined || options === null) {
            throw new Error('Options is required.');
        }

        let schema = this.#localizedNumber();

        schema = this.#withRequiredValueRule(validationRuleSet, validationFor, schema, options.withRequiredValueRule);
        schema = this.#withShouldBeIntegerRule(validationRuleSet, validationFor, schema);
        schema = this.#withMinValueRule(validationRuleSet, validationFor, schema, options.withMinValueRule, value => parseInt(value));
        schema = this.#withMaxValueRule(validationRuleSet, validationFor, schema, options.withMaxValueRule, value => parseInt(value));
        schema = this.#withOneOfRule(validationRuleSet, validationFor, schema, options.withOneOfRule, value => parseInt(value));

        return schema;
    }

    #withRequiredValueRule(validationRuleSet, validationFor, schema, condition) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (schema === undefined || schema === null) {
            throw new Error('Schema is required.');
        }

        if (condition === undefined || condition === null || condition !== true) {
            return schema;
        }

        const requiredValueRule = this.#validationRuleSetHelper.getValidationRule(validationRuleSet, `${validationFor}:RequiredValueRule`);
        return schema.required(requiredValueRule.validationError);
    }

    #withMinLengthRule(validationRuleSet, validationFor, schema, condition) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (schema === undefined || schema === null) {
            throw new Error('Schema is required.');
        }

        if (condition === undefined || condition === null || condition !== true) {
            return schema;
        }

        const minLengthRule = this.#validationRuleSetHelper.getValidationRule(validationRuleSet, `${validationFor}:MinLengthRule`);
        return schema.min(parseInt(minLengthRule.value), minLengthRule.validationError);
    }

    #withMaxLengthRule(validationRuleSet, validationFor, schema, condition) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (schema === undefined || schema === null) {
            throw new Error('Schema is required.');
        }

        if (condition === undefined || condition === null || condition !== true) {
            return schema;
        }

        const maxLengthRule = this.#validationRuleSetHelper.getValidationRule(validationRuleSet, `${validationFor}:MaxLengthRule`);
        return schema.max(parseInt(maxLengthRule.value), maxLengthRule.validationError);
    }

    #withShouldBeIntegerRule(validationRuleSet, validationFor, schema) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (schema === undefined || schema === null) {
            throw new Error('Schema is required.');
        }

        const shouldBeIntegerRule = this.#validationRuleSetHelper.getValidationRule(validationRuleSet, `${validationFor}:ShouldBeIntegerRule`);
        return schema.integer(shouldBeIntegerRule.validationError);
    }

    #withMinValueRule(validationRuleSet, validationFor, schema, condition, valueMapper) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (schema === undefined || schema === null) {
            throw new Error('Schema is required.');
        }

        if (valueMapper === undefined || valueMapper === null) {
            throw new Error('Value mapper is required.');
        }

        if (condition === undefined || condition === null || condition !== true) {
            return schema;
        }

        const minValueRule = this.#validationRuleSetHelper.getValidationRule(validationRuleSet, `${validationFor}:MinValueRule`);
        return schema.min(valueMapper(minValueRule.value), minValueRule.validationError);
    }

    #withMaxValueRule(validationRuleSet, validationFor, schema, condition, valueMapper) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (schema === undefined || schema === null) {
            throw new Error('Schema is required.');
        }

        if (valueMapper === undefined || valueMapper === null) {
            throw new Error('Value mapper is required.');
        }

        if (condition === undefined || condition === null || condition !== true) {
            return schema;
        }

        const maxValueRule = this.#validationRuleSetHelper.getValidationRule(validationRuleSet, `${validationFor}:MaxValueRule`);
        return schema.max(valueMapper(maxValueRule.value), maxValueRule.validationError);
    }

    #withPatternRule(validationRuleSet, validationFor, schema, condition) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (schema === undefined || schema === null) {
            throw new Error('Schema is required.');
        }

        if (condition === undefined || condition === null || condition !== true) {
            return schema;
        }

        const patternRule = this.#validationRuleSetHelper.getValidationRule(validationRuleSet, `${validationFor}:PatternRule`);
        return schema.matches(new RegExp(patternRule.value, 'g'), patternRule.validationError);
    }

    #withOneOfRule(validationRuleSet, validationFor, schema, condition, valueMapper) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (validationFor === undefined || validationFor === null || validationFor.trim() === '') {
            throw new Error('Validation for is required.');
        }

        if (schema === undefined || schema === null) {
            throw new Error('Schema is required.');
        }

        if (valueMapper === undefined || valueMapper === null) {
            throw new Error('Value mapper is required.');
        }

        if (condition === undefined || condition === null || condition !== true) {
            return schema;
        }

        const oneOfRule = this.#validationRuleSetHelper.getValidationRule(validationRuleSet, `${validationFor}:OneOfRule`);
        return schema.oneOf(oneOfRule.values.map(value => valueMapper(value)), oneOfRule.validationError);
    }

    #localizedNumber()
    {
        return number()
            .transform((value, originalValue, context) => {
                if (context.isType(value)) {
                    return value;
                }

                if (originalValue === undefined || originalValue === null) {
                    return 0;
                }

                return parseFloat(originalValue.replace(',', '.'));
            });
    }
}