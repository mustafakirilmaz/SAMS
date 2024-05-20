import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class BusinessProjectService extends BaseService {

  constructor() {
    super();
  }

  createBusinessProject(businessProjectForm: object) {
    return this.httpHelper.post('businessProjects', null, businessProjectForm);
  }  
  
  updateBusinessProject(id:number, businessProjectForm: object) {
    return this.httpHelper.put('businessProjects', null, id, businessProjectForm);
  }

  getBusinessProjectForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('businessProjects', 'search', params);
  }

  deleteBusinessProject(businessProjectId: number) {
    return this.httpHelper.delete('businessProjects', businessProjectId.toString());
  }

  aprroveBusinessProject(businessProjectId: number) {
    return this.httpHelper.post('businessProjects', null, businessProjectId);
  }

  getBusinessProjectById(businessProjectId: number) {
    return this.httpHelper.get('businessProjects', businessProjectId.toString());
  }

  getCurrentBusinessProject() {
    return this.httpHelper.get('businessProjects', 'current');
  }
}
