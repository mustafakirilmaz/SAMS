import { Component } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { environment } from 'src/environments/environment';

@Component({
  templateUrl: './home.component.html'
})
export class HomeComponent extends BaseComponent {
  showEnvironmentMessage = !environment.production;
  environmentText = environment.environment;
  constructor() {
    super();
    if (this.showEnvironmentMessage && !this.ch.globals.isShowedEnvironmentMessage) {
      this.ch.messageHelper.showInfoMessage("Çalışma Ortamı: " + this.environmentText);
      this.ch.globals.isShowedEnvironmentMessage = true;
    }
  }
}
