import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account-service';
import { BaseComponent } from 'src/app/shared/bases/base.component';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
})
export class ForgetPasswordComponent extends BaseComponent implements OnInit {
  forgetPasswordForm: FormGroup;
  constructor(public route: ActivatedRoute, private accountService: AccountService, public router: Router) { super(); }

  ngOnInit() {
    this.createforgetPasswordForm();
  }

  createforgetPasswordForm() {
    this.forgetPasswordForm = this.ch.formBuilder.group({
      email: ['', Validators.required]
    });
  }

  forgetPassword() {
    if (!this.ch.validateAllFormFields(this.forgetPasswordForm, true)) {
      return;
    }
    this.accountService.forgetPassword(this.forgetPasswordForm.value).subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        this.logout();
      }
    });
  }

  get forgetPasswordFormControls() {
    return this.forgetPasswordForm.controls;
  }
}
