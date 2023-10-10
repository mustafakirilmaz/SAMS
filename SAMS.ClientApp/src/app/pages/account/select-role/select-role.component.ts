import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account-service';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { RoleDescription } from 'src/app/shared/enums/role';

@Component({
  selector: 'app-select-role',
  templateUrl: './select-role.component.html',
})
export class SelectRoleComponent extends BaseComponent implements OnInit {
  roles: any = [];
  constructor(public route: ActivatedRoute, private accountService: AccountService, public router: Router) { super(); }

  ngOnInit() {
    if (this.ch.isNullOrUndefined(this.currentUser) || this.ch.isNullOrUndefined(this.currentUser.roles)) {
      this.logout();
      return;
    }
    if(this.currentUser.roles.length == 0){
      this.ch.messageHelper.showInfoMessage("Sistemde kayıtlı rol bulunamadı. Lütfen yöneticinizle iletişime geçiniz...");
      this.logout();
      return;
    }
    for (let i = 0; i < this.currentUser.roles.length; i++) {
      const role = JSON.parse(this.currentUser.roles[i]);
      let description = RoleDescription[role.RoleName];
      if (!this.ch.isNullOrWhiteSpace(role.HospitalName)) {
        description += ' - ' + role.HospitalName;
      }
      if (!this.ch.isNullOrWhiteSpace(role.DoctorName)) {
        description += ' - ' + role.DoctorName;
      }
      this.roles.push({ name: role.RoleName, description: description, role: role });
    }
  }

  setSelectedRole(role) {
    this.accountService.setSelectedRole(role).subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.ch.clearLocalStorageItems();
        this.ch.clearSessionStorageItems();
        localStorage.setItem(this.globals.localStorageItems.authToken, result.data['token']);
        this.ch.setCurrentUser();
        this.router.navigate(['/']);
      } else {
        this.ch.messageHelper.showErrorMessage(result.messages[0]);
      }
    });
  }
}
