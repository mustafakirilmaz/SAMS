import {Component} from '@angular/core';
import {MenuLayoutComponent} from '../layout/menu-layout/menu-layout.component';

@Component({
    selector: 'app-right-menu',
    templateUrl: './app.right-menu.component.html'
})
export class AppRightMenuComponent {
    statusActive = true;

    messagesActive: boolean;

    constructor(public app: MenuLayoutComponent) {
    }

    messagesClick() {
        this.statusActive = false;
        this.messagesActive = true;
    }

    statusClick() {
        this.statusActive = true;
        this.messagesActive = false;
    }
}
