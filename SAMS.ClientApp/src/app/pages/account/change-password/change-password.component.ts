import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account-service';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { RegexType } from 'src/app/shared/enums/regex-type';
import { ValidatorRegex } from 'src/app/shared/validators/regex.validator';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
})
export class ChangePasswordComponent extends BaseComponent implements OnInit {
  changePasswordForm: FormGroup;
  guid: string = null;
  constructor(public route: ActivatedRoute, private accountService: AccountService, public router: Router) { super(); }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.guid = params['guid'];
    });
    this.createChangePasswordForm();
    if (!this.ch.isNullOrUndefined(this.guid) && !this.ch.isNullOrWhiteSpace(this.guid)) {
      this.accountService.checkGuid(this.guid).subscribe(result => {
        if (!this.ch.checkResult(result)) {
          this.logout();
        }
      });
    }
  }

  createChangePasswordForm() {
    this.changePasswordForm = this.ch.formBuilder.group({
      newPassword: [null, [Validators.required, ValidatorRegex(RegexType.SysPvd)]],
      newPasswordConfirm: [null, [Validators.required, ValidatorRegex(RegexType.SysPvd)]],
      userId: [null],
      guid: [this.guid],
    });
  }

  changePassword() {
    if (!this.ch.validateAllFormFields(this.changePasswordForm, true)) {
      return;
    }

    if (this.changePasswordForm.value.newPassword !== this.changePasswordForm.value.newPasswordConfirm) {
      this.ch.messageHelper.showErrorMessage('Girdiğiniz şifreler eşleşmiyor. Lütfen tekrar deneyin.');
      return;
    }

    this.accountService.changePassword(this.changePasswordForm.value).subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        this.logout();
      } else {
        this.ch.messageHelper.showErrorMessage(result.messages[0]);
      }
    });
  }

  get changePasswordFormControls() {
    return this.changePasswordForm.controls;
  }
}
