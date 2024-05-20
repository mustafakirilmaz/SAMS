import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class BuildingService extends BaseService {

  constructor() {
    super();
  }

  createBuilding(buildingForm: object) {
    return this.httpHelper.post('buildings', null, buildingForm);
  }  
  
  updateBuilding(id:number, buildingForm: object) {
    return this.httpHelper.put('buildings', null, id, buildingForm);
  }

  getBuildingForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('buildings', 'search', params);
  }

  deleteBuilding(buildingId: number) {
    return this.httpHelper.delete('buildings', buildingId.toString());
  }

  aprroveBuilding(buildingId: number) {
    return this.httpHelper.post('buildings', null, buildingId);
  }

  getBuildingById(buildingId: number) {
    return this.httpHelper.get('buildings', buildingId.toString());
  }

  getCurrentBuilding() {
    return this.httpHelper.get('buildings', 'current');
  }
}
