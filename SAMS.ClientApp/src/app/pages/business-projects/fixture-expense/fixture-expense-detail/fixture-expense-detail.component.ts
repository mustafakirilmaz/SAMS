import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { FixtureExpenseService } from 'src/app/services/fixture-expense-service';
import { ActivatedRoute, Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-fixture-expense-detail',
  templateUrl: './fixture-expense-detail.component.html',
  styleUrls: ['./fixture-expense-detail.component.css'],

})
export class FixtureExpenseDetailComponent extends BaseComponent implements OnInit {
  @Input() fixtureExpenseId;
  @Output() onFixtureExpenseSaved = new EventEmitter<string>();
  fixtureExpenseForm: FormGroup;
  fixtureExpenseTypes: SelectItem[];
  businessProjects: SelectItem[];


  constructor(private fixtureExpenseService: FixtureExpenseService, public router: Router, private activatedRoute: ActivatedRoute) { super(); }

  ngOnInit() {
    this.createFixtureExpenseForm();
    if (this.fixtureExpenseId > 0) {
      this.getFixtureExpenseById(this.fixtureExpenseId);
    };
    this.cs.getFixtureExpenseTypes().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.fixtureExpenseTypes = result.data;
      }
    });
    this.cs.getBusinessProjects().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.businessProjects = result.data;
      }
    });
    console.log(this.fixtureExpenseForm.getRawValue());
  }
  
  createFixtureExpenseForm() {
    const businessProjectIdStr = this.activatedRoute.snapshot.params['businessProjectId'];
    const businessProjectIdAsNumber = this.ch.isNullOrUndefined(businessProjectIdStr) ? null : parseInt(businessProjectIdStr);
    this.fixtureExpenseForm = this.ch.formBuilder.group({
      id: [null],
      businessProjectId: [businessProjectIdAsNumber, Validators.required],
      fixtureExpenseType: [null, Validators.required],      
      description: [null, Validators.required],
      cost: [null, Validators.required]
    });
  }
  
  saveFixtureExpense() {
    if (!this.ch.validateAllFormFields(this.fixtureExpenseForm, true)) {
      return;
    }
  
    const fixtureExpenseId = this.ch.getControlValue(this.fixtureExpenseForm, 'id');
    const data = this.fixtureExpenseForm.getRawValue();
  
    // URL'den businessProjectId'yi al
    //const businessProjectId = this.activatedRoute.snapshot.paramMap.get('businessProjectId');
    
    // businessProjectId'yi formda olmayan bir değere set et
    //this.ch.setControlValue(this.fixtureExpenseForm, 'businessProjectId', businessProjectId);
  
    if (this.ch.isNullOrUndefined(fixtureExpenseId)) {
      this.fixtureExpenseService.createFixtureExpense(data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onFixtureExpenseSaved.emit(data);
        }
      });
    } else {
      this.fixtureExpenseService.updateFixtureExpense(fixtureExpenseId, data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onFixtureExpenseSaved.emit(data);
        }
      });
    }
  }

  getFixtureExpenseById(fixtureExpenseId) {
    this.fixtureExpenseService.getFixtureExpenseById(fixtureExpenseId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.fixtureExpenseForm);
      }
    });
  }

  get fixtureExpenseFormControls() {
    return this.fixtureExpenseForm.controls;
  }
}
