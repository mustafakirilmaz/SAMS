import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem, SelectItem } from 'primeng/api';
import { BusinessProjectService } from 'src/app/services/business-project-service';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import ServiceResult from 'src/app/shared/models/service-result';


@Component({
  selector: 'app-business-project-stepper',
  templateUrl: './business-project-stepper.component.html',
  styleUrls: ['./business-project-stepper.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class BusinessProjectStepperComponent extends BaseComponent implements OnInit {
  items: MenuItem[];
  businessProjectInfo: any = null;

  constructor(public router: Router, public businessProjectService: BusinessProjectService) { super(); }

  ngOnInit() {
    let items: MenuItem[] = [
      { label: 'Detay', routerLink: '/business-project/detail' },
      { label: 'Eşit Giderler', routerLink: '/business-project/equal-expenses' },
      { label: 'Oransal Giderler', routerLink: '/business-project/proportional-expenses' },
      { label: 'Demirbaş Giderleri', routerLink: '/business-project/fixture-expenses' },
      { label: 'Özet', routerLink: '/business-project/summary' }
    ];

    const businessProjectId = this.ch.getUrlParams().businessProjectId || '';
    if (businessProjectId > 0) {
      const queryParams = this.ch.urlEncrypt({ businessProjectId: businessProjectId });
      for (let i = 0; i < items.length; i++) {
        const item = items[i];
        item.queryParams = queryParams;
      }
      this.businessProjectService.getBusinessProjectById(businessProjectId).subscribe((result: ServiceResult<object>) => {
        if (this.ch.checkResult(result)) {
          this.businessProjectInfo = result.data;
        }
      });
    }
    this.items = items;
  }
  getBusinessProjectInfo() {
    return this.businessProjectInfo.name
  }
}