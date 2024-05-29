import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { BusinessProjectExpenseService } from 'src/app/services/business-project-expense-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-business-project-expense-list',
  templateUrl: './business-project-expense-list.component.html',
  styleUrls: ['./business-project-expense-list.component.css']
})
export class BusinessProjectExpenseListComponent extends BaseComponent implements OnInit {
  createBusinessProjectExpenseModalVisible = false;
  showEqualExpenseDialog = false;
  gridName = 'businessProjectExpenseGrid';
  selectedBusinessProjectExpense: any;
  formGridColumns = [
    ['expenseType', 'Gider Türü'],
    ['expense', 'Gider'],
    ['cost', 'Tutar', ColumnType.Money],
    ['operations', 'İşlem', ColumnType.Operation]
  ];
  expenseTypes: SelectItem[];

  constructor(public businessProjectExpenseService: BusinessProjectExpenseService, private router: Router, private activatedRoute: ActivatedRoute) { super(); }

  ngOnInit() {
    this.ch.clearComponent(this.gridName);
    this.createFilterForm();
    this.ch.createColumns(this.formGridColumns, this.gridName);
    this.cs.getExpenseTypes().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.expenseTypes = result.data;
      }
    });
  }

  createFilterForm() {
    this.ch.setFilterForm(this.ch.formBuilder.group({
      expenseType: [null],
      expense: [null],
      cost: [''],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.businessProjectExpenseService.getBusinessProjectExpenseForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteBusinessProjectExpense(businessProjectExpenseId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.businessProjectExpenseService.deleteBusinessProjectExpense(businessProjectExpenseId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editBusinessProjectExpense(businessProjectExpense) {
    this.selectedBusinessProjectExpense = businessProjectExpense;
    this.openBusinessProjectExpenseModal();
  }

  openBusinessProjectExpenseModal() {
    this.createBusinessProjectExpenseModalVisible = true;
  }

  onBusinessProjectExpenseSaved(event) {
    this.createBusinessProjectExpenseModalVisible = false;
    this.gridRefresh();
  }

  onHideBusinessProjectExpenseModal() {
    this.selectedBusinessProjectExpense = null;
  }

  setSelectedBusinessProjectExpense(businessProjectExpense: any) {
    this.selectedBusinessProjectExpense = businessProjectExpense;
  }
}