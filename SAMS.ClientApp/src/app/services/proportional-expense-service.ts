import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class ProportionalExpenseService extends BaseService {

  constructor() {
    super();
  }

  createProportionalExpense(proportionalExpenseForm: object) {
    return this.httpHelper.post('proportionalExpenses', null, proportionalExpenseForm);
  }  
  
  updateProportionalExpense(id:number, proportionalExpenseForm: object) {
    return this.httpHelper.put('proportionalExpenses', null, id, proportionalExpenseForm);
  }

  getProportionalExpenseForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('proportionalExpenses', 'search', params);
  }

  deleteProportionalExpense(proportionalExpenseId: number) {
    return this.httpHelper.delete('proportionalExpenses', proportionalExpenseId.toString());
  }

  aprroveProportionalExpense(proportionalExpenseId: number) {
    return this.httpHelper.post('proportionalExpenses', null, proportionalExpenseId);
  }

  getProportionalExpenseById(proportionalExpenseId: number) {
    return this.httpHelper.get('proportionalExpenses', proportionalExpenseId.toString());
  }

  getCurrentProportionalExpense() {
    return this.httpHelper.get('proportionalExpenses', 'current');
  }
}
