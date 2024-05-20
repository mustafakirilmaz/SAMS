import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { FixtureExpenseService } from 'src/app/services/fixture-expense-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-fixture-expense-list',
  templateUrl: './fixture-expense-list.component.html',
  styleUrls: ['./fixture-expense-list.component.css']
})
export class FixtureExpenseListComponent extends BaseComponent implements OnInit {
  createFixtureExpenseModalVisible: boolean = false;
  gridName = 'fixtureExpenseGrid';
  selectedFixtureExpense: any;
  formGridColumns = [
    ['fixtureExpenseType', 'Demirbaş Türü'],
    ['description', 'Açıklama'],
    ['cost', 'Tutar'],
    ['operations', 'İşlem', ColumnType.Operation]
  ];
businessProjects: SelectItem[];
  constructor(public fixtureExpenseService: FixtureExpenseService, private router: Router, private activatedRoute: ActivatedRoute) { super(); }

  ngOnInit() {
    this.ch.clearComponent(this.gridName);
    this.createFilterForm();
    this.ch.createColumns(this.formGridColumns, this.gridName);        
    this.cs.getBusinessProjects().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.businessProjects = result.data;
      }
    });
    this.activatedRoute.params.subscribe(params => {
      const businessProjectId = +params['businessProjectId'];
      if (!isNaN(businessProjectId)) {
        this.ch.getFilterForm(this.gridName).get('businessProjectId').setValue(businessProjectId);
        this.gridRefresh();
      }
    });
  }

  createFilterForm() {
    this.ch.setFilterForm(this.ch.formBuilder.group({
      businessProjectId:[null],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.fixtureExpenseService.getFixtureExpenseForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteFixtureExpense(fixtureExpenseId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.fixtureExpenseService.deleteFixtureExpense(fixtureExpenseId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editFixtureExpense(fixtureExpense) {
    this.selectedFixtureExpense = fixtureExpense;
    this.openFixtureExpenseModal();
  }

  openFixtureExpenseModal() {
    this.createFixtureExpenseModalVisible = true;
  }

  onFixtureExpenseSaved(event){
    this.createFixtureExpenseModalVisible = false;
    this.gridRefresh();
  }

  onHideFixtureExpenseModal(){
    this.selectedFixtureExpense = null;
  }

  setSelectedFixtureExpense(fixtureExpense: any) {
    this.selectedFixtureExpense = fixtureExpense;
  }
}
