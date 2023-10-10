import { Injectable } from '@angular/core';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class UserService extends BaseService {

  constructor() {
    super();
  }

  createUser(userForm: object) {
    return this.httpHelper.post('users', null, userForm);
  }  
  
  updateUser(id:number, userForm: object) {
    return this.httpHelper.put('users', null, id, userForm);
  }

  getUserForGrid(gridName: string) {
    const params = this.ch.createParams(this.ch.getGridFilter(gridName), this.ch.getSearchFilter(gridName));
    return this.httpHelper.get<object[]>('users', 'search', params);
  }

  deleteUser(userId: number) {
    return this.httpHelper.delete('users', userId.toString());
  }

  aprroveUser(userId: number) {
    return this.httpHelper.post('users', null, userId);
  }

  getUserById(userId: number) {
    return this.httpHelper.get('users', userId.toString());
  }

  getCurrentUser() {
    return this.httpHelper.get('users', 'current');
  }
}
