
import { Directive } from '@angular/core';
import { SplitButton } from 'primeng/splitbutton';

@Directive({
    selector: 'p-splitButton'
})

export class SplitButtonDirective {
    constructor(public splitButton: SplitButton) {
        splitButton.appendTo = 'body';
    }
}
