import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { ExpenseService } from 'src/app/services/expense-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { Router } from '@angular/router';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css']
})
export class ExpenseListComponent extends BaseComponent implements OnInit {
  createExpenseModalVisible: boolean = false;
  expenseTypes: SelectItem[];

  gridName = 'expenseGrid';
  selectedExpense: any;
  formGridColumns = [
    ['expenseTypeName', 'Gider Türü'],
    ['name', 'Gider'],
    ['operations', 'İşlem', ColumnType.Operation]
  ];

  constructor(public expenseService: ExpenseService, private router: Router) { super(); }

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
      expenseTypeId: [null],
      name: [''],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.expenseService.getExpenseForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteExpense(expenseId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.expenseService.deleteExpense(expenseId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editExpense(expense) {
    this.selectedExpense = expense;
    this.openExpenseModal();
  }

  openExpenseModal() {
    this.createExpenseModalVisible = true;
  }

  onExpenseSaved(event) {
    this.createExpenseModalVisible = false;
    this.gridRefresh();
  }

  onHideExpenseModal() {
    this.selectedExpense = null;
  }

  setSelectedExpense(expense: any) {
    this.selectedExpense = expense;
  }
}
