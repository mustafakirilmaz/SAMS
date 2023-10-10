import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { BaseComponent } from '../../bases/base.component';
import { CommonHelper } from '../../helpers/common-helper';
import { AppConfigComponent } from '../app-config/app.config.component';
import { MenuLayoutComponent } from '../layout/menu-layout/menu-layout.component';

@Component({
  selector: 'app-topbar',
  templateUrl: './app.topbar.component.html'
})
export class AppTopBarComponent extends BaseComponent {
  constructor(public app: MenuLayoutComponent, public router: Router, public ch: CommonHelper) { super(); }

  goProfile() {
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
