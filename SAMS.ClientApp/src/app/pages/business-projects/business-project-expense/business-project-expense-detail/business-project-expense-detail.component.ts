import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { BusinessProjectExpenseService } from 'src/app/services/business-project-expense-service';
import { EqualExpenseService } from 'src/app/services/equal-expense-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-business-project-expense-detail',
  templateUrl: './business-project-expense-detail.component.html',
  styleUrls: ['./business-project-expense-detail.component.css'],

})

export class BusinessProjectExpenseDetailComponent extends BaseComponent implements OnInit {
  @Input() businessProjectExpenseId;
  businessProjectExpenseForm: FormGroup;
  expenseTypes: SelectItem[];
  expenses: SelectItem[];

  constructor(private businessProjectExpenseService: BusinessProjectExpenseService, private equalExpenseService: EqualExpenseService, public router: Router) { super(); }

  ngOnInit() {
    this.createBusinessProjectExpenseForm();
    if (this.businessProjectExpenseId > 0) {
      this.getBusinessProjectExpenseById(this.businessProjectExpenseId);
    };
    this.cs.getExpenseTypes().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.expenseTypes = result.data;
      }
    });
  }

  createBusinessProjectExpenseForm() {
    this.businessProjectExpenseForm = this.ch.formBuilder.group({
      id: [null],
      expenseTypeId: [null, Validators.required],
      expenseId: [null, Validators.required],
      cost: [null, Validators.required],
    });
  }

  saveBusinessProjectExpense(callback = null) {
    if (!this.ch.validateAllFormFields(this.businessProjectExpenseForm, true)) {
      return;
    }
    const businessProjectExpenseId = this.ch.getControlValue(this.businessProjectExpenseForm, 'id');
    const data = this.businessProjectExpenseForm.getRawValue();
    if (this.ch.isNullOrUndefined(businessProjectExpenseId)) {
      this.businessProjectExpenseService.createBusinessProjectExpense(data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          if (callback) callback(result);
        }
      });
    }
    else {
      this.businessProjectExpenseService.updateBusinessProjectExpense(businessProjectExpenseId, data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          if (callback) callback(result);
        }
      });
    }
  }

  getBusinessProjectExpenseById(businessProjectExpenseId) {
    this.businessProjectExpenseService.getBusinessProjectExpenseById(businessProjectExpenseId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.businessProjectExpenseForm);
      }
    });
  }

  onExpenseTypeChange(event: any) {
    this.cs.getExpenses(event.value).subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.expenses = result.data;
      }
    });
  }

  get businessProjectExpenseFormControls() {
    return this.businessProjectExpenseForm.controls;
  }

}