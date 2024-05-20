import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class EqualExpenseService extends BaseService {

  constructor() {
    super();
  }

  createEqualExpense(equalExpenseForm: object) {
    return this.httpHelper.post('equalExpenses', null, equalExpenseForm);
  }  
  
  updateEqualExpense(id:number, equalExpenseForm: object) {
    return this.httpHelper.put('equalExpenses', null, id, equalExpenseForm);
  }

  getEqualExpenseForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('equalExpenses', 'search', params);
  }

  deleteEqualExpense(equalExpenseId: number) {
    return this.httpHelper.delete('equalExpenses', equalExpenseId.toString());
  }

  aprroveEqualExpense(equalExpenseId: number) {
    return this.httpHelper.post('equalExpenses', null, equalExpenseId);
  }

  getEqualExpenseById(equalExpenseId: number) {
    return this.httpHelper.get('equalExpenses', equalExpenseId.toString());
  }

  getCurrentEqualExpense() {
    return this.httpHelper.get('equalExpenses', 'current');
  }

  // getEqualExpensesByBusinessProjectId(businessProjectId: number) {
  //   return this.httpHelper.get<object[]>('equalExpenses', businessProjectId.toString());
  // }
}