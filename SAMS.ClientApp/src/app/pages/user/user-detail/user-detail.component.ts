import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { SelectItem } from 'primeng/api';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { TCNOKontrolValidator } from 'src/app/shared/validators/custom-validators';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css'],

})
export class UserDetailComponent extends BaseComponent implements OnInit {
  userForm: FormGroup;
  userRoleForm: FormGroup;
  roles: SelectItem[];

  constructor(private userService: UserService, public router: Router) { super(); }

  ngOnInit() {
    this.createUserForm();
    this.createUserRoleForm();
    this.getRoles();
    const userId = this.ch.getUrlParams().id || '';
    if (userId > 0) {
      this.getUserById(userId);
    }
  }

  getRoles() {
    this.cs.getRoles().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.roles = this.ch.addUnselectedItem(result.data, 0);
      }
    });
  }

  createUserForm() {
    this.userForm = this.ch.formBuilder.group({
      id: [null],
      phone: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      name: [null, Validators.required],
      surname: [null, Validators.required],
      roleIds: [null, [Validators.required, Validators.minLength(1)]],
      isActive: [true, Validators.required],
    });
  }

  createUserRoleForm() {
    this.userRoleForm = this.ch.formBuilder.group({
      roleName: [null],
      roleId: [null, Validators.required],
    });
  }

  addUserRole() {
    let addingRole = this.userRoleForm.value;
    if (this.ch.isNullOrUndefined(addingRole.roleId)) {
      this.ch.messageHelper.showWarnMessage("Rol seçiniz...");
      return;
    }
    let roles = this.ch.getControlValue(this.userForm, 'roles');
    for (let i = 0; i < roles.length; i++) {
      const role = roles[i];
      if (role.roleId == addingRole.roleId) {
        this.ch.messageHelper.showWarnMessage("Rol daha önceden eklenmiş...");
        return;
      }
    }
    if (!this.ch.isNullOrUndefined(addingRole.roleId)) {
      for (let i = 0; i < this.roles.length; i++) {
        const role = this.roles[i];
        if (role.value === addingRole.roleId) {
          addingRole.roleName = role.label;
          break;
        }
      }
    }
    roles.push(addingRole);
    this.ch.setControlValue(this.userForm, 'roles', roles);

    const roleControl = this.userForm.get("roles");
    roleControl.updateValueAndValidity();
    this.userRoleForm.reset();
  }

  deleteUserRole(deletingRole) {
    let userRoles: any[] = this.ch.getControlValue(this.userForm, 'roles');
    for (let i = 0; i < userRoles.length; i++) {
      const userRole = userRoles[i];
      if (userRole.roleId == deletingRole.roleId) {
        userRoles.splice(i, 1);
        return;
      }
    }
  }

  saveUser() {
    if (!this.ch.validateAllFormFields(this.userForm, true)) {
      return;
    }
    const userId = this.ch.getControlValue(this.userForm, 'id');
    if (this.ch.isNullOrUndefined(userId)) {
      this.userService.createUser(this.userForm.value).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.router.navigate(['/user/list-user']);
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    }
    else {
      this.userService.updateUser(this.userForm.value.id, this.userForm.value).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.router.navigate(['/user/list-user']);
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    }
  }

  getUserById(userId) {
    this.userService.getUserById(userId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.userForm);
      }
    });
  }

  get userFormControls() {
    return this.userForm.controls;
  }

  get userRoleFormControls() {
    return this.userRoleForm.controls;
  }
}
