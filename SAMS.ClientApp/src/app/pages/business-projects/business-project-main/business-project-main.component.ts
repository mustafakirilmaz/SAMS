import { Component, Input, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from 'src/app/shared/bases/base.component';

@Component({
  selector: 'app-business-project-main',
  templateUrl: './business-project-main.component.html',
  styleUrls: ['./business-project-main.component.css'],
  encapsulation: ViewEncapsulation.None,
})

export class BusinessProjectMainComponent extends BaseComponent {
  index: number = 0;
  tabCount: number = 6;

  constructor(public router: Router) {
    super();
  }

  openNext() {
    this.index = (this.index === this.tabCount - 1) ? 0 : this.index + 1;
  }

  openPrev() {
    this.index = (this.index === 0) ? this.tabCount - 1 : this.index - 1;
  }
}