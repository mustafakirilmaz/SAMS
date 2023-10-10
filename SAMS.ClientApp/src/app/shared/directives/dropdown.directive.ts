
import { Directive } from '@angular/core';
import { Dropdown } from 'primeng/dropdown';

@Directive({
    selector: 'p-dropdown'
})

export class DropdownDirective {
    constructor(public dropdown: Dropdown) {
        dropdown.filter = true;
        dropdown.appendTo="body";
        dropdown.autofocus = true;
        dropdown.filterBy = "value,label";
        dropdown.emptyMessage = dropdown.emptyFilterMessage = "sonuç bulunamadı...";
        dropdown.placeholder="Seçiniz..."
    }
}
