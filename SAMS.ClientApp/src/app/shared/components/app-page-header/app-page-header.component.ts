import { Component, Input, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { CommonHelper } from '../../helpers/common-helper';

@Component({
  selector: 'app-page-header',
  templateUrl: './app-page-header.component.html',
  styleUrls: ['./app-page-header.component.css'],
})

export class PageHeaderComponent implements OnInit {
  @Input() caption: string;
  items: MenuItem[];
  home: MenuItem;
  @Input() parents: MenuItem[];

  constructor(private ch: CommonHelper) { }

  ngOnInit() {
    if (!this.ch.isNullOrUndefined(this.parents)) {
      this.items = JSON.parse(JSON.stringify(this.parents));
      this.items.push({ label: this.caption });
    }
    else {
      this.items = [{ label: this.caption }];
    }

    this.home = { icon: 'pi pi-home', label: ' Anasayfa', routerLink: '/' };
  }
}
