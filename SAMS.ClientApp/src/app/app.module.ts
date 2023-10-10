import { Injector, LOCALE_ID, NgModule } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ShareddModule } from './shared/shared.module';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { LoginComponent } from './pages/account/login/login.component';
import { HomeComponent } from './pages/home/home.component'

import localeTr from '@angular/common/locales/tr';

import { CommonModule, DatePipe, registerLocaleData } from '@angular/common';
import { AppInjector } from './shared/bases/app-injector.service';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoaderInterceptor } from './shared/interceptor/loader-interceptor';
import { ConfirmationService } from 'primeng/api';
import { AuthGuard } from './shared/guards/auth.guard';
import { CommonService } from './services/common.service';
import { HttpHelper } from './services/http-helper.service';
import { RoleGuard } from './shared/guards/role-guard';
import { JwtModule } from '@auth0/angular-jwt';
import { FullCalendarModule } from '@fullcalendar/angular';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppAccessdeniedComponent } from './shared/components/app-access-denied/app.accessdenied.component';
import { AppNotfoundComponent } from './shared/components/app-not-found/app.notfound.component';

import { UserDetailComponent } from './pages/user/user-detail/user-detail.component';
import { UserListComponent } from './pages/user/user-list/user-list.component';
import { UserProfileComponent } from './pages/account/profile/user-profile.component';
import { ChangePasswordComponent } from './pages/account/change-password/change-password.component';
import { ForgetPasswordComponent } from './pages/account/forget-password/forget-password.component';
import { SelectRoleComponent } from './pages/account/select-role/select-role.component';
import { AppConfigComponent } from './shared/components/app-config/app.config.component';



registerLocaleData(localeTr, 'tr');
export function tokenGetter() {
    return localStorage.getItem(this.globals.localStorageItems.authToken);
}

export function HttpLoaderFactory(httpClient: HttpClient) {
    return new TranslateHttpLoader(httpClient);
}

@NgModule({
    imports: [
        FormsModule,
        BrowserModule,
        BrowserAnimationsModule,
        ReactiveFormsModule,
        CommonModule,
        ShareddModule,
        AppRoutingModule,
        NgxUiLoaderModule,
        FullCalendarModule,
        TranslateModule.forRoot({
            loader: {
              provide: TranslateLoader,
              useFactory: HttpLoaderFactory,
              deps: [HttpClient]
            }
          }),
        JwtModule.forRoot({ config: { tokenGetter: tokenGetter, } }),
    ],
    declarations: [
        AppComponent, AppAccessdeniedComponent, AppNotfoundComponent,
        HomeComponent,
        LoginComponent, ChangePasswordComponent, ForgetPasswordComponent, SelectRoleComponent,
        UserDetailComponent, UserListComponent, UserProfileComponent,
    ],
    exports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        TranslateModule
    ],
    providers: [
        { provide: LOCALE_ID, useValue: 'tr-TR' },
        { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
        DatePipe,
        ConfirmationService,
        AuthGuard,
        RoleGuard,
        FormBuilder,
        CommonService,
        HttpHelper,
        AppConfigComponent,
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
    constructor(injector: Injector) {
        AppInjector.setInjector(injector);
    }
}
