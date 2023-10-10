import { Injectable } from '@angular/core';
import { CommonHelper } from '../../../app/shared/helpers/common-helper';
import { AppInjector } from './app-injector.service';
import { HttpHelper } from 'src/app/services/http-helper.service';
import { CommonService } from 'src/app/services/common.service';

@Injectable({
    providedIn: 'root'
})

export class BaseService {
    protected ch: CommonHelper;
    protected httpHelper: HttpHelper;
    protected cs: CommonService;

    constructor() {

        const injector = AppInjector.getInjector();
        this.ch = injector.get(CommonHelper);
        this.httpHelper = injector.get(HttpHelper);
        this.cs = injector.get(CommonService);
    }
}
