import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';
import { MenuService } from '../../app-menu/app.menu.service';

@Component({
    selector: 'app-menu-layout',
    templateUrl: './menu-layout.component.html',
})
export class MenuLayoutComponent implements OnInit {
    layoutMode = 'static';
    overlayMenuActive: boolean;
    staticMenuDesktopInactive: boolean;
    staticMenuMobileActive: boolean;
    layoutMenuScroller: HTMLDivElement;
    lightMenu = true;
    topbarColor = 'layout-topbar-blue';
    menuClick: boolean;
    userMenuClick: boolean;
    notificationMenuClick: boolean;
    rightMenuClick: boolean;
    resetMenu: boolean;
    menuHoverActive: boolean;
    topbarUserMenuActive: boolean;
    topbarNotificationMenuActive: boolean;
    rightPanelMenuActive: boolean;
    inlineUser: boolean = false;
    isRTL: boolean = false;
    configActive: boolean = false;
    configClick: boolean;
    profileClick: boolean;
    inlineUserMenuActive = false;
    inputStyle: string = "outlined";
    ripple: boolean = true;

    constructor(private menuService: MenuService, private primengConfig: PrimeNGConfig) { }

    ngOnInit() {
        this.primengConfig.ripple = true;
    }

    onLayoutClick() {
        if (!this.userMenuClick) {
            this.topbarUserMenuActive = false;
        }

        if (!this.notificationMenuClick) {
            this.topbarNotificationMenuActive = false;
        }

        if (!this.rightMenuClick) {
            this.rightPanelMenuActive = false;
        }

        if (!this.profileClick && this.isSlim()) {
            this.inlineUserMenuActive = false;
        }

        if (!this.menuClick) {
            if (this.isHorizontal() || this.isSlim()) {
                this.menuService.reset();
            }

            if (this.overlayMenuActive || this.staticMenuMobileActive) {
                this.hideOverlayMenu();
            }

            this.menuHoverActive = false;
            this.unblockBodyScroll();
        }

        if (this.configActive && !this.configClick) {
            this.configActive = false;
        }

        this.configClick = false;
        this.userMenuClick = false;
        this.rightMenuClick = false;
        this.notificationMenuClick = false;
        this.menuClick = false;
        this.profileClick = false;
    }

    onMenuButtonClick(event) {
        this.menuClick = true;
        this.topbarUserMenuActive = false;
        this.topbarNotificationMenuActive = false;
        this.rightPanelMenuActive = false;

        if (this.isOverlay()) {
            this.overlayMenuActive = !this.overlayMenuActive;
        }

        if (this.isDesktop()) {
            this.staticMenuDesktopInactive = !this.staticMenuDesktopInactive;
        } else {
            this.staticMenuMobileActive = !this.staticMenuMobileActive;
            if (this.staticMenuMobileActive) {
                this.blockBodyScroll();
            } else {
                this.unblockBodyScroll();
            }
        }

        event.preventDefault();
    }

    onMenuClick(event) {
        this.menuClick = true;
        this.resetMenu = false;
    }

    onTopbarUserMenuButtonClick(event) {
        this.userMenuClick = true;
        this.topbarUserMenuActive = !this.topbarUserMenuActive;

        this.hideOverlayMenu();

        event.preventDefault();
    }

    onTopbarNotificationMenuButtonClick(event) {
        this.notificationMenuClick = true;
        this.topbarNotificationMenuActive = !this.topbarNotificationMenuActive;

        this.hideOverlayMenu();

        event.preventDefault();
    }

    onRightMenuClick(event) {
        this.rightMenuClick = true;
        this.rightPanelMenuActive = !this.rightPanelMenuActive;

        this.hideOverlayMenu();

        event.preventDefault();
    }

    onProfileClick(event) {
        this.profileClick = true;
        this.inlineUserMenuActive = !this.inlineUserMenuActive;
    }

    onTopbarSubItemClick(event) {
        event.preventDefault();
    }

    onConfigClick(event) {
        this.configClick = true;
    }

    onRippleChange(event) {
        this.ripple = event.checked;
    }

    isHorizontal() {
        return this.layoutMode === 'horizontal';
    }

    isSlim() {
        return this.layoutMode === 'slim';
    }

    isOverlay() {
        return this.layoutMode === 'overlay';
    }

    isStatic() {
        return this.layoutMode === 'static';
    }

    isMobile() {
        return window.innerWidth < 1025;
    }

    isDesktop() {
        return window.innerWidth > 896;
    }

    isTablet() {
        const width = window.innerWidth;
        return width <= 1024 && width > 640;
    }

    hideOverlayMenu() {
        this.overlayMenuActive = false;
        this.staticMenuMobileActive = false;
    }

    blockBodyScroll(): void {
        if (document.body.classList) {
            document.body.classList.add('blocked-scroll');
        } else {
            document.body.className += ' blocked-scroll';
        }
    }

    unblockBodyScroll(): void {
        if (document.body.classList) {
            document.body.classList.remove('blocked-scroll');
        } else {
            document.body.className = document.body.className.replace(new RegExp('(^|\\b)' +
                'blocked-scroll'.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
        }
    }
}
