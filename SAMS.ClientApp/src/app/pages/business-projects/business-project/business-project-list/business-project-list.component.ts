import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { BusinessProjectService } from 'src/app/services/business-project-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectItem } from 'primeng/api/selectitem';

@Component({
  selector: 'app-business-project-list',
  templateUrl: './business-project-list.component.html',
  styleUrls: ['./business-project-list.component.css']
})
export class BusinessProjectListComponent extends BaseComponent implements OnInit {
  createBusinessProjectModalVisible = false;
  showEqualExpenseDialog = false;
  gridName = 'businessProjectGrid';
  selectedBusinessProject: any;
  formGridColumns = [
    ['buildingName', 'Bina Adı'],
    ['name', 'İşletme Projesi Adı'],
    ['startDate', 'Geçerlilik Başlangıç Tarihi', ColumnType.Date],
    ['endDate', 'Geçerlilik Bitiş Tarihi', ColumnType.Date],
    ['details', 'İşletme Projesi Detayları', ColumnType.Operation],
    ['operations', 'İşlem', ColumnType.Operation]
  ];
  buildings: SelectItem[];

  constructor(public businessProjectService: BusinessProjectService, private router: Router, private activatedRoute: ActivatedRoute) { super(); }

  ngOnInit() {
    this.ch.clearComponent(this.gridName);
    this.createFilterForm();
    this.ch.createColumns(this.formGridColumns, this.gridName);
    this.cs.getBuildings().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.buildings = result.data;
      }
    });
    this.activatedRoute.params.subscribe(params => {
      const buildingId = +params['buildingId'];
      if (!isNaN(buildingId)) {
        this.ch.getFilterForm(this.gridName).get('buildingId').setValue(buildingId);
        this.gridRefresh();
      }
    });
  }

  createFilterForm() {
    this.ch.setFilterForm(this.ch.formBuilder.group({
      siteId: [null],
      buildingId: [null],
      name: [''],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.businessProjectService.getBusinessProjectForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteBusinessProject(businessProjectId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.businessProjectService.deleteBusinessProject(businessProjectId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editBusinessProject(businessProject) {
    this.selectedBusinessProject = businessProject;
    this.openBusinessProjectModal();
  }

  openBusinessProjectModal() {
    this.createBusinessProjectModalVisible = true;
  }

  onBusinessProjectSaved(event) {
    this.createBusinessProjectModalVisible = false;
    this.gridRefresh();
  }

  onHideBusinessProjectModal() {
    this.selectedBusinessProject = null;
  }

  setSelectedBusinessProject(businessProject: any) {
    this.selectedBusinessProject = businessProject;
  }

  showEqualExpenseListDialog(businessProject) {
    this.selectedBusinessProject = businessProject;
    this.showEqualExpenseDialog = true;
  }

  onEqualExpenseSaved() {
    this.showEqualExpenseDialog = false;
  }

  // goToEqualExpenses(businessProjectId: number) {
  //   // Seçilen işletme projesinin ID'sini alır ve equal-expense-list sayfasına yönlendirir.
  //   this.router.navigate(['/equal-expense/list-equal-expense', businessProjectId]);
  // }

  goToProportionalExpenses(businessProjectId: number) {
    // Seçilen işletme projesinin ID'sini alır ve proportional-expense-list sayfasına yönlendirir.
    this.router.navigate(['/proportional-expense/list-proportional-expense', businessProjectId]);
  }

  goToFixtureExpenses(businessProjectId: number) {
    // Seçilen işletme projesinin ID'sini alır ve fixture-expense-list sayfasına yönlendirir.
    this.router.navigate(['/fixture-expense/list-fixture-expense', businessProjectId]);
  }
}