import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account-service';
import { BaseComponent } from 'src/app/shared/bases/base.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent extends BaseComponent implements OnInit {
  loginForm: FormGroup;
  passwordChange = false;
  forgetPasswordDisplay = false;
  changePasswordForm: FormGroup;
  forgetPasswordForm: FormGroup;

  constructor(public route: ActivatedRoute, private accountService: AccountService, public router: Router) {
    super();
  }

  ngOnInit() {
    this.ch.clearLocalStorageItems();
    this.ch.clearSessionStorageItems();
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = this.ch.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  createForgetPasswordForm() {
    this.forgetPasswordForm = this.ch.formBuilder.group({
      email: ['', Validators.required],
      guid: ['']
    });
  }

  login() {
    if (!this.ch.validateAllFormFields(this.loginForm, true)) {
      return;
    }
    this.accountService.userLogin(this.loginForm.value).subscribe(result => {
      if(!this.ch.isNullOrUndefined(result.data) && (<any>result.data).roles.length == 0){
        this.ch.messageHelper.showInfoMessage("Sistemde kayıtlı rol bulunamadı. Lütfen yöneticinizle iletişime geçiniz...");
        this.logout();
        return;
      }
      if (this.ch.checkResult(result)) {
        this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        this.ch.clearLocalStorageItems();
        this.ch.clearSessionStorageItems();

        localStorage.setItem(this.globals.localStorageItems.authToken, result.data['token']);
        this.ch.setCurrentUser();

        const currentUser = this.ch.currentUser;
        if (!this.ch.isNullOrUndefined(currentUser)) {
          if (currentUser.selectedRole === null || currentUser.selectedRole === '') {
            this.router.navigate(['/select-role']);
            return;
          }
        }
        this.router.navigate(['/']);
      }
    });
  }

  get loginFormControls() {
    return this.loginForm.controls;
  }

  forgetPassword() {
    this.router.navigate(['/forget-password']);
  }

  goFrontend() {
    window.location.href = "/";
  }
}
