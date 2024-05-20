import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { UnitService } from 'src/app/services/unit-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectItem } from 'primeng/api/selectitem';
import { Console } from 'console';

@Component({
  selector: 'app-unit-list',
  templateUrl: './unit-list.component.html',
  styleUrls: ['./unit-list.component.css']
})
export class UnitListComponent extends BaseComponent implements OnInit {
  createUnitModalVisible: boolean = false;
  gridName = 'unitGrid';
  selectedUnit: any;
  formGridColumns = [
    ['siteName', 'Site Adı'],
    ['buildingName', 'Bina Adı'],
    ['unitName', 'Bağımsız Bölüm Adı'],
    ['netSquareMeter', 'Net Metre kare'],
    ['grossSquareMeter', 'Brüt Metre kare'],
    ['shareOfLand', 'Arsa Payı'],    
    ['borc', 'Borçlar'],
    ['operations', 'İşlem', ColumnType.Operation]
  ];  
  buildings: SelectItem[];
  sites: SelectItem[];

  constructor(public unitService: UnitService, private router: Router, private activatedRoute: ActivatedRoute) { super(); }

  ngOnInit() {
    this.ch.clearComponent(this.gridName);
    this.createFilterForm();
    this.ch.createColumns(this.formGridColumns, this.gridName);    
    this.cs.getSites().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.sites = result.data;
      }
    });
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
      siteId:[null],
      buildingId:[null],
      name: [''],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.unitService.getUnitForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteUnit(unitId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.unitService.deleteUnit(unitId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editUnit(unit) {
    this.selectedUnit = unit;
    this.openUnitModal();
  }

  openUnitModal() {
    this.createUnitModalVisible = true;
  }

  onUnitSaved(event){
    this.createUnitModalVisible = false;
    this.gridRefresh();
  }

  onHideUnitModal(){
    this.selectedUnit = null;
  }

  setSelectedUnit(unit: any) {
    this.selectedUnit = unit;
  }
}
