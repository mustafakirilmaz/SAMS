import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class SiteService extends BaseService {

  constructor() {
    super();
  }

  createSite(siteForm: object) {
    return this.httpHelper.post('sites', null, siteForm);
  }  
  
  updateSite(id:number, siteForm: object) {
    return this.httpHelper.put('sites', null, id, siteForm);
  }

  getSiteForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('sites', 'search', params);
  }

  deleteSite(siteId: number) {
    return this.httpHelper.delete('sites', siteId.toString());
  }

  aprroveSite(siteId: number) {
    return this.httpHelper.post('sites', null, siteId);
  }

  getSiteById(siteId: number) {
    return this.httpHelper.get('sites', siteId.toString());
  }

  getCurrentSite() {
    return this.httpHelper.get('sites', 'current');
  }
}
