import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class FixtureExpenseService extends BaseService {

  constructor() {
    super();
  }

  createFixtureExpense(fixtureExpenseForm: object) {
    return this.httpHelper.post('fixtureExpenses', null, fixtureExpenseForm);
  }  
  
  updateFixtureExpense(id:number, fixtureExpenseForm: object) {
    return this.httpHelper.put('fixtureExpenses', null, id, fixtureExpenseForm);
  }

  getFixtureExpenseForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('fixtureExpenses', 'search', params);
  }

  deleteFixtureExpense(fixtureExpenseId: number) {
    return this.httpHelper.delete('fixtureExpenses', fixtureExpenseId.toString());
  }

  aprroveFixtureExpense(fixtureExpenseId: number) {
    return this.httpHelper.post('fixtureExpenses', null, fixtureExpenseId);
  }

  getFixtureExpenseById(fixtureExpenseId: number) {
    return this.httpHelper.get('fixtureExpenses', fixtureExpenseId.toString());
  }

  getCurrentFixtureExpense() {
    return this.httpHelper.get('fixtureExpenses', 'current');
  }
}
