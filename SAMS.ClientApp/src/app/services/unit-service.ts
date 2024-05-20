import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class UnitService extends BaseService {

  constructor() {
    super();
  }

  createUnit(unitForm: object) {
    return this.httpHelper.post('units', null, unitForm);
  }  
  
  updateUnit(id:number, unitForm: object) {
    return this.httpHelper.put('units', null, id, unitForm);
  }

  getUnitForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('units', 'search', params);
  }

  deleteUnit(unitId: number) {
    return this.httpHelper.delete('units', unitId.toString());
  }

  aprroveUnit(unitId: number) {
    return this.httpHelper.post('units', null, unitId);
  }

  getUnitById(unitId: number) {
    return this.httpHelper.get('units', unitId.toString());
  }

  getCurrentUnit() {
    return this.httpHelper.get('units', 'current');
  }
}
