import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { BusinessProjectService } from 'src/app/services/business-project-service';
import { EqualExpenseService } from 'src/app/services/equal-expense-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-business-project-detail',
  templateUrl: './business-project-detail.component.html',
  styleUrls: ['./business-project-detail.component.css'],

})

export class BusinessProjectDetailComponent extends BaseComponent implements OnInit {
  @Input() businessProjectId;
  businessProjectForm: FormGroup;
  buildings: SelectItem[];


  constructor(private businessProjectService: BusinessProjectService, private equalExpenseService: EqualExpenseService, public router: Router) { super(); }

  ngOnInit() {
    this.createBusinessProjectForm();
    if (this.businessProjectId > 0) {
      this.getBusinessProjectById(this.businessProjectId);
    };
    this.cs.getBuildings().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.buildings = result.data;
      }
    });
  }

  createBusinessProjectForm() {
    this.businessProjectForm = this.ch.formBuilder.group({
      id: [null],
      buildingId: [null, Validators.required],
      name: [null, Validators.required],
      startDate: [null, Validators.required],
      endDate: [null, Validators.required],
    });
  }

  saveBusinessProject(callback = null) {
    if (!this.ch.validateAllFormFields(this.businessProjectForm, true)) {
      return;
    }
    const businessProjectId = this.ch.getControlValue(this.businessProjectForm, 'id');
    const data = this.businessProjectForm.getRawValue();
    if (this.ch.isNullOrUndefined(businessProjectId)) {
      this.businessProjectService.createBusinessProject(data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          if (callback) callback(result);
        }
      });
    }
    else {
      this.businessProjectService.updateBusinessProject(businessProjectId, data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          if (callback) callback(result);
        }
      });
    }
  }

  getBusinessProjectById(businessProjectId) {
    this.businessProjectService.getBusinessProjectById(businessProjectId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.businessProjectForm);
      }
    });
  }

  get businessProjectFormControls() {
    return this.businessProjectForm.controls;
  }

  // getEqualExpensesByBusinessProjectId(businessProjectId) {
  //   this.equalExpenseService.getEqualExpensesByBusinessProjectId(businessProjectId).subscribe((result: ServiceResult<object[]>) => {
  //     if (this.ch.checkResult(result)) 
  //       this.equalExpenses = result.data.map(expense => ({
  //       id: expense.id,
  //       equalExpenseType: expense.equalExpenseType,
  //       cost: expense.cost
  //     }));
  // }

}

export interface Product {
  id: number;
  name: string;
  price: number;
  category: string;
}
