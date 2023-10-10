import { Component, OnInit } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { MenuLayoutComponent } from '../layout/menu-layout/menu-layout.component';
import { MenuRoutes } from 'src/app/app-menu-routes';
import { CommonHelper } from '../../helpers/common-helper';
import { Roles } from '../../enums/role';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { BaseComponent } from '../../bases/base.component';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html',
    animations: [
        trigger('inline', [
            state('hidden', style({
                height: '0px',
                overflow: 'hidden'
            })),
            state('visible', style({
                height: '*',
            })),
            state('hiddenAnimated', style({
                height: '0px',
                overflow: 'hidden'
            })),
            state('visibleAnimated', style({
                height: '*',
            })),
            transition('visibleAnimated => hiddenAnimated', animate('400ms cubic-bezier(0.86, 0, 0.07, 1)')),
            transition('hiddenAnimated => visibleAnimated', animate('400ms cubic-bezier(0.86, 0, 0.07, 1)'))
        ])
    ]
})
export class AppMenuComponent extends BaseComponent implements OnInit {

    model: any[];

    inlineModel: any[];

    constructor(public app: MenuLayoutComponent, public menuRoutes: MenuRoutes, public router: Router) { super(); }

    ngOnInit() {
        this.model = this.getPermittedMenuRoutes(JSON.parse(JSON.stringify(this.menuRoutes.otherRoutes)));

        // this.inlineModel = [
        //     {
        //         label: 'Profile', icon: 'pi pi-fw pi-user'
        //     },
        //     {
        //         label: 'Settings', icon: 'pi pi-fw pi-cog'
        //     },
        //     {
        //         label: 'Messages', icon: 'pi pi-fw pi-envelope'
        //     },
        //     {
        //         label: 'Notifications', icon: 'pi pi-fw pi-bell'
        //     }
        // ];
    }

    onMenuClick(event) {
        this.app.onMenuClick(event);
    }

    getPermittedMenuRoutes(menuRoutes: any): any {
        let permittedMenuRoutes = [];
        for (let i = 0; i < menuRoutes.length; i++) {
            const menuRoute = menuRoutes[i];
            if (!this.ch.isNullOrUndefined(menuRoute.items) && menuRoute.items.length > 0) {
                menuRoute.items = this.getPermittedMenuRoutes(menuRoute.items);
            }
            if (!this.ch.isNullOrUndefined(menuRoute.roles)) {
                for (let j = 0; j < menuRoute.roles.length; j++) {
                    const role = menuRoute.roles[j];
                    if (this.ch.hasRole(role, true)) {
                        permittedMenuRoutes.push(menuRoute);
                        break;
                    }
                }
            }
            else {
                if (this.ch.isNullOrUndefined(menuRoute.items)) {
                    permittedMenuRoutes.push(menuRoute);
                }
                if (!this.ch.isNullOrUndefined(menuRoute.items) && menuRoute.items.length > 0) {
                    permittedMenuRoutes.push(menuRoute);
                }
            }
        }

        return permittedMenuRoutes;
    }

    goProfile() {
        //window.location.href = "/";
        this.router.navigate(['/user/profile']);
    }

    changeRole() {
        this.router.navigate(['/select-role']);
    }

    goSettings() {
        this.router.navigate(['/settings/edit']);
    }

    goFrontend() {
        window.location.href = this.globals.safeUrls.main;
    }
}
