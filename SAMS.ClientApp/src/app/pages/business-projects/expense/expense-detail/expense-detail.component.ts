import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ExpenseService } from 'src/app/services/expense-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SelectItem } from 'primeng/api/selectitem';


@Component({
  selector: 'app-expense-detail',
  templateUrl: './expense-detail.component.html',
  styleUrls: ['./expense-detail.component.css'],

})
export class ExpenseDetailComponent extends BaseComponent implements OnInit {
  @Input() expenseId;
  @Output() onExpenseSaved = new EventEmitter<string>();
  expenseForm: FormGroup;
  expenseTypes: SelectItem[];

  constructor(private expenseService: ExpenseService, public router: Router) { super(); }

  ngOnInit() {
    this.createExpenseForm();
    if (this.expenseId > 0) {
      this.getExpenseById(this.expenseId);
    };
    this.cs.getExpenseTypes().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.expenseTypes = result.data;
      }
    });
  }

  createExpenseForm() {
    this.expenseForm = this.ch.formBuilder.group({
      id: [null],
      expenseTypeId: [null],
      name: [null, Validators.required],
    });
  }

  saveExpense() {
    if (!this.ch.validateAllFormFields(this.expenseForm, true)) {
      return;
    }
    const expenseId = this.ch.getControlValue(this.expenseForm, 'id');
    if (this.ch.isNullOrUndefined(expenseId)) {
      this.expenseService.createExpense(this.expenseForm.value).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onExpenseSaved.emit(this.expenseForm.value);
        }
      });
    }
    else {
      this.expenseService.updateExpense(this.expenseForm.value.id, this.expenseForm.value).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onExpenseSaved.emit(this.expenseForm.value);
        }
      });
    }
  }

  getExpenseById(expenseId) {
    this.expenseService.getExpenseById(expenseId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.expenseForm);
      }
    });
  }

  get expenseFormControls() {
    return this.expenseForm.controls;
  }
}
