
import { Directive } from '@angular/core';
import { AutoComplete } from 'primeng/autocomplete';

@Directive({
    selector: 'p-autoComplete'
})

export class AutoCompleteDirective {
    constructor(public autoComplete: AutoComplete) {
        autoComplete.minLength = 3;
        autoComplete.delay = 500;
        autoComplete.placeholder = 'birşeyler yazın...';
        autoComplete.emptyMessage = 'sonuç bulunamadı...';
    }
}
