import { AfterViewInit, Component, OnInit } from '@angular/core';
import { CommonHelper } from '../../helpers/common-helper';
import { MenuLayoutComponent } from '../layout/menu-layout/menu-layout.component';

@Component({
    selector: 'app-config',
    templateUrl: './app.config.component.html',
})
export class AppConfigComponent implements OnInit, AfterViewInit {
    themes: any[];
    themeColor = 'blue';
    topbarColors: any[];
    savedSettings: any = {};

    constructor(public app: MenuLayoutComponent, public ch: CommonHelper) { }

    ngAfterViewInit() {
        setTimeout(() => {
            this.getSettingInLocalStorage();
        }, 0);
    }

    ngOnInit() {
        this.topbarColors = [
            { label: 'layout-topbar-light', logo: 'logo', color: '#ffffff' },
            { label: 'layout-topbar-dark', logo: 'logo-white', color: '#252529' },
            { label: 'layout-topbar-blue', logo: 'logo-white', color: '#dc0d15' },
            { label: 'layout-topbar-green', logo: 'logo-white', color: '#0F8C50' },
            { label: 'layout-topbar-orange', logo: 'logo-white', color: '#C76D09' },
            { label: 'layout-topbar-magenta', logo: 'logo-white', color: '#972BB1' },
            { label: 'layout-topbar-bluegrey', logo: 'logo-white', color: '#406E7E' },
            { label: 'layout-topbar-deeppurple', logo: 'logo-white', color: '#543CD9' },
            { label: 'layout-topbar-brown', logo: 'logo-white', color: '#794F36' },
            { label: 'layout-topbar-lime', logo: 'logo-white', color: '#849201' },
            { label: 'layout-topbar-rose', logo: 'logo-white', color: '#8F3939' },
            { label: 'layout-topbar-cyan', logo: 'logo-white', color: '#0C8990' },
            { label: 'layout-topbar-teal', logo: 'logo-white', color: '#337E59' },
            { label: 'layout-topbar-deeporange', logo: 'logo-white', color: '#D74A1D' },
            { label: 'layout-topbar-indigo', logo: 'logo-white', color: '#3D53C9' },
            { label: 'layout-topbar-pink', logo: 'logo-white', color: '#BF275B' },
            { label: 'layout-topbar-purple', logo: 'logo-white', color: '#7F32DA' }
        ];

        this.themes = [
            { label: 'blue', color: '#0f97c7' },
            { label: 'green', color: '#10B163' },
            { label: 'orange', color: '#E2841A' },
            { label: 'magenta', color: '#B944D6' },
            { label: 'bluegrey', color: '#578697' },
            { label: 'deeppurple', color: '#6952EC' },
            { label: 'brown', color: '#97664A' },
            { label: 'lime', color: '#A5B600' },
            { label: 'rose', color: '#AB5353' },
            { label: 'cyan', color: '#1BA7AF' },
            { label: 'teal', color: '#4EA279' },
            { label: 'deeporange', color: '#F96F43' },
            { label: 'indigo', color: '#435AD8' },
            { label: 'pink', color: '#E93A76' },
            { label: 'purple', color: '#9643F9' }
        ];
    }

    changeTheme(theme: string) {
        this.changeStyleSheetsColor('layout-css', 'layout-' + theme + '.css');
        this.changeStyleSheetsColor('theme-css', 'theme-' + theme + '.css');
        this.themeColor = theme;
        this.savedSettings.themeColor = theme;
        this.setSettingInLocalStorage();
    }

    changeTopbarColor(topbarColor, logo) {
        // this.app.topbarColor = topbarColor;
        // const topbarLogoLink: HTMLImageElement = document.getElementById('topbar-logo') as HTMLImageElement;
        // topbarLogoLink.src = 'assets/layout/images/' + logo + '.png';
        // this.savedSettings.logo = logo;
        // this.savedSettings.topbarColor = topbarColor;
        // this.setSettingInLocalStorage();
    }

    changeStyleSheetsColor(id, value) {
        // const element = document.getElementById(id);
        // const urlTokens = element.getAttribute('href').split('/');
        // urlTokens[urlTokens.length - 1] = value;

        // const newURL = urlTokens.join('/');

        // this.replaceLink(element, newURL);

        // this.savedSettings.styleSheetsColorId = id;
        // this.savedSettings.styleSheetsColorValue = value;
        // this.setSettingInLocalStorage();
    }

    replaceLink(linkElement, href) {
        // if (this.isIE()) {
        //     linkElement.setAttribute('href', href);
        // }
        // else {
        //     const id = linkElement.getAttribute('id');
        //     const cloneLinkElement = linkElement.cloneNode(true);

        //     cloneLinkElement.setAttribute('href', href);
        //     cloneLinkElement.setAttribute('id', id + '-clone');

        //     linkElement.parentNode.insertBefore(cloneLinkElement, linkElement.nextSibling);

        //     cloneLinkElement.addEventListener('load', () => {
        //         linkElement.remove();
        //         cloneLinkElement.setAttribute('id', id);
        //     });
        // }

        // this.savedSettings.replaceLinkHref = href;
        // this.savedSettings.replaceLinkElement = linkElement;
        // this.setSettingInLocalStorage();
    }

    isIE() {
        return /(MSIE|Trident\/|Edge\/)/i.test(window.navigator.userAgent);
    }

    onConfigButtonClick(event) {
        this.app.configActive = !this.app.configActive;
        event.preventDefault();
    }

    onConfigCloseClick(event) {
        this.app.configActive = false;
        event.preventDefault();
    }

    onChangeLayoutModeClick(event) {
        if (this.app.layoutMode == 'horizantal') {
            this.app.inlineUser = false;
            this.savedSettings.inlineUser = false;
        }
        this.savedSettings.layoutMode = this.app.layoutMode;
        this.setSettingInLocalStorage();
    }

    onChangeInlineUser(){
        this.savedSettings.inlineUser = this.app.inlineUser;
        this.setSettingInLocalStorage();
    }

    onRippleChange(value){
        this.savedSettings.ripple = this.app.ripple = value.checked;
        this.setSettingInLocalStorage();
    }

    onChangeInputStyle(){
        this.savedSettings.inputStyle = this.app.inputStyle;
        this.setSettingInLocalStorage();
    }

    setSettingInLocalStorage() {
        //localStorage.setItem("app_settings", JSON.stringify(this.savedSettings));
    }

    getSettingInLocalStorage() {
        this.savedSettings = JSON.parse(localStorage.getItem("app_settings"));
        if (!this.ch.isNullOrUndefined(this.savedSettings)) {
            if (!this.ch.isNullOrUndefined(this.savedSettings.changeTheme)) {
                this.changeTheme(this.savedSettings.changeTheme);
            }

            if (!this.ch.isNullOrUndefined(this.savedSettings.topbarColor) && !this.ch.isNullOrUndefined(this.savedSettings.logo)) {
                this.changeTopbarColor(this.savedSettings.topbarColor, this.savedSettings.logo);
            }

            if (!this.ch.isNullOrUndefined(this.savedSettings.styleSheetsColorId) && !this.ch.isNullOrUndefined(this.savedSettings.styleSheetsColorValue)) {
                this.changeStyleSheetsColor(this.savedSettings.styleSheetsColorId, this.savedSettings.styleSheetsColorValue);
            }

            if (!this.ch.isNullOrUndefined(this.savedSettings.replaceLinkHref) && !this.ch.isNullOrUndefined(this.savedSettings.linkElement)) {
                this.replaceLink(this.savedSettings.replaceLinkHref, this.savedSettings.linkElement);
            }

            if (!this.ch.isNullOrUndefined(this.savedSettings.layoutMode)) {
                this.app.layoutMode = this.savedSettings.layoutMode;
                this.onChangeLayoutModeClick(null);
            }

            if (!this.ch.isNullOrUndefined(this.savedSettings.inlineUser)) {
                this.app.inlineUser = this.savedSettings.inlineUser;
            }

            if (!this.ch.isNullOrUndefined(this.savedSettings.ripple)) {
                this.app.ripple = this.savedSettings.ripple;
            }

            if (!this.ch.isNullOrUndefined(this.savedSettings.inputStyle)) {
                this.app.inputStyle = this.savedSettings.inputStyle;
            }
        }
        else {
            this.savedSettings = {
                changeTheme: null,
                topbarColor: null,
                styleSheetsColorId: null,
                replaceLinkHref: null,
                layoutMode: null,
                inlineUser: false,
                ripple: true,
                inputStyle: 'outlined'
            };
        }
    }
}
