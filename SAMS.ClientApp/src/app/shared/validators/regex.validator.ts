import { AbstractControl, ValidatorFn } from '@angular/forms';
import { RegexType, regexTypeDescriptions, regexTypeMessageDescriptions } from '../enums/regex-type';

export function ValidatorRegex(regexType: RegexType): ValidatorFn {
    return (control: AbstractControl) => {
        if (control.value) {
            const pattern = new RegExp(regexTypeDescriptions[RegexType[regexType]]);
            const result = pattern.test(control.value);
            if (result === false) {
                return { regex: { hataMesaji: regexTypeMessageDescriptions[RegexType[regexType]] } };
            }
        }
        return null;
    };
}
