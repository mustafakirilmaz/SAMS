import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { MenuLayoutComponent } from './shared/components/layout/menu-layout/menu-layout.component';
import { EmptyLayoutComponent } from './shared/components/layout/empty-layout/empty-layout.component';

import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/account/login/login.component';
import { RoleGuard } from './shared/guards/role-guard';
import { UserProfileComponent } from './pages/account/profile/user-profile.component';
import { UserDetailComponent } from './pages/user/user-detail/user-detail.component';
import { UserListComponent } from './pages/user/user-list/user-list.component';
import { AppNotfoundComponent } from './shared/components/app-not-found/app.notfound.component';
import { ChangePasswordComponent } from './pages/account/change-password/change-password.component';
import { ForgetPasswordComponent } from './pages/account/forget-password/forget-password.component';
import { AppAccessdeniedComponent } from './shared/components/app-access-denied/app.accessdenied.component';
import { SelectRoleComponent } from './pages/account/select-role/select-role.component';



@NgModule({
    imports: [
        RouterModule.forRoot([
            {
                path: 'user', component: MenuLayoutComponent,
                children: [
                    { path: 'add-user', component: UserDetailComponent, canActivate: [RoleGuard] },
                    { path: 'edit-user', component: UserDetailComponent, canActivate: [RoleGuard] },
                    { path: 'list-user', component: UserListComponent, canActivate: [RoleGuard] },
                    { path: 'profile', component: UserProfileComponent, canActivate: [RoleGuard] },
                ]
            },
            {
                path: '', component: MenuLayoutComponent,
                children: [
                    { path: '', component: HomeComponent, canActivate: [RoleGuard] },
                    { path: 'home', component: HomeComponent, canActivate: [RoleGuard] },
                ]
            },
            {
                path: '', component: EmptyLayoutComponent,
                children: [
                    //{ path: '', component: LoginComponent },
                    // { path: 'uikit/formlayout', component: FormLayoutDemoComponent },
                ]
            },
            { path: 'login', component: LoginComponent },
            { path: 'change-password', component: ChangePasswordComponent },
            { path: 'forget-password', component: ForgetPasswordComponent },
            // { path: 'register', component: RegisterComponent },
            { path: 'accessdenied', component: AppAccessdeniedComponent },
            { path: 'select-role', component: SelectRoleComponent },
            { path: '**', component: AppNotfoundComponent },

        ], { scrollPositionRestoration: 'enabled' })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
