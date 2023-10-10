import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { CommonHelper } from '../helpers/common-helper';
import * as moment from 'moment';
import { Router } from '@angular/router';
import { GlobalVariables } from '../constants/global-variables';


@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
  constructor(private ch: CommonHelper,private globals: GlobalVariables, public router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.ch.showLoader();
    if (!request.headers['lazyUpdate'] || request.headers['lazyUpdate'].filter(p => p.name === 'Authorization').length === 0) {
      request = request.clone({ setHeaders: { Authorization: 'Bearer ' + localStorage.getItem(this.globals.localStorageItems.authToken) } });
    }

    //this.convertDates(request);

    const nextFn = (event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        this.ch.hideLoader();
      }
    };

    const errorFn = (error: any) => {
      let errorMessage = 'İşlem sırasında bir hata oluştu.';
      if (error instanceof HttpErrorResponse) {
        const errorCode = error.headers.get('Application-Error-Code');
        if (error.status === 401) {
          errorMessage = 'İşlemi yapmanız için yetkiniz bulunmamaktadır.';
        }
        else if (error.status === 422) {
          errorMessage = error.error.messages[0].message;
        } else if (error.status === 403) {
          errorMessage = 'Kullanıcı oturumunuz sonlanmıştır. Lütfen tekrar giriş yapınız.';
          this.clearStoragesAndGoLogin();
        } else if (error.status === 400) {
          if (error?.error?.title) {
            errorMessage = error.error.title;
          }
        }

        if (errorCode) {
          errorMessage = `${errorMessage} Hata kodu:${errorCode}`;
        }
      }
      this.ch.messageHelper.showErrorMessage(errorMessage);
      this.ch.hideLoader();
    };

    return next.handle(request).pipe(tap({ next: nextFn, error: errorFn }));
  }

  // convertDates(request) {
  //   if (request.body && request.body instanceof FormData) {
  //     const bodyFormDataObject = JSON.parse(request.body.get('formData'));
  //     for (const property in bodyFormDataObject) {
  //       if (bodyFormDataObject.hasOwnProperty(property)) {
  //         if (moment(bodyFormDataObject[property], moment.ISO_8601, true).isValid()) {
  //           bodyFormDataObject[property] = moment(new Date(bodyFormDataObject[property])).format('YYYY-MM-DD[T]HH:mm:ss.SSS');
  //         } else if (bodyFormDataObject[property] instanceof Object) {
  //           this.convertDates(bodyFormDataObject[property]);
  //         }
  //       }
  //     }
  //     request.body.set('formData', JSON.stringify(bodyFormDataObject));
  //   } else {
  //     for (const property in request) {
  //       if (request.hasOwnProperty(property)) {
  //         if (request[property] instanceof Date) {
  //           debugger;
  //           request[property] = request[property] + ' ' + request[property];
  //         } else if (request[property] instanceof Object) {
  //           this.convertDates(request[property]);
  //         }
  //       }
  //     }
  //   }
  // }

  clearStoragesAndGoLogin() {
    this.ch.clearLocalStorageItems();
    this.ch.clearSessionStorageItems();
    this.router.navigate(['login']);
  }
}

export const LoaderInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: LoaderInterceptor,
  multi: true
};
