import { Injector } from '@angular/core';

export class AppInjector {
    private static injector: Injector;

    static setInjector(injector: Injector) {
        this.injector = injector;
    }

    static getInjector(): Injector {
        return this.injector;
    }
}
