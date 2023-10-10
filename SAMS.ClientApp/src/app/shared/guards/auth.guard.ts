import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { CommonHelper } from '../helpers/common-helper';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(public ch: CommonHelper) { }

  canActivate(): boolean {
    if (!this.ch.isLoggedIn()) {
      window.location.href = (this.ch.getClientBaseUrl());
      this.ch.messageHelper.showErrorMessage('Oturumunuz sona ermiştir. Lütfen tekrar giriş yapınız.');
      return false;
    }

    return true;
  }
}
