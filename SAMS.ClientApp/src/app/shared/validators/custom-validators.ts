import { AbstractControl, ValidatorFn } from '@angular/forms';
import { AppInjector } from '../bases/app-injector.service';
import { CustomValidatorMessageDescriptions } from '../enums/custom-validator-messages';
import { CommonHelper } from '../helpers/common-helper';

export function MinLengthArrayValidator(min: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        if (control.value.length >= min) {
            return null;
        }

        const message = CustomValidatorMessageDescriptions.MinArrayLength.replace('{count}', min.toString());
        return { regex: { errorMessage: message } };
    };
}


export function MaxLengthArrayValidator(max: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        if (control.value.length <= max) {
            return null;
        }

        const message = CustomValidatorMessageDescriptions.MaxArrayLength.replace('{count}', max.toString());
        return { regex: { errorMessage: message } };
    };
}

export function TCNOKontrolValidator(control: AbstractControl): { [key: string]: any } | null {
    const injector = AppInjector.getInjector();
    const ch = injector.get(CommonHelper);
    if (ch.TCNOKontrol(control.value)) {
        return null;
    }
    const message = CustomValidatorMessageDescriptions.TCNoMessage;
    return { regex: { errorMessage: message } };
}
