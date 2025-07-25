export default class ValidationRuleSetHelper {
    getValidationRule(validationRuleSet, ruleName) {
        if (validationRuleSet === undefined || validationRuleSet === null) {
            throw new Error('Validation rule set is required.');
        }

        if (ruleName === undefined || ruleName === null || ruleName.trim() === '') {
            throw new Error('Rule name is required.');
        }

        const match = /^([A-Za-z0-9]+){1}:(RequiredValueRule|MinLengthRule|MaxLengthRule|ShouldBeIntegerRule|MinValueRule|MaxValueRule|PatternRule|OneOfRule){1}$/g.exec(ruleName);
        if (match === null || match.length !== 3) {
            throw Error('The rule name is not valid: ' + ruleName);
        }

        let found = undefined;
        switch (match[2]) {
            case 'RequiredValueRule':
                found = validationRuleSet.requiredValueRules.find(rule => rule.name === `${match[1]}:RequiredValueRule`);
                break;

            case 'MinLengthRule':
                found = validationRuleSet.minLengthRules.find(rule => rule.name === `${match[1]}:MinLengthRule`);
                break;

            case 'MaxLengthRule':
                found = validationRuleSet.maxLengthRules.find(rule => rule.name === `${match[1]}:MaxLengthRule`);
                break;

            case 'ShouldBeIntegerRule':
                found = validationRuleSet.shouldBeIntegerRules.find(rule => rule.name === `${match[1]}:ShouldBeIntegerRule`);
                break;

            case 'MinValueRule':
                found = validationRuleSet.minValueRules.find(rule => rule.name === `${match[1]}:MinValueRule`);
                break;

            case 'MaxValueRule':
                found = validationRuleSet.maxValueRules.find(rule => rule.name === `${match[1]}:MaxValueRule`);
                break;

            case 'PatternRule':
                found = validationRuleSet.patternRules.find(rule => rule.name === `${match[1]}:PatternRule`);
                break;

            case 'OneOfRule':
                found = validationRuleSet.oneOfRules.find(rule => rule.name === `${match[1]}:OneOfRule`);
                break;
        }

        if (found) {
            return found;
        }

        throw Error('No validation rule was found with the name: ' + ruleName);
    }
}