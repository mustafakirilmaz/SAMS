import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { ProportionalExpenseService } from 'src/app/services/proportional-expense-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-proportional-expense-list',
  templateUrl: './proportional-expense-list.component.html',
  styleUrls: ['./proportional-expense-list.component.css']
})
export class ProportionalExpenseListComponent extends BaseComponent implements OnInit {
  createProportionalExpenseModalVisible: boolean = false;
  gridName = 'proportionalExpenseGrid';
  selectedProportionalExpense: any;
  formGridColumns = [
    ['proportionalExpenseType', 'Oransal Gider Türü'],
    ['cost', 'Tutar'],
    ['operations', 'İşlem', ColumnType.Operation]
  ];
businessProjects: SelectItem[];
  

  constructor(public proportionalExpenseService: ProportionalExpenseService, private router: Router, private activatedRoute: ActivatedRoute) { super(); }

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
    this.proportionalExpenseService.getProportionalExpenseForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteProportionalExpense(proportionalExpenseId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.proportionalExpenseService.deleteProportionalExpense(proportionalExpenseId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editProportionalExpense(proportionalExpense) {
    this.selectedProportionalExpense = proportionalExpense;
    this.openProportionalExpenseModal();
  }

  openProportionalExpenseModal() {
    this.createProportionalExpenseModalVisible = true;
  }

  onProportionalExpenseSaved(event){
    this.createProportionalExpenseModalVisible = false;
    this.gridRefresh();
  }

  onHideProportionalExpenseModal(){
    this.selectedProportionalExpense = null;
  }

  setSelectedProportionalExpense(proportionalExpense: any) {
    this.selectedProportionalExpense = proportionalExpense;
  }
}
