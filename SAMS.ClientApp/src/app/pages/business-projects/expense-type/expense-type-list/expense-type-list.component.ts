import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { ExpenseTypeService } from 'src/app/services/expense-type-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { Router } from '@angular/router';

@Component({
  selector: 'app-expense-type-list',
  templateUrl: './expense-type-list.component.html',
  styleUrls: ['./expense-type-list.component.css']
})
export class ExpenseTypeListComponent extends BaseComponent implements OnInit {
  createExpenseTypeModalVisible: boolean = false;
  gridName = 'expenseTypeGrid';
  selectedExpenseType: any;
  formGridColumns = [
    ['name', 'Gider Türü'],
    ['operations', 'İşlem', ColumnType.Operation]
  ];

  constructor(public expenseTypeService: ExpenseTypeService, private router: Router) { super(); }

  ngOnInit() {
    this.ch.clearComponent(this.gridName);
    this.createFilterForm();
    this.ch.createColumns(this.formGridColumns, this.gridName);
  }

  createFilterForm() {
    this.ch.setFilterForm(this.ch.formBuilder.group({
      name: [''],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.expenseTypeService.getExpenseTypeForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteExpenseType(expenseTypeId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.expenseTypeService.deleteExpenseType(expenseTypeId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editExpenseType(expenseType) {
    this.selectedExpenseType = expenseType;
    this.openExpenseTypeModal();
  }

  openExpenseTypeModal() {
    this.createExpenseTypeModalVisible = true;
  }

  onExpenseTypeSaved(event) {
    this.createExpenseTypeModalVisible = false;
    this.gridRefresh();
  }

  onHideExpenseTypeModal() {
    this.selectedExpenseType = null;
  }

  setSelectedExpenseType(expenseType: any) {
    this.selectedExpenseType = expenseType;
  }
}
