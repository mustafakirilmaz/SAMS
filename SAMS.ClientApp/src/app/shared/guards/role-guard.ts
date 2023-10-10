import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account-service';
import { CommonHelper } from '../helpers/common-helper';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(public router: Router, public ch: CommonHelper, public as: AccountService) { }

  canActivate(): boolean {
    if (!this.ch.isLoggedIn()) {
      this.ch.clearLocalStorageItems();
      this.ch.clearSessionStorageItems();
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
