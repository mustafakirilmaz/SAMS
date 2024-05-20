import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { EqualExpenseService } from 'src/app/services/equal-expense-service';
import { ActivatedRoute, Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-equal-expense-detail',
  templateUrl: './equal-expense-detail.component.html',
  styleUrls: ['./equal-expense-detail.component.css'],

})
export class EqualExpenseDetailComponent extends BaseComponent implements OnInit {
  @Input() equalExpenseId;
  @Output() onEqualExpenseSaved = new EventEmitter<string>();
  equalExpenseForm: FormGroup;
  equalExpenseTypes: SelectItem[];
  businessProjects: SelectItem[];


  constructor(private equalExpenseService: EqualExpenseService, public router: Router, private activatedRoute: ActivatedRoute) { super(); }

  ngOnInit() {
    this.createEqualExpenseForm();
    if (this.equalExpenseId > 0) {
      this.getEqualExpenseById(this.equalExpenseId);
    };
    this.cs.getEqualExpenseTypes().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.equalExpenseTypes = result.data;
      }
    });
    this.cs.getBusinessProjects().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.businessProjects = result.data;
      }
    });
    console.log(this.equalExpenseForm.getRawValue());
  }
  
  createEqualExpenseForm() {
    const businessProjectIdStr = this.activatedRoute.snapshot.params['businessProjectId'];
    const businessProjectIdAsNumber = this.ch.isNullOrUndefined(businessProjectIdStr) ? null : parseInt(businessProjectIdStr);
    this.equalExpenseForm = this.ch.formBuilder.group({
      id: [null],
      businessProjectId: [businessProjectIdAsNumber, Validators.required],
      equalExpenseType: [null, Validators.required],
      cost: [null, Validators.required]
    });
  }
  
  saveEqualExpense() {
    if (!this.ch.validateAllFormFields(this.equalExpenseForm, true)) {
      return;
    }
  
    const equalExpenseId = this.ch.getControlValue(this.equalExpenseForm, 'id');
    const data = this.equalExpenseForm.getRawValue();
  
    // URL'den businessProjectId'yi al
    //const businessProjectId = this.activatedRoute.snapshot.paramMap.get('businessProjectId');
    
    // businessProjectId'yi formda olmayan bir değere set et
    //this.ch.setControlValue(this.equalExpenseForm, 'businessProjectId', businessProjectId);
  
    if (this.ch.isNullOrUndefined(equalExpenseId)) {
      this.equalExpenseService.createEqualExpense(data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onEqualExpenseSaved.emit(data);
        }
      });
    } else {
      this.equalExpenseService.updateEqualExpense(equalExpenseId, data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onEqualExpenseSaved.emit(data);
        }
      });
    }
  }

  getEqualExpenseById(equalExpenseId) {
    this.equalExpenseService.getEqualExpenseById(equalExpenseId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.equalExpenseForm);
      }
    });
  }

  get equalExpenseFormControls() {
    return this.equalExpenseForm.controls;
  }
}
