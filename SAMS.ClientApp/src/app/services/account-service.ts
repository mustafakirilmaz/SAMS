import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BaseService } from '../shared/bases/base.service';

@Injectable({
  providedIn: 'root'
})

export class AccountService extends BaseService{
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor() {
    super();
  }

  userLogin(loginForm: object) {
    return this.httpHelper.post('accounts', 'login', loginForm);
  }

  changePassword(changePasswordForm: object) {
    return this.httpHelper.post('accounts', 'change-password', changePasswordForm);
  }

  changePasswordProfile(changePasswordForm: object) {
    return this.httpHelper.post('accounts', 'change-password-profile', changePasswordForm);
  }

  forgetPassword(forgetPasswordForm: object) {
    return this.httpHelper.post('accounts', 'forget-password', forgetPasswordForm);
  }

  checkGuid(guid: string) {
    return this.httpHelper.get('accounts', 'check-guid/' + guid);
  }

  logout() {
    return this.httpHelper.get('accounts', 'logout');
  }

  setSelectedRole(role: any) {
    return this.httpHelper.post('accounts', 'set-selected-role', role);
  }

  loginAnotherUserAccount(email) {
    const params = this.ch.createParams({ email: email });
    return this.httpHelper.get('accounts', 'login-another-user-account', params);
  }

  updateProfile(id:number, userForm: object) {
    return this.httpHelper.put('accounts', 'profile', id, userForm);
  }
}
