import { HostListener, OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { NgxUiLoaderConfig } from 'ngx-ui-loader';
import { BaseComponent } from './shared/bases/base.component';
import { CommonHelper } from './shared/helpers/common-helper';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNGConfig } from 'primeng/api';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent extends BaseComponent implements OnInit {
    loaderConfig: NgxUiLoaderConfig = {
        "fgsColor": "#ffffff",
        "overlayColor": "rgba(40, 40, 40, 0.8)",
        "fgsSize": 60,
        "fgsType": "ball-spin-clockwise",
        "hasProgressBar": true,
        "pbThickness": 3,
        "fastFadeOut": true
    };

    constructor(public ch: CommonHelper, private config: PrimeNGConfig, private translateService: TranslateService) { super(); }

    ngOnInit() {
        this.translateService.setDefaultLang('tr');
        this.translate('tr');
        const splashScreen: HTMLElement = document.getElementById('splashScreenClass');
        if (splashScreen) {
            splashScreen.remove();
        }
    }

    translate(lang: string) {
        this.translateService.use(lang);
        this.translateService.get('primeng').subscribe(res => this.config.setTranslation(res));
    }
}
