import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ExpenseTypeService } from 'src/app/services/expense-type-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';

@Component({
  selector: 'app-expense-type-detail',
  templateUrl: './expense-type-detail.component.html',
  styleUrls: ['./expense-type-detail.component.css'],

})
export class ExpenseTypeDetailComponent extends BaseComponent implements OnInit {
  @Input() expenseTypeId;
  @Output() onExpenseTypeSaved = new EventEmitter<string>();
  expenseTypeForm: FormGroup;

  constructor(private expenseTypeService: ExpenseTypeService, public router: Router) { super(); }

  ngOnInit() {
    this.createExpenseTypeForm();
    if (this.expenseTypeId > 0) {
      this.getExpenseTypeById(this.expenseTypeId);
    }
  }

  createExpenseTypeForm() {
    this.expenseTypeForm = this.ch.formBuilder.group({
      id: [null],
      name: [null, Validators.required],
    });
  }

  saveExpenseType() {
    if (!this.ch.validateAllFormFields(this.expenseTypeForm, true)) {
      return;
    }
    const expenseTypeId = this.ch.getControlValue(this.expenseTypeForm, 'id');
    if (this.ch.isNullOrUndefined(expenseTypeId)) {
      this.expenseTypeService.createExpenseType(this.expenseTypeForm.value).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onExpenseTypeSaved.emit(this.expenseTypeForm.value);
        }
      });
    }
    else {
      this.expenseTypeService.updateExpenseType(this.expenseTypeForm.value.id, this.expenseTypeForm.value).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onExpenseTypeSaved.emit(this.expenseTypeForm.value);
        }
      });
    }
  }

  getExpenseTypeById(expenseTypeId) {
    this.expenseTypeService.getExpenseTypeById(expenseTypeId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.expenseTypeForm);
      }
    });
  }

  get expenseTypeFormControls() {
    return this.expenseTypeForm.controls;
  }
}
