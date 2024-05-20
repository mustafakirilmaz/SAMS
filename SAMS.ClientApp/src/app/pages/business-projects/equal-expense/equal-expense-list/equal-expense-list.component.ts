import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { EqualExpenseService } from 'src/app/services/equal-expense-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-equal-expense-list',
  templateUrl: './equal-expense-list.component.html',
  styleUrls: ['./equal-expense-list.component.css']
})
export class EqualExpenseListComponent extends BaseComponent implements OnInit {
  @Input() businessProjectId: any = null;
  createEqualExpenseModalVisible: boolean = false;
  gridName = 'equalExpenseGrid';
  selectedEqualExpense: any;
  formGridColumns = [
    ['equalExpenseType', 'Eşit Gider Türü'],
    ['cost', 'Tutar'],
    ['operations', 'İşlem', ColumnType.Operation]
  ];
  businessProjects: SelectItem[];
  constructor(public equalExpenseService: EqualExpenseService, private router: Router, private activatedRoute: ActivatedRoute) { super(); }

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
      const businessProjectId = params['businessProjectId'];
      if (businessProjectId) {
        this.ch.getFilterForm(this.gridName).get('businessProjectId').setValue(businessProjectId);
        this.gridRefresh();
      }
    });

    if (this.businessProjectId) {
      this.ch.getFilterForm(this.gridName).get('businessProjectId').setValue(this.businessProjectId);
      this.gridRefresh();
    }
  }

  createFilterForm() {
    this.ch.setFilterForm(this.ch.formBuilder.group({
      businessProjectId: [null],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.equalExpenseService.getEqualExpenseForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
      // const total = 0;
      // for (const item of result.data) {
      //   total += item.amount;
      // }
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteEqualExpense(equalExpenseId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.equalExpenseService.deleteEqualExpense(equalExpenseId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editEqualExpense(equalExpense) {
    this.selectedEqualExpense = equalExpense;
    this.openEqualExpenseModal();
  }

  openEqualExpenseModal() {
    this.createEqualExpenseModalVisible = true;
  }

  onEqualExpenseSaved(event) {
    this.createEqualExpenseModalVisible = false;
    this.gridRefresh();
  }

  onHideEqualExpenseModal() {
    this.selectedEqualExpense = null;
  }

  setSelectedEqualExpense(equalExpense: any) {
    this.selectedEqualExpense = equalExpense;
  }
}
