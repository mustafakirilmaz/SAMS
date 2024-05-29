import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class ExpenseTypeService extends BaseService {

  constructor() {
    super();
  }

  createExpenseType(expenseTypeForm: object) {
    return this.httpHelper.post('expenseTypes', null, expenseTypeForm);
  }

  updateExpenseType(id: number, expenseTypeForm: object) {
    return this.httpHelper.put('expenseTypes', null, id, expenseTypeForm);
  }

  getExpenseTypeForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('expenseTypes', 'search', params);
  }

  deleteExpenseType(expenseTypeId: number) {
    return this.httpHelper.delete('expenseTypes', expenseTypeId.toString());
  }

  aprroveExpenseType(expenseTypeId: number) {
    return this.httpHelper.post('expenseTypes', null, expenseTypeId);
  }

  getExpenseTypeById(expenseTypeId: number) {
    return this.httpHelper.get('expenseTypes', expenseTypeId.toString());
  }

  getCurrentExpenseType() {
    return this.httpHelper.get('expenseTypes', 'current');
  }
}
