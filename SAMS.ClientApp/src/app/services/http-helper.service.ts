import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonHelper } from '../shared/helpers/common-helper';
import ServiceResult from '../shared/models/service-result';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class HttpHelper {
  loaderDisplay = true;

  constructor(private http: HttpClient, private ch: CommonHelper) { }

  get<T>(controllerName: string, methodName: string, params: HttpParams = null, headers: HttpHeaders = null): Observable<ServiceResult<T>> {
    const requestUrl = this.getRequestUrl(controllerName, methodName);
    const headerAndParams = this.getHeaderAndParams(headers, params);

    return this.http.get<ServiceResult<T>>(requestUrl, headerAndParams);
  }

  post<T>(controllerName: string, methodName: string, postedObject, headers: HttpHeaders = null): Observable<ServiceResult<T>> {
    const requestUrl = this.getRequestUrl(controllerName, methodName);

    return this.http.post<ServiceResult<T>>(requestUrl, postedObject, { headers });
  }

  put<T>(controllerName: string, methodName: string, id: number, postedObject, headers: HttpHeaders = null): Observable<ServiceResult<T>> {
    const requestUrl = this.getRequestUrl(controllerName, methodName, id);

    return this.http.put<ServiceResult<T>>(requestUrl, postedObject, { headers });
  }

  delete<T, PostedObject>(controllerName: string, methodName: string, id = null, headers: HttpHeaders = null): Observable<ServiceResult<T>> {
    const requestUrl = this.getRequestUrl(controllerName, methodName);
    const headerAndParams = this.getHeaderAndParams(headers);

    if (id) {
      return this.http.delete<ServiceResult<T>>(`${requestUrl}?${id}`, { headers });
    }

    return this.http.delete<ServiceResult<T>>(`${requestUrl}`, { headers });
  }

  getBlob<T>(controllerName: string, methodName: string, params: HttpParams = null) {
    const requestUrl = this.getRequestUrl(controllerName, methodName);
    const headerAndParams = this.getHeaderAndParams(null, params, 'blob');

    return this.http.get(requestUrl, headerAndParams);
  }

  private getRequestUrl(controllerName, methodName, id = null) {
    let url = `${environment.apiUrl}${controllerName}`
    if (methodName) {
      url = `${url}/${methodName}`;
    }    
    if (id) {
      url = `${url}/${id}`;
    }
    return url;
  }

  private getHeaderAndParams(headers?, params?, responseType?) {
    return {
      headers: headers,
      params: params,
      responseType: responseType
    };
  }

  private convertDates(params) {
    for (const property in params) {
      if (params.hasOwnProperty(property)) {
        if (params[property] instanceof Date) {
          params[property] = params[property].toLocaleDateString('tr-TR') + ' ' + params[property].toLocaleTimeString('tr-TR');
        } else if (params[property] instanceof Object) {
          this.convertDates(params[property]);
        }
      }
    }
  }
}