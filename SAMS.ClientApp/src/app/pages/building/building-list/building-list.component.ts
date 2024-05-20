import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { BuildingService } from 'src/app/services/building-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { Router } from '@angular/router';



@Component({
  selector: 'app-building-list',
  templateUrl: './building-list.component.html',
  styleUrls: ['./building-list.component.css']
})
export class BuildingListComponent extends BaseComponent implements OnInit {
  createBuildingModalVisible: boolean = false;
  gridName = 'buildingGrid';
  selectedBuilding: any;
  formGridColumns = [
    ['siteName', 'Site Adı'],
    ['name', 'Bina Adı'],
    ['city', 'İl'],
    ['town', 'İlçe'],
    ['district', 'Mahalle'],    
    ['units', 'Bağımsız Bölümler', ColumnType.Operation],
    ['operations', 'İşlem', ColumnType.Operation]
  ];
 
  constructor(public buildingService: BuildingService, private router: Router) { super(); }

  ngOnInit() {
    this.ch.clearComponent(this.gridName);
    this.createFilterForm();
    this.ch.createColumns(this.formGridColumns, this.gridName);
  }

  createFilterForm() {
    this.ch.setFilterForm(this.ch.formBuilder.group({
      name: [''],
      cityCode: [null],
      townCode: [null],
      districtCode: [null],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.buildingService.getBuildingForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteBuilding(buildingId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.buildingService.deleteBuilding(buildingId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editBuilding(building) {
    this.selectedBuilding = building;
    this.openBuildingModal();
  }

  openBuildingModal() {
    this.createBuildingModalVisible = true;
  }

  onBuildingSaved(event){
    this.createBuildingModalVisible = false;
    this.gridRefresh();
  }

  onHideBuildingModal(){
    this.selectedBuilding = null;
  }

  setSelectedBuilding(building: any) {
    this.selectedBuilding = building;
  }

  goToUnitList(buildingId: number) {
    // Seçilen binanın ID'sini alır ve unit-list sayfasına yönlendirir
    this.router.navigate(['/unit/list-unit', buildingId]);
  }
}
