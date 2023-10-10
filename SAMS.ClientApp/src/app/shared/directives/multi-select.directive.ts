
import { Directive } from '@angular/core';
import { MultiSelect } from 'primeng/multiselect';

@Directive({
    selector: 'p-multiSelect'
})

export class MultiSelectDirective {
    constructor(public multiSelect: MultiSelect) {
        multiSelect.defaultLabel = "Seçiniz";
        multiSelect.selectedItemsLabel = "{0} öğre seçili"
    }
}
