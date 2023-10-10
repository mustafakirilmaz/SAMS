import { Component, Input, OnInit, forwardRef, EventEmitter, Output } from '@angular/core';
import { BaseComponent } from '../../bases/base.component';
import { FormGroup } from '@angular/forms';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { HttpHelper } from 'src/app/services/http-helper.service';
import { SelectItem } from 'primeng/api';
import ServiceResult from '../../models/service-result';
import { debug } from 'console';

@Component({
  selector: 'app-autoComplete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.css']
})
export class AutoCompleteComponent extends BaseComponent implements OnInit {
  @Input() parentForm;
  @Input() controlName: string;
  @Input() controlInfoName: string;
  @Input() controllerName: string;
  @Input() actionName: string;
  @Input() validator: any;
  @Input() parentId: any;
  @Output() selectCallback = new EventEmitter<any>();

  filteredItems: SelectItem[];
  selectedValue: string[] = [];
  timer: any;
  attempCount: number = 0;
  isDisabled = false;

  constructor(private httpHelper: HttpHelper) {
    super();
  }

  ngOnInit() {
    this.timer = setInterval(() => {
      const initialValue = this.parentForm.get(this.controlName).value
      if (!this.ch.isNullOrWhiteSpace(initialValue) && typeof (initialValue) === 'number') {
        clearInterval(this.timer);
        setTimeout(() => {
          this.search(null, initialValue).subscribe((result: ServiceResult<SelectItem[]>) => {
            if (this.ch.checkResult(result)) {
              this.selectedValue = [result.data[0].label];
              this.isDisabled = this.parentForm.get(this.controlName).disabled;
              this.selectCallback.emit(result.data[0]);
            }
          });
        });
      }
      this.attempCount++;
      if (this.attempCount == 100) {
        clearInterval(this.timer);
      }
    }, 50);

    this.parentForm.get(this.controlName).valueChanges.subscribe((val) => {
      if (this.ch.isNullOrUndefined(val)) {
        this.selectedValue = [];
      }
    });
  }

  searchFn(searchTerm) {
    this.search(searchTerm.query, null).subscribe((result: ServiceResult<SelectItem[]>) => {
      if (this.ch.checkResult(result)) {
        this.filteredItems = result.data;
      }
    });
  }

  search(searchTerm, id) {
    const params = this.ch.createParams({ id: id, searchTerm: searchTerm, parentId: this.parentId });
    return this.httpHelper.get<SelectItem[]>(this.controllerName, this.actionName, params);
  }

  onSelect(selected) {
    this.selectedValue = [selected.label];
    this.parentForm.get(this.controlName).setValue(selected.value);
    if (!this.ch.isNullOrUndefined(this.controlInfoName)) {
      this.parentForm.get(this.controlInfoName).setValue(selected.label);
    }
    this.selectCallback.emit(selected);
  }

  onRemove() {
    this.parentForm.get(this.controlName).setValue(null);
    if (!this.ch.isNullOrUndefined(this.controlInfoName)) {
      this.parentForm.get(this.controlInfoName).setValue(null);
    }
  }
}
