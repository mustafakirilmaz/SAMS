import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ProportionalExpenseService } from 'src/app/services/proportional-expense-service';
import { ActivatedRoute, Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SelectItem } from 'primeng/api/selectitem';


@Component({
  selector: 'app-proportional-expense-detail',
  templateUrl: './proportional-expense-detail.component.html',
  styleUrls: ['./proportional-expense-detail.component.css'],

})
export class ProportionalExpenseDetailComponent extends BaseComponent implements OnInit {
  @Input() proportionalExpenseId;
  @Output() onProportionalExpenseSaved = new EventEmitter<string>();
  proportionalExpenseForm: FormGroup;
  proportionalExpenseTypes: SelectItem[];
  businessProjects: SelectItem[];

  constructor(private proportionalExpenseService: ProportionalExpenseService, public router: Router, private activatedRoute: ActivatedRoute) { super(); }

  ngOnInit() {
    this.createProportionalExpenseForm();
    if (this.proportionalExpenseId > 0) {
      this.getProportionalExpenseById(this.proportionalExpenseId);
    };
    this.cs.getProportionalExpenseTypes().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.proportionalExpenseTypes = result.data;
      }
    });
    this.cs.getBusinessProjects().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.businessProjects = result.data;
      }
    });
  }
  
  createProportionalExpenseForm() {
    const businessProjectIdStr = this.activatedRoute.snapshot.params['businessProjectId'];
    const businessProjectIdAsNumber = this.ch.isNullOrUndefined(businessProjectIdStr) ? null : parseInt(businessProjectIdStr);
    this.proportionalExpenseForm = this.ch.formBuilder.group({
      id: [null],
      businessProjectId: [businessProjectIdAsNumber, Validators.required],
      proportionalExpenseType: [null, Validators.required],
      cost: [null, Validators.required]
    });
  }

  saveProportionalExpense() {
    if (!this.ch.validateAllFormFields(this.proportionalExpenseForm, true)) {
      return;
    }
    const proportionalExpenseId = this.ch.getControlValue(this.proportionalExpenseForm, 'id');
    const data = this.proportionalExpenseForm.getRawValue();
    if (this.ch.isNullOrUndefined(proportionalExpenseId)) {
      this.proportionalExpenseService.createProportionalExpense(data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onProportionalExpenseSaved.emit(data);
        }
      });
    }
    else {
      this.proportionalExpenseService.updateProportionalExpense(proportionalExpenseId, data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onProportionalExpenseSaved.emit(data);
        }
      });
    }
  }

  getProportionalExpenseById(proportionalExpenseId) {
    this.proportionalExpenseService.getProportionalExpenseById(proportionalExpenseId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.proportionalExpenseForm);
      }
    });
  }

  get proportionalExpenseFormControls() {
    return this.proportionalExpenseForm.controls;
  }
}
