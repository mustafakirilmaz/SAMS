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
import { SiteListComponent } from './pages/site/site-list/site-list.component';
import { SiteDetailComponent } from './pages/site/site-detail/site-detail.component';
import { BuildingDetailComponent } from './pages/building/building-detail/building-detail.component';
import { BuildingListComponent } from './pages/building/building-list/building-list.component';
import { UnitDetailComponent } from './pages/unit/unit-detail/unit-detail.component';
import { UnitListComponent } from './pages/unit/unit-list/unit-list.component';
import { BusinessProjectDetailComponent } from './pages/business-projects/business-project/business-project-detail/business-project-detail.component';
import { BusinessProjectListComponent } from './pages/business-projects/business-project/business-project-list/business-project-list.component';
import { EqualExpenseDetailComponent } from './pages/business-projects/equal-expense/equal-expense-detail/equal-expense-detail.component';
import { EqualExpenseListComponent } from './pages/business-projects/equal-expense/equal-expense-list/equal-expense-list.component';
import { ProportionalExpenseListComponent } from './pages/business-projects/proportional-expense/proportional-expense-list/proportional-expense-list.component';
import { ProportionalExpenseDetailComponent } from './pages/business-projects/proportional-expense/proportional-expense-detail/proportional-expense-detail.component';
import { FixtureExpenseDetailComponent } from './pages/business-projects/fixture-expense/fixture-expense-detail/fixture-expense-detail.component';
import { FixtureExpenseListComponent } from './pages/business-projects/fixture-expense/fixture-expense-list/fixture-expense-list.component';
import { BusinessProjectMainComponent } from './pages/business-projects/business-project-main/business-project-main.component';
import { ExpenseTypeListComponent } from './pages/business-projects/expense-type/expense-type-list/expense-type-list.component';
import { ExpenseTypeDetailComponent } from './pages/business-projects/expense-type/expense-type-detail/expense-type-detail.component';
import { ExpenseListComponent } from './pages/business-projects/expense/expense-list/expense-list.component';
import { ExpenseDetailComponent } from './pages/business-projects/expense/expense-detail/expense-detail.component';

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
                path: 'site', component: MenuLayoutComponent,
                children: [
                    { path: 'add-site', component: SiteDetailComponent, canActivate: [RoleGuard] },
                    { path: 'edit-site', component: SiteDetailComponent, canActivate: [RoleGuard] },
                    { path: 'list-site', component: SiteListComponent, canActivate: [RoleGuard] },
                ]
            },
            {
                path: 'building', component: MenuLayoutComponent,
                children: [
                    { path: 'add-building', component: BuildingDetailComponent, canActivate: [RoleGuard] },
                    { path: 'edit-building', component: BuildingDetailComponent, canActivate: [RoleGuard] },
                    { path: 'list-building', component: BuildingListComponent, canActivate: [RoleGuard] },
                ]
            },
            {
                path: 'unit', component: MenuLayoutComponent,
                children: [
                    { path: 'add-unit', component: UnitDetailComponent, canActivate: [RoleGuard] },
                    { path: 'edit-unit', component: UnitDetailComponent, canActivate: [RoleGuard] },
                    { path: 'list-unit/:buildingId', component: UnitListComponent, canActivate: [RoleGuard] },
                    { path: 'list-unit', component: UnitListComponent, canActivate: [RoleGuard] },
                ]
            },
            {
                path: 'business-project',
                component: MenuLayoutComponent,
                children: [
                    {
                        path: 'add-business-project',
                        component: BusinessProjectDetailComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'edit-business-project',
                        component: BusinessProjectDetailComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'list-business-project/:buildingId',
                        component: BusinessProjectListComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'list-business-project',
                        component: BusinessProjectListComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'add-expense-type',
                        component: ExpenseTypeDetailComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'edit-expense-type',
                        component: ExpenseTypeDetailComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'list-expense-type',
                        component: ExpenseTypeListComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'add-expense',
                        component: ExpenseDetailComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'edit-expense',
                        component: ExpenseDetailComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'list-expense/:expenseTypeId',
                        component: ExpenseListComponent,
                        canActivate: [RoleGuard]
                    },
                    {
                        path: 'list-expense',
                        component: ExpenseListComponent,
                        canActivate: [RoleGuard]
                    },
                ]
            },
            {
                path: 'equal-expense', component: MenuLayoutComponent,
                children: [
                    { path: 'add-equal-expense', component: EqualExpenseDetailComponent, canActivate: [RoleGuard] },
                    { path: 'add-equal-expense/:businessProjectId', component: EqualExpenseDetailComponent, canActivate: [RoleGuard] },
                    { path: 'edit-equal-expense', component: EqualExpenseDetailComponent, canActivate: [RoleGuard] },
                    { path: 'edit-equal-expense/:businessProjectId', component: EqualExpenseDetailComponent, canActivate: [RoleGuard] },
                    { path: 'list-equal-expense/:businessProjectId', component: EqualExpenseListComponent, canActivate: [RoleGuard] },
                    { path: 'list-equal-expense', component: EqualExpenseListComponent, canActivate: [RoleGuard] },
                ]
            },
            {
                path: 'proportional-expense', component: MenuLayoutComponent,
                children: [
                    { path: 'add-proportional-expense', component: ProportionalExpenseDetailComponent, canActivate: [RoleGuard] },
                    { path: 'edit-proportional-expense', component: ProportionalExpenseDetailComponent, canActivate: [RoleGuard] },
                    { path: 'list-proportional-expense/:businessProjectId', component: ProportionalExpenseListComponent, canActivate: [RoleGuard] },
                    { path: 'list-proportional-expense', component: ProportionalExpenseListComponent, canActivate: [RoleGuard] },
                ]
            },
            {
                path: 'fixture-expense', component: MenuLayoutComponent,
                children: [
                    { path: 'add-fixture-expense', component: FixtureExpenseDetailComponent, canActivate: [RoleGuard] },
                    { path: 'edit-fixture-expense', component: FixtureExpenseDetailComponent, canActivate: [RoleGuard] },
                    { path: 'list-fixture-expense/:businessProjectId', component: FixtureExpenseListComponent, canActivate: [RoleGuard] },
                    { path: 'list-fixture-expense', component: FixtureExpenseListComponent, canActivate: [RoleGuard] },
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
