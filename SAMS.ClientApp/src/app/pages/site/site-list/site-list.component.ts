import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SiteService } from 'src/app/services/site-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { Router } from '@angular/router';



@Component({
  selector: 'app-site-list',
  templateUrl: './site-list.component.html',
  styleUrls: ['./site-list.component.css']
})
export class SiteListComponent extends BaseComponent implements OnInit {
  createSiteModalVisible: boolean = false;
  gridName = 'siteGrid';
  selectedSite: any;
  formGridColumns = [
    ['name', 'Site Adı'],
    ['operations', 'İşlem', ColumnType.Operation]
  ];

  constructor(public siteService: SiteService, private router: Router) { super(); }

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
    this.siteService.getSiteForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteSite(siteId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.siteService.deleteSite(siteId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  editSite(site) {
    this.selectedSite = site;
    this.openSiteModal();
  }

  openSiteModal() {
    this.createSiteModalVisible = true;
  }

  onSiteSaved(event){
    this.createSiteModalVisible = false;
    this.gridRefresh();
  }

  onHideSiteModal(){
    this.selectedSite = null;
  }

  setSelectedSite(site: any) {
    this.selectedSite = site;
  }
}
