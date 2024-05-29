import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class BusinessProjectExpenseService extends BaseService {

  constructor() {
    super();
  }

  createBusinessProjectExpense(businessProjectExpenseForm: object) {
    return this.httpHelper.post('businessProjectExpenses', null, businessProjectExpenseForm);
  }

  updateBusinessProjectExpense(id: number, businessProjectExpenseForm: object) {
    return this.httpHelper.put('businessProjectExpenses', null, id, businessProjectExpenseForm);
  }

  getBusinessProjectExpenseForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('businessProjectExpenses', 'search', params);
  }

  deleteBusinessProjectExpense(businessProjectExpenseId: number) {
    return this.httpHelper.delete('businessProjectExpenses', businessProjectExpenseId.toString());
  }

  aprroveBusinessProjectExpense(businessProjectExpenseId: number) {
    return this.httpHelper.post('businessProjectExpenses', null, businessProjectExpenseId);
  }

  getBusinessProjectExpenseById(businessProjectExpenseId: number) {
    return this.httpHelper.get('businessProjectExpenses', businessProjectExpenseId.toString());
  }

  getCurrentBusinessProjectExpense() {
    return this.httpHelper.get('businessProjectExpenses', 'current');
  }
}
