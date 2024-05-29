import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class ExpenseService extends BaseService {

  constructor() {
    super();
  }

  createExpense(expenseForm: object) {
    return this.httpHelper.post('expenses', null, expenseForm);
  }

  updateExpense(id: number, expenseForm: object) {
    return this.httpHelper.put('expenses', null, id, expenseForm);
  }

  getExpenseForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('expenses', 'search', params);
  }

  deleteExpense(expenseId: number) {
    return this.httpHelper.delete('expenses', expenseId.toString());
  }

  aprroveExpense(expenseId: number) {
    return this.httpHelper.post('expenses', null, expenseId);
  }

  getExpenseById(expenseId: number) {
    return this.httpHelper.get('expenses', expenseId.toString());
  }

  getCurrentExpense() {
    return this.httpHelper.get('expenses', 'current');
  }
}
