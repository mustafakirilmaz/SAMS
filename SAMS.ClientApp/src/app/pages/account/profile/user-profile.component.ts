import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user-service';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { CommonHelper } from 'src/app/shared/helpers/common-helper';
import { UserInfo } from 'src/app/shared/models/user-info';
import { AccountService } from 'src/app/services/account-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { ValidatorRegex } from 'src/app/shared/validators/regex.validator';
import { RegexType } from 'src/app/shared/enums/regex-type';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent extends BaseComponent implements OnInit {
  userForm: FormGroup;
  changePasswordForm: FormGroup;
  userInfo: UserInfo;
  avatarPath = '../assets/images/user-avatar1.png';

  constructor(public ch: CommonHelper, public accountService: AccountService,private userService: UserService, public router: Router) { super(); }

  ngOnInit() {
    this.userInfo = this.currentUser;
    this.createUserForm();
    this.createChangePasswordForm();
    this.getCurrentUser();
  }

  createUserForm() {
    this.userForm = this.ch.formBuilder.group({
      id: [null],
      userId: [''],
      name: ['', Validators.required],
      surname: ['', Validators.required],
      phone: ['', Validators.required],
      email: [{value: '', disabled: true}, Validators.required],
      imageUrl: [''],
      rowGuid: [this.ch.createGuid()]
    });
  }

  createChangePasswordForm() {
    this.changePasswordForm = this.ch.formBuilder.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, ValidatorRegex(RegexType.SysPvd)]],
      newPasswordConfirm: ['', [Validators.required, ValidatorRegex(RegexType.SysPvd)]],
      userId: [null]
    });
  }

  getCurrentUser() {
    this.userService.getCurrentUser().subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.userForm);
      }
    });
  }

  updateUserInfo() {
    if (!this.ch.validateAllFormFields(this.userForm, true)) {
      return;
    }

    this.accountService.updateProfile(this.userForm.value.id, this.userForm.value).subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.ch.messageHelper.showSuccessMessage(result.messages[0]);
      }
    });
  }

  uploadCompleted(files) {
    this.ch.setControlValue(this.userForm, 'imageUrl', files[0].url);
    if (!this.ch.validateAllFormFields(this.userForm, false)) {
      this.ch.messageHelper.showSuccessMessage("Profil resminin aktif olması için lütfen Kaydet'e basınız.");
    }

    this.accountService.updateProfile(this.userForm.value.id, this.userForm.value).subscribe(result => {
      this.ch.currentUser.imageUrl = files[0].url + "?q=" + this.ch.createGuid();
    });
  }

  removedCompleted(files) {
    this.ch.setControlValue(this.userForm, 'imageUrl', null);
    this.ch.currentUser.imageUrl = null;
    
    this.accountService.updateProfile(this.userForm.value.id, this.userForm.value).subscribe(result => {
      this.ch.currentUser.imageUrl = null;
    });
  }

  get userFormControls() {
    return this.userForm.controls;
  }

  changePassword() {
    if (!this.ch.validateAllFormFields(this.changePasswordForm, true)) {
      return;
    }

    if (this.changePasswordForm.value.newPassword !== this.changePasswordForm.value.newPasswordConfirm) {
      this.ch.messageHelper.showErrorMessage('Girdiğiniz şifreler eşleşmiyor. Lütfen tekrar deneyiniz');
      return;
    }

    this.changePasswordForm.value.userId = this.currentUser.id;
    this.accountService.changePasswordProfile(this.changePasswordForm.value).subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.ch.messageHelper.showSuccessMessage('Şifreniz başarıyla değiştirildi');
        this.changePasswordForm.reset();
      }
    });
  }

  get changePasswordFormControls() {
    return this.changePasswordForm.controls;
  }
}
