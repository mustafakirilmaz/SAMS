import { Component, OnInit } from '@angular/core';
import { CommonHelper } from '../../../app/shared/helpers/common-helper';
import { AppInjector } from './app-injector.service';
import { UserInfo } from '../models/user-info';
import { GlobalVariables } from '../constants/global-variables';
import { CommonService } from '../../services/common.service';
import { Router } from '@angular/router';
import { MenuService } from '../components/app-menu/app.menu.service';
import { AccountService } from 'src/app/services/account-service';

@Component({
    template: '',
})

export class BaseComponent implements OnInit {
    public ch: CommonHelper;
    protected cs: CommonService;
    public globals: GlobalVariables;
    private as: AccountService;
    currentUser: UserInfo;

    constructor() {
        try {
            const injector = AppInjector.getInjector();
            this.ch = injector.get(CommonHelper);
            this.cs = injector.get(CommonService);
            this.globals = injector.get(GlobalVariables);
            this.as = injector.get(AccountService);
            injector.get(MenuService).reset();
            this.currentUser = this.ch.currentUser;
            if (!this.ch.isNullOrUndefined(this.currentUser) &&
                !this.ch.isNullOrUndefined(this.currentUser.selectedRole) &&
                (this.currentUser.selectedRole === null || this.currentUser.selectedRole === '')) {
                injector.get(Router).navigate(['/select-role']);
            }
        } catch (e) {
            console.log('Bağımlılıklar yüklenirken hata oluştu', e)
        }
    }

    ngOnInit() { }

    goUserAccount(email) {
        this.as.loginAnotherUserAccount(email).subscribe(result => {
            if (this.ch.checkResult(result)) {
                const injector = AppInjector.getInjector();
                this.ch.messageHelper.showSuccessMessage(result.messages[0]);
                this.ch.clearLocalStorageItems();
                this.ch.clearSessionStorageItems();
                localStorage.setItem(this.globals.localStorageItems.authToken, result.data['token']);
                this.ch.setCurrentUser();

                const currentUser = this.ch.currentUser;
                if (!this.ch.isNullOrUndefined(currentUser)) {
                    if (currentUser.selectedRole === null || currentUser.selectedRole === '') {
                        injector.get(Router).navigate(['/select-role']);
                        return;
                    }
                }
                injector.get(Router).navigate(['/']);
            }
        });
    }

    logout() {
        const injector = AppInjector.getInjector();
        this.ch.clearLocalStorageItems();
        this.ch.clearSessionStorageItems();
        injector.get(Router).navigate(['/login']);
    }
}
