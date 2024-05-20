import { DatePipe } from '@angular/common';
import { HttpParams, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidatorFn } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../../environments/environment';
import * as moment from 'moment';
import { SelectItem, TreeNode } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import * as XLSX from 'xlsx';
import { GlobalVariables } from '../constants/global-variables';
import { GridRefreshMode } from '../enums/grid-refresh-mode';
import { ResultType } from '../enums/result-type';
//import { Role } from '../enums/role';
import { CustomEncoder } from '../extensions/custom-encoder';
import { DatatableInfo } from '../models/datatable-info';
import { GridFilter } from '../models/grid-filter';
import ServiceResult from '../models/service-result';
import { UserInfo } from '../models/user-info';
import { MessageHelper } from './message-helper';
import { ColumnType } from '../enums/column-type';
import { NgxUiLoaderService } from 'ngx-ui-loader';

import { Constants } from '../constants/constants';
import { Table } from 'primeng/table';


@Injectable({
  providedIn: 'root'
})

export class CommonHelper {
  private _currentUser: UserInfo;

  get currentUser(): UserInfo {
    if (this.isNullOrUndefined(this._currentUser)) {
      this.setCurrentUser();
    }
    return this._currentUser;
  }
  set currentUser(currentUser: UserInfo) {
    this._currentUser = currentUser;
  }

  constructor(public messageHelper: MessageHelper, private globalVariables: GlobalVariables, public formBuilder: FormBuilder, private datePipe: DatePipe, private jwtHelper: JwtHelperService, private router: Router, private route: ActivatedRoute, private loaderService: NgxUiLoaderService, private constants: Constants) { }

  /**
    * Enum'daki tüm değerleri ve description'ları SelectItem array'e dönüştürür.
    * @param enumObj Değerleri alınacak enum.
    * @param descriptions Enum'a ait description değerleri.
    * @param exceptedValues (isteğe bağlı) Listeye eklenmesi istenilmeyen enum değer dizisi (Örnek parametre: [Cinsiyet.Bilinmiyor, Cinsiyet.Kadin])
    * @param sortByLabel (isteğe bağlı) Dizinin isme göre sıralanmasını sağlar. (Default = false)
    */
  enumToSelectItemArray<TEnum>(enumObj: TEnum, descriptions: Record<keyof TEnum, string>, exceptedValues: number[] = [], sortByLabel: boolean = false): SelectItem[] {
    let filteredObjectKeys = (Object.keys(enumObj) as Array<keyof TEnum>).filter(p => typeof enumObj[p] === 'number');
    for (let i = 0; i < exceptedValues.length; i++) {
      filteredObjectKeys = filteredObjectKeys.filter(p => p !== enumObj[exceptedValues[i]]);
    }
    let mappedSelectItems = filteredObjectKeys.map(p => ({
      label: descriptions[p],
      value: (enumObj[p])
    }));
    if (sortByLabel) {
      mappedSelectItems = mappedSelectItems.sort((a, b) => (a > b ? -1 : 1));
    }
    return mappedSelectItems;
  }

  /**
   * Belirtilen bir dizenin null, boş veya yalnızca boşluk olup olmadığını belirtir.
   * @param text Test Edilecek Değer.
   */
  isNullOrWhiteSpace(text) {
    if (typeof (text) === 'number' && text.toString() === '') {
      return true;
    } else if (typeof (text) === 'number' && text.toString() !== '') {
      return false;
    }

    return (typeof text === 'undefined' || text == null) || text.replace(/\s/g, '').length < 1;
  }

  /**
   * Dialog penceresinin full-screen olarak açılmasını sağlar.
   * @param dialog Full-screen görüntülenmek istenen dialog.
  */
  showDialogMaximized(dialog: Dialog) {
    setTimeout(() => {
      dialog.maximize();
    }, 0);
  }

  createParamsFromArray(objs: Object[]) {
    let params = new HttpParams({ encoder: new CustomEncoder() });
    for (let i = 0; i < objs.length; i++) {
      if (objs[i]) {
        for (const property in objs[i]) {
          const value = objs[i][property];
          if (value != null && value !== undefined) {
            params = params.append(encodeURIComponent(property), encodeURIComponent(value));
          }
        }
      }
    }
    return params;
  }

  /**
   * Gönderilen nesnenin tüm property'lerinden bir HttpParam oluşturur.
   * @param obj Full-screen görüntülenmek istenen dialog.
   * @param searchfilter (isteğe bağlı) Grid için search filter oluşturulmak istendiğinde kullanılır.
   */
  createParams(obj: Object, searchfilter?: Object) {
    let params = new HttpParams({ encoder: new CustomEncoder() });
    if (obj) {
      for (const property in obj) {
        if (obj.hasOwnProperty(property)) {
          const value = obj[property];
          if (!this.isNullOrUndefined(value)) {
            if (property === 'pageSize' && value === 247) { // Grid excel çıktısı için yapıldı.
              params = params.append('pageSize', '0'); // TODO: Bu rakam 999999999 yapılacak.
            } else {
              if (value instanceof Array) {
                for (const val of value) {
                  params = params.append(property, val); // (encodeURIComponent(property), encodeURIComponent(value));
                }
              } else {
                params = params.append(property, value); // (encodeURIComponent(property), encodeURIComponent(value));
              }
            }
          }
        }
      }
    }
    if (searchfilter) {
      for (const property in searchfilter) {
        if (searchfilter.hasOwnProperty(property)) {
          const value = searchfilter[property];
          if (!this.isNullOrUndefined(value)) {
            if (typeof (value) === 'object') {
              for (const innerValue of value) {
                params = params.append('searchFilter.' + property, innerValue); // (encodeURIComponent("searchFilter." + property), encodeURIComponent(value));
              }
            } else {
              params = params.append('searchFilter.' + property, value); // (encodeURIComponent("searchFilter." + property), encodeURIComponent(value));
            }
          }
        }
      }
    }
    return params;
  }

  createHeaders(obj: Object) {
    let params = new HttpHeaders();
    if (obj) {
      for (const property in obj) {
        if (obj.hasOwnProperty(property)) {
          const value = obj[property];
          if (!this.isNullOrUndefined(value)) {
            if (value instanceof Array) {
              for (const val of value) {
                params = params.append(property, val); // (encodeURIComponent(property), encodeURIComponent(value));
              }
            } else {
              params = params.append(property, value); // (encodeURIComponent(property), encodeURIComponent(value));
            }
          }
        }
      }
    }
    return params;
  }

  getGridFilter(dataTableId: string): GridFilter {
    const dataTableInfo = this.getDataTableInfo(dataTableId);
    const gridFilter = new GridFilter;
    if (dataTableInfo.datatable && dataTableInfo.gridRefreshMode !== GridRefreshMode.ExportExcel) {
      const sortField = dataTableInfo.datatable.sortField;
      gridFilter.pageFirstIndex = dataTableInfo.datatable.first;
      gridFilter.sortBy = sortField;
      gridFilter.isSortAscending = dataTableInfo.datatable.sortOrder === 1 ? true : false;
      gridFilter.pageSize = dataTableInfo.datatable.rows;
    } else {
      gridFilter.pageSize = 247; // Normalden farklı olduğu anlaşılması için herhangi bir sayı verildi(Excel export).Sıfıra primeng izin vermiyor.
    }
    return gridFilter;
  }

  getSearchFilter(dataTableId: string): Object {
    let filterForm = this.getFilterForm(dataTableId);
    let dataTableInfo = this.getDataTableInfo(dataTableId);
    if (filterForm) {
      if (this.isNullOrUndefined(dataTableInfo.searchFilter)) {
        dataTableInfo.searchFilter = {};
      }
      for (const key in filterForm.controls) {
        if (this.isNullOrUndefined(dataTableInfo.searchFilter[key])) {
          dataTableInfo.searchFilter[key] = '';
        }
      }
    }
    return dataTableInfo.searchFilter;
  }

  /**
   * Nesneyi FormGroup'a map etmek için kullanılır.
   * @param sourceObject kaynak nesne.
   * @param targetFormGroup hedef FormGroup.
   */
  mapToFormGroup(sourceObject: Object, targetFormGroup: FormGroup) {
    // tslint:disable-next-line:forin
    for (const key in targetFormGroup.controls) {
      const control = targetFormGroup.get(key);
      if (!this.isNullOrUndefined(sourceObject[key])) {
        const dateProp = moment(sourceObject[key], moment.ISO_8601);
        if (dateProp.isValid() && typeof (sourceObject[key]) !== 'number' && isNaN(sourceObject[key])) {
          control.setValue(new Date(dateProp.year(), dateProp.month(), dateProp.date(), dateProp.hour(), dateProp.minute(), dateProp.second()));
        } else if (typeof (sourceObject[key]) === 'string') {
          control.setValue(sourceObject[key].toString());
        } else {
          control.setValue(sourceObject[key]);
        }
      }

      sourceObject[key] = control.value;
    }
  }

  /**
   * FormGroup'u nesneye map etmek için kullanılır.
   * @param sourceObject kaynak FormGroup.
   * @param targetFormGroup hedef nesne.
   */
  mapToObject(sourceFormGroup: FormGroup, targetObject: Object) {
    // tslint:disable-next-line:forin
    for (const key in sourceFormGroup.controls) {
      const control = sourceFormGroup.get(key);
      // control.setValue(targetObject[key]);
      targetObject[key] = control.value;
    }
  }

  /**
   * Bir nesnenin istenilen property name'lerini değiştirmek için kullanılır.
   * @param obj Property adları değiştirilecek nesne.
   * @param differentPropertyNames Değiştirilecek property isimlerinin listesi.
   */
  convertObjectProperty(obj: Object, differentPropertyNames: [string[]]) {
    for (let i = 0; i < differentPropertyNames.length; i++) {
      obj[differentPropertyNames[i][1]] = obj[differentPropertyNames[i][0]];
      delete obj[differentPropertyNames[i][0]];
    }
    return obj;
  }

  /**
   * Bir nesne array'inin istenilen property name'lerini değiştirmek için kullanılır.
   * @param obj Property adları değiştirilecek nesne array'ı.
   * @param differentPropertyNames Değiştirilecek property isimlerinin listesi.
   */
  convertArrayProperty(objArray: Object[], differentPropertyNames: [string[]]) {
    // tslint:disable-next-line:forin
    for (const i in objArray) {
      this.convertObjectProperty(objArray[i], differentPropertyNames);
    }

    return objArray;
  }

  goFirstPage(dataTableId: string, dt?: Table) {
    var dataTableInfo = this.getDataTableInfo(dataTableId);
    if (dt) {
      dt.first = 0;
      dataTableInfo.datatable.first = 0;
    } else {
      dataTableInfo.datatable.first = 0;
    }
  }

  goLastPage(dataTableId: string, dt?: Table, isDeletedProcess: boolean = false) {
    var dataTableInfo = this.getDataTableInfo(dataTableId);
    if (dt) {
      if (isDeletedProcess) {
        if (dt.totalRecords % dt.rows === 1) {
          dt.first = (Math.ceil(((dt.totalRecords - 1) / dt.rows)) - 1) * dt.rows;
        }
      } else {
        dt.first = (Math.ceil(((dt.totalRecords + 1) / dt.rows)) - 1) * dt.rows;
      }
    } else {
      if (isDeletedProcess) {
        if (dataTableInfo.totalRecords % dataTableInfo.datatable.rows === 1) {
          dataTableInfo.datatable.first = (Math.ceil(((dataTableInfo.totalRecords - 1) / dataTableInfo.datatable.rows)) - 1) * dataTableInfo.datatable.rows;
          if (dataTableInfo.datatable.first < 0) {
            dataTableInfo.datatable.first = 0;
          }
        }
      } else {
        dataTableInfo.datatable.first = (Math.ceil(((dataTableInfo.totalRecords + 1) / dataTableInfo.datatable.rows)) - 1) * dataTableInfo.datatable.rows;
      }
    }
  }

  /**
  * Sayfanın ilk açılışında var olan tüm datatable bilgilerini temizlemek için kullanılır.
  */
  clearComponent(dataTableId: string) {
    for (let i = 0; i < this.globals.dataTableInfos.length; i++) {
      let dataTableInfo = this.getDataTableInfo(dataTableId);
      if (dataTableInfo) {
        dataTableInfo.datatable = null;
        dataTableInfo.cols = null;
        dataTableInfo.totalRecords = null;
        dataTableInfo.selectedColumns = null;
        dataTableInfo.displayGrid = false;
        dataTableInfo.searchFilter = {};
        setTimeout(() => {
          dataTableInfo.loading = true;
        }, 0);
      }
      const filterForm = this.getFilterForm(dataTableId);
      if (filterForm) {
        filterForm.reset();
      }
    }
  }

  clearFilters(dataTableId: string, ignoredProperties: string[] = []) {
    this.getFilterForm(dataTableId).reset();
    const dataTableInfo = this.getDataTableInfo(dataTableId);
    if (dataTableInfo && dataTableInfo.datatable) {
      dataTableInfo.datatable.first = 0;
      dataTableInfo.datatable.sortField = '';
      dataTableInfo.datatable.sortOrder = 1;
      dataTableInfo.datatable.rows = 10;
      if (ignoredProperties.length > 0) {
        for (const prop of Object.keys(dataTableInfo.searchFilter)) {
          if (!ignoredProperties.includes(prop)) {
            delete dataTableInfo.searchFilter[prop];
          }
        }
      } else {
        dataTableInfo.searchFilter = {};
      }

      dataTableInfo.datatable.reset();
      setTimeout(() => {
        dataTableInfo.loading = true;
      }, 0);
    }
  }

  private setSearchFilterFromFormGroup(filterForm: FormGroup, dataTableId: string) {
    const dataTableInfo = this.getDataTableInfo(dataTableId);
    dataTableInfo.searchFilter = {};
    if (filterForm) {
      for (const control in filterForm.controls) {
        if (filterForm.controls.hasOwnProperty(control)) {
          let valueOfControl = filterForm.controls[control].value;
          if (!this.isNullOrUndefined(valueOfControl) && valueOfControl.toString().indexOf(' (GMT') > 0) {
            valueOfControl = filterForm.controls[control].value.toLocaleDateString() + ' ' + filterForm.controls[control].value.toLocaleTimeString();
          }
          dataTableInfo.searchFilter[control] = valueOfControl;
        }
      }
    }
  }

  ////kontrol edilecek
  getSearchFilterFromFormGroup(filterForm: FormGroup, dataTableId: string): object {
    const searchFilter = {};
    for (const control in filterForm.controls) {
      if (filterForm.controls.hasOwnProperty(control)) {
        let valueOfControl = filterForm.controls[control].value;
        if (!this.isNullOrUndefined(valueOfControl) && valueOfControl.toString().indexOf(' (GMT') > 0) {
          valueOfControl = filterForm.controls[control].value.toLocaleDateString() + ' ' + filterForm.controls[control].value.toLocaleTimeString();
        }
        searchFilter[control] = valueOfControl;
      }
    }
    return searchFilter;
  }

  beforeGridRefresh(dataTableId: string, searchFilterIgnorance: boolean = false) {
    const dataTableInfo = this.getDataTableInfo(dataTableId);
    if (dataTableInfo && dataTableInfo.gridRefreshMode !== GridRefreshMode.ExportExcel) {
      setTimeout(() => {
        dataTableInfo.loading = true;
      }, 0);
    } else {
      this.showLoader();
    }

    const filterForm = this.getFilterForm(dataTableId);
    if (dataTableInfo.gridRefreshMode === GridRefreshMode.LazyLoad) { // 1. sayfadan 2. sayfaya geçişlerde sayfada değiştirilmiş olan filtreler uygulanmamalı. Bu yüzden lazy load modunda sadece sayfa ilk açıldığında setSearchFilterFromFormGroup metodu çağrılır.
      this.setSearchFilterFromFormGroup(filterForm, dataTableId);
    }
    else if (dataTableInfo.gridRefreshMode === GridRefreshMode.Search) {
      this.setSearchFilterFromFormGroup(filterForm, dataTableId);
      this.goFirstPage(dataTableId, dataTableInfo.datatable);
    }

    setTimeout(() => {
      if (dataTableInfo.gridRefreshMode !== GridRefreshMode.ExportExcel) {
        if (dataTableInfo && dataTableInfo.gridData) {
          dataTableInfo.gridData = null; // Sayfa geçişlerinde önceki sayfadaki gridin kayıtlarının gösterilmemesi için data null'lanıyor.
        }
      }
    }, 0);
  }

  gridDatabind(result: any, dataTableId: string, propertyConvertion: [string[]] = null) {
    setTimeout(() => {
      if (propertyConvertion != null) {
        result.data = this.convertArrayProperty(result.data, propertyConvertion);
      }

      const datatableInfo = this.getDataTableInfo(dataTableId);

      if (datatableInfo.gridRefreshMode === GridRefreshMode.ExportExcel) {
        const exportObject = [];
        const columns = datatableInfo.cols;

        // tslint:disable-next-line:forin
        for (const i in result.data) {
          const excelRow = {};
          for (const j in columns) {
            if (columns[j]['type'] === 'date') {
              excelRow[columns[j]['header']] = this.convertFormattedDatetime(result.data[i][columns[j]['field']]);
            } else {
              excelRow[columns[j]['header']] = result.data[i][columns[j]['field']];
            }
          }
          exportObject.push(excelRow);
        }
        this.exportExcelFromJson(exportObject);
        this.hideLoader();
        datatableInfo.gridRefreshMode = GridRefreshMode.Search;
      } else {
        datatableInfo.gridData = result.data;
        datatableInfo.totalRecords = parseInt(!this.isNullOrUndefined(result.messages) ? result.messages[0] : 0, 10);
        setTimeout(() => {
          datatableInfo.loading = false;
        }, 0);
      }

      datatableInfo.displayGrid = true;
    }, 0);
  }

  setGridRefreshAsSearch(dataTableId: string) {
    const dataTableInfo = this.getDataTableInfo(dataTableId);
    dataTableInfo.gridRefreshMode = GridRefreshMode.Search;
  }

  getDataTableInfo(dataTableId: string): DatatableInfo {
    let hasFound = false;
    let dataTableInfo = new DatatableInfo();
    for (let i = 0; i < this.globals.dataTableInfos.length; i++) {
      dataTableInfo = this.globals.dataTableInfos[i];
      if (dataTableInfo.id == dataTableId) {
        hasFound = true;
        break;
      }
    }
    if (!hasFound) {
      dataTableInfo = new DatatableInfo();
      dataTableInfo.id = dataTableId;
      dataTableInfo.gridRefreshMode = GridRefreshMode.LazyLoad;
      this.globals.dataTableInfos.push(dataTableInfo);
    }
    return dataTableInfo;
  }

  getFilterForm(dataTableId: string): FormGroup {
    return this.getDataTableInfo(dataTableId).filterForm;
  }

  setFilterForm(filterForm: FormGroup, dataTableId: string) {
    return this.getDataTableInfo(dataTableId).filterForm = filterForm;
  }

  createColumns(columnArray: any[], dataTableId: string) {
    const columns = [];

    for (let i = 0; i < columnArray.length; i++) {
      if (columnArray[i] !== null) {
        const column = { field: '', header: '' };
        column['field'] = columnArray[i][0];
        column['header'] = columnArray[i][1];
        if (columnArray[i][2] != null) {
          column['type'] = columnArray[i][2];
        }
        columns.push(column);
      }
    }

    let dataTableInfo = this.getDataTableInfo(dataTableId);
    dataTableInfo.cols = columns;
    dataTableInfo.selectedColumns = columns;
    this.getDataTableInfo(dataTableId).cols = columns;
  }

  get globals() {
    return this.globalVariables;
  }

  showLoader() {
    if (this.globals.loaderCount == 0) {
      setTimeout(() => {
        this.globals.displayLoader = true;
      }, 0);
      //this.loaderService.start();
    }
    this.globals.loaderCount++;
  }

  hideLoader() {
    this.globals.loaderCount--;
    if (this.globals.loaderCount == 0) {
      //this.loaderService.stop();
      setTimeout(() => {
        this.globals.displayLoader = false;
      }, 250);
    }
  }

  exportExcelFromJson(jsonData, columnMapping: any = null, dosyaAdi: string = ''): void {
    if (!this.isNullOrUndefined(columnMapping)) {
      for (const mapping of columnMapping) {
        if (!this.isNullOrUndefined(mapping[2])) {
          for (const obj of jsonData) {
            if (mapping[2] === ColumnType.Date) {
              obj[mapping[0]] = !this.isNullOrUndefined(obj[mapping[0]]) ? this.convertFormattedDate(obj[mapping[0]]) : '';
            } else if (mapping[2] === ColumnType.DateTime) {
              obj[mapping[0]] = !this.isNullOrUndefined(obj[mapping[0]]) ? this.convertFormattedDatetime(obj[mapping[0]]) : '';
            }
          }
        }
      }
      this.convertArrayProperty(jsonData, columnMapping);
    }

    let fileName = '';
    if (!this.isNullOrWhiteSpace(dosyaAdi)) {
      fileName = dosyaAdi + '.xlsx';
    } else {
      fileName = this.createGuid() + '.xlsx';
    }

    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(jsonData, { dateNF: 'dd.MM.yyyy' });
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Page1');

    XLSX.writeFile(wb, fileName);
  }

  getLabelFromSelectList(selectList: SelectItem[], id: any) {
    return selectList.filter(p => p.value === id)[0].label;
  }

  createGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      const r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  convertFormattedDate(date) {
    if (!this.isNullOrUndefined(date)) {
      if (date.toString().length > 10) {
        return this.datePipe.transform(date, 'dd.MM.yyyy');
      } else {
        return date;
      }
    }
  }

  convertFormattedDatetime(datetime) {
    return this.datePipe.transform(datetime, 'dd.MM.yyyy HH:mm:ss');
  }

  getValueFromGridData(id: number, propertyName: string, dataTableId: string) {
    if (id > 0) {
      const data = this.getDataTableInfo(dataTableId).gridData.filter(p => p.id === id);
      if (data.length > 0) {
        return data[0][propertyName];
      } else {
        return null;
      }
    } else {
      return null;
    }
  }

  nthIndex(str, pat, n) {
    const L = str.length;
    let i = -1;
    while (n-- && i++ < L) {
      i = str.indexOf(pat, i);
      if (i < 0) { break; }
    }
    return i;
  }

  getControlValue(formGroup: FormGroup, key: string) {
    return formGroup.controls[key].value;
  }

  setControlValue(formGroup: FormGroup, key: string, value: any) {
    return formGroup.controls[key].setValue(value);
  }

  changeControlValidation(formGroup: FormGroup, key: string, validator: ValidatorFn[]) {
    const formControl = formGroup.controls[key];
    formControl.setValidators(validator);
    formControl.updateValueAndValidity();
  }

  validateAllFormFields(formGroup: FormGroup, showMessage: boolean = false): boolean {
    let isValid = true;
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control.invalid) {
        isValid = false;
        if (control instanceof FormControl) {
          control.markAsTouched({ onlySelf: true });
          control.markAsDirty({ onlySelf: true });
        } else if (control instanceof FormGroup) {
          this.validateAllFormFields(control);
        }
      }
    });
    if (!isValid && showMessage) {
      this.messageHelper.showWarnMessage('Giriş yaptığınız alanları kontrol ediniz.')
    }
    return isValid;
  }

  isNullOrUndefined(obj: any) {
    if (obj === undefined || obj === null || obj === 'null') {
      return true;
    } else {
      return false;
    }
  }

  getMessageText(result: ServiceResult<any>): string {
    let messageText = '';
    if (!this.isNullOrUndefined(result.messages) && result.messages.length >= 0) {
      if (result.messages.length === 1) {
        messageText = result.messages[0];
      } else {
        let i = 1;
        for (const message of result.messages) {
          messageText += i + ') ' + message + '\n';
          i++;
        }
      }
    }
    return messageText;
  }
  checkResult(result: ServiceResult<any>): boolean {
    let checkResult = true;
    let messageText = '';
    if (!this.isNullOrUndefined(result.messages) && result.messages.length >= 0) {
      if (result.resultType === ResultType.Warning || result.resultType === ResultType.Error) {
        if (result.messages.length === 1) {
          messageText = result.messages[0];
          this.messageHelper.showErrorMessage(messageText);
        } else {
          let sayac = 1;
          for (const message of result.messages) {
            messageText += sayac + ') ' + message + '\n';
            sayac++;
          }
          this.messageHelper.showErrorMessage(messageText);
        }
      }
    }

    if (result.resultType === ResultType.Warning || result.resultType === ResultType.Error) {
      checkResult = false;
    }
    return checkResult;
  }

  getClientBaseUrl() {
    return environment.baseUrl;
  }

  getBaseUrl() {
    return environment.apiUrl;
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem(this.globals.localStorageItems.authToken);
    if (token === 'null') {
      return false;
    }
    return !this.jwtHelper.isTokenExpired(token);
  }

  clearLocalStorageItems() {
    localStorage.setItem(this.globals.localStorageItems.authToken, null);
  }

  clearSessionStorageItems() {
    sessionStorage.setItem(this.globals.localStorageItems.authToken, null);
  }

  hasRole(role: string, isSelectedRole: boolean = true) {
    if (this.currentUser.selectedRole === undefined) {
      this.clearLocalStorageItems();
      this.clearSessionStorageItems();
      window.location.reload();
    }
    if (isSelectedRole) {
      const selectedRole = JSON.parse(this.currentUser.selectedRole);
      return selectedRole.RoleName === role ? true : false;
    }
    for (let i = 0; i < this.currentUser.roles.length; i++) {
      const userRole = JSON.parse(this.currentUser.roles[i]);
      if (userRole.RoleName === role) {
        return true;
      }
    }
    return false;
  }

  hasRoles(roles: string[]) {
    let result = false;
    roles.forEach(element => {
      if (!result) {
        if (this.hasRole(element)) {
          result = true;
        }
      }
    });
    return result;
  }

  hasAnyRole(roles: []) {
    let result = false;
    roles.forEach(role => {
      if (this.hasRole(role)) {
        result = true;
      }
    });
    return result;
  }

  createFormDataWithFiles(fileParameterNames: string[], additionalObject: object) {
    const formData = new FormData();
    const fileObjectName = 'uploadedFiles';
    const additionalObjectName = 'formData';

    for (let i = 0; i < fileParameterNames.length; i++) {
      const fileParameterName = fileParameterNames[i];
      formData.append(`${fileObjectName}[${i}].value`, fileParameterName.toString());
      const files = this.globals.fileUploadFormData.getAll(fileParameterName);
      for (const file of files) {
        formData.append(`${fileObjectName}[${i}].files`, file);
      }
    }
    if (!this.isNullOrUndefined(additionalObject) && !this.isNullOrWhiteSpace(additionalObjectName)) {
      formData.append(additionalObjectName, JSON.stringify(additionalObject));
    }
    return formData;
  }

  setFilesOnInitial(key: string, fileNames: string[] = []) {
    this.globals.fileUploadFormData.delete(key);
    if (fileNames.length > 0) {
      for (const fileName of fileNames) {
        const file = new File([''], fileName, { type: 'text/plain' });
        this.globals.fileUploadFormData.append(key, file);
      }
    }
  }

  getUrlParams(route: ActivatedRoute = null) {
    if (Object.keys(this.route.snapshot.params).length > 0) {
      return this.convertUrlParamsToObject(this.route.snapshot.params['q']);
    }
    if (Object.keys(this.route.snapshot.queryParams).length > 0) {
      return this.convertUrlParamsToObject(this.route.snapshot.queryParams['q']);
    }
    if (!!route) {
      if (Object.keys(route.snapshot.params).length > 0) {
        return this.convertUrlParamsToObject(route.snapshot.params['q']);
      }
      if (Object.keys(route.snapshot.queryParams).length > 0) {
        return this.convertUrlParamsToObject(route.snapshot.queryParams['q']);
      }
    }

    return {};
  }

  getUrlParamsWithoutEncode(route: ActivatedRoute = null) {
    if (Object.keys(this.route.snapshot.params).length > 0) {
      return this.route.snapshot.params;
    }
    if (Object.keys(this.route.snapshot.queryParams).length > 0) {
      return this.route.snapshot.queryParams;
    }
    if (!!route) {
      if (Object.keys(route.snapshot.params).length > 0) {
        return route.snapshot.params;
      }
      if (Object.keys(route.snapshot.queryParams).length > 0) {
        return route.snapshot.queryParams;
      }
    }

    return {};
  }

  private convertUrlParamsToObject(params: string): any {
    const splittedParams = params.split('&');
    const convertedObject = new Object();
    for (let i = 0; i < splittedParams.length; i++) {
      const paramKey = splittedParams[i].split('=')[0];
      const paramValue = splittedParams[i].split('=')[1];
      if (convertedObject.hasOwnProperty(paramKey)) {
        for (const property in convertedObject) {
          if (convertedObject.hasOwnProperty(property)) {
            const previousValue = convertedObject[property];
            if (previousValue instanceof Array) {
              convertedObject[property].push(paramValue);
            } else {
              convertedObject[property] = new Array();
              convertedObject[property].push(previousValue);
              convertedObject[property].push(paramValue);
            }
          }
        }
      } else {
        convertedObject[paramKey] = paramValue;
      }
    }
    return convertedObject;
  }

  convertUrlParamsToQueryString(options: object) {
    let params = new URLSearchParams();
    for (let key in options) {
      params.set(key, options[key])
    }
    return params.toString();
  }

  isFileAnImage(fileName: string) {
    return fileName.match(/.(jpg|jpeg|png|gif)$/i);
  }

  removeGridColumn(columns, silinecekKolonAdlari: string[]): [] {
    const designedColumns = columns.slice();
    for (const silinecekKolonAdi of silinecekKolonAdlari) {
      designedColumns.splice(designedColumns.indexOf((designedColumns.filter(p => p === designedColumns.filter(q => q[0] === silinecekKolonAdi)[0]))[0]), 1);
    }
    return designedColumns;
  }

  copyObject(obj) {
    return JSON.parse(JSON.stringify(obj));
  }

  // TODO: Recursive yapılacak
  setFormDisableOrEnable(form: FormGroup, disable: boolean) {
    for (const control in form.controls) {
      if (form.controls.hasOwnProperty(control)) {
        if (disable === true) {
          form.controls[control].disable();
        } else {
          form.controls[control].enable();
        }
      }
    }
  }

  setCurrentUser(): UserInfo {
    if (this.isNullOrUndefined(localStorage.getItem(this.globals.localStorageItems.authToken)) || this.isNullOrWhiteSpace(localStorage.getItem(this.globals.localStorageItems.authToken))) {
      return;
    }
    const token = localStorage.getItem(this.globals.localStorageItems.authToken);
    const decodedToken = this.jwtHelper.decodeToken(token);
    decodedToken.id = decodedToken.userId;
    this._currentUser = decodedToken;
    Object.keys(decodedToken).forEach(field => {
      this.currentUser[field] = decodedToken[field];
    });
    this.currentUser.token = token;
    const unnecessaryFields = ['exp', 'nbf', 'iat'];
    if (typeof this.currentUser.roles === "string") {
      this.currentUser.roles = [this.currentUser.roles];
    }
    unnecessaryFields.forEach(field => {
      delete this.currentUser[field];
    });
    
    return this.currentUser;
  }

  createSelectItem(value: any, label?: string, styleClass?: string, icon?: string, title?: string, disabled?: boolean): SelectItem {
    return {
      label: label,
      value: value,
      styleClass: styleClass,
      icon: icon,
      title: title,
      disabled: disabled
    };
  }

  urlEncrypt(obj: Object) {
    let urlParams = '';
    if (obj) {
      for (const property in obj) {
        if (obj.hasOwnProperty(property)) {
          const value = obj[property];
          if (value instanceof Array) {
            for (let i = 0; i < value.length; i++) {
              urlParams += property + '=' + (!this.isNullOrUndefined(value[i]) ? value[i] : '') + '&';
            }

          } else {
            urlParams += property + '=' + (!this.isNullOrUndefined(value) ? value : '') + '&';
          }
        }
      }
      urlParams = urlParams.substring(0, urlParams.length - 1);
    }

    //urlParams = this.encrypt(urlParams);
    return { q: urlParams };
    // return { queryParams: { q: urlParams } };
  }

  createRandomColorList(length: number) {
    const colorList = [];
    for (let index = 0; index < length; index++) {
      const hex = '0123456789ABCDEF';
      let color = '#';
      for (let i = 1; i <= 6; i++) {
        color += hex[Math.floor(Math.random() * 16)];
      }
      colorList.push(color);
    }
    return colorList;
  }

  maxDateNowForCalender() {
    const now = moment();
    return new Date(now.year(), now.month(), now.date(), now.hour(), now.minute(), now.second());
  }

  setCacheValue(name: string, value: any) {
    this.globals.tempData['name'] = value;
  }

  getCacheValue(name: string) {
    return this.isNullOrUndefined(this.globals.tempData['name']) ? null : this.globals.tempData['name'];
  }

  getCacheFilterAsFormGroup(name: string, formGroup: FormGroup) {
    const cachedValue = this.globals.tempData['name'];
    if (!this.isNullOrUndefined(cachedValue)) {
      this.mapToFormGroup(cachedValue, formGroup);
    }
  }

  changeFormControlValidation(form: FormGroup, controlName: string, validators: any = null) {
    if (validators === null) {
      form.get(controlName).clearValidators();
    }
    else {
      form.get(controlName).setValidators(validators);
    }
    form.get(controlName).updateValueAndValidity({ onlySelf: true });
  }

  getFileUploadUrl(folderName: string, referenceGuid: string) {
    return `${environment.apiUrl}files/${folderName}/add/${referenceGuid}`;
  }

  // createCkEditor(elementId = 'editor') {
  //   ClassicEditor.create(document.querySelector('#' + elementId), {
  //     language: 'tr',
  //     placeholder: 'İçeriği buraya giriniz!',
  //     plugins: [SimpleUploadAdapter],
  //     simpleUpload: {
  //       uploadUrl: `${environment.fileApiUrl}files/blog/add/ckeditor`,
  //       headers: {
  //         Authorization: 'Bearer ' + localStorage.getItem(this.globals.localStorageItems.authToken)
  //       }
  //     },
  //   }).then().catch();
  // }

  TCNOKontrol(TCNO) {
    let tek = 0; let cift = 0; let sonuc = 0; let TCToplam = 0; let index = 0;

    if (TCNO.length != 11) return false;
    if (isNaN(TCNO)) return false;
    if (TCNO[0] == 0) return false;

    tek = parseInt(TCNO[0]) + parseInt(TCNO[2]) + parseInt(TCNO[4]) + parseInt(TCNO[6]) + parseInt(TCNO[8]);
    cift = parseInt(TCNO[1]) + parseInt(TCNO[3]) + parseInt(TCNO[5]) + parseInt(TCNO[7]);

    tek = tek * 7;
    sonuc = Math.abs(tek - cift);
    if (sonuc % 10 != TCNO[9]) return false;

    for (let index = 0; index < 10; index++) {
      TCToplam += parseInt(TCNO[index]);
    }

    if (TCToplam % 10 != TCNO[10]) return false;

    return true;
  }

  setContentTreeDefaultProperties(treeArray: any, parent: any = null) {
    for (let i = 0; i < treeArray.length; i++) {
      const item = treeArray[i];
      item.isLeaf = this.isNullOrUndefined(item.children) || item.children.length == 0;
      item.expandedIcon = "pi pi-folder-open";
      item.collapsedIcon = "pi pi-folder";
      item.partialSelected = false;
      //.selectable = false;
      item.parent = parent;

      if (!this.isNullOrUndefined(item.children) && item.children.length > 0) {
        this.setContentTreeDefaultProperties(item.children, item);
      }

      if (item.isLeaf) {
        let selectedCount = 0;
        for (let j = 0; j < treeArray.length; j++) {
          const itemControl = treeArray[j];
          if (itemControl.isSelected) selectedCount++;
        }
        if (selectedCount > 0 && selectedCount != treeArray.length && !!parent) parent.partialSelected = true;
        if (selectedCount > 0 && selectedCount == treeArray.length) item.isSelected = true;
      }
      else {
        if (item.partialSelected && !this.isNullOrUndefined(parent)) {
          parent.partialSelected = true;
        }

        let selectedCount = 0;
        for (let j = 0; j < item.children.length; j++) {
          const itemControl = item.children[j];
          if (itemControl.isSelected) selectedCount++;
        }
        if (selectedCount == item.children.length) {
          item.isSelected = true;
        }
        if (selectedCount > 0 && selectedCount != item.children.length) {
          item.partialSelected = true;
          if (!this.isNullOrUndefined(parent)) parent.partialSelected = true;
        }
      }
    }
  }

  flattenContentTree(treeArray: any): any {
    const items = [];
    for (let i = 0; i < treeArray.length; i++) {
      const item = treeArray[i];
      items.push(item);
      if (!this.isNullOrUndefined(item.children) && item.children.length > 0) {
        items.push(...this.flattenContentTree(item.children));
      }
    }
    return items;
  }

  getFullUrl(url) {
    if (this.isNullOrUndefined(url) || this.isNullOrWhiteSpace(url)) return url;
    url = url[0] == "/" ? url = url.slice(1, url.length) : url;
    return environment.baseUrl + url;
  }

  setContentBreadCrumb(contentType) {
    let index = 0;
    for (let i = 0; i < this.globals.selectedContents.length; i++) {
      const item = this.globals.selectedContents[i];
      if (item.type === contentType) {
        index = i;
      }
    }
    if (index > 0) {
      this.globals.selectedContents.splice(index, this.globals.selectedContents.length - index);
    }
  }

  printCanvas(elementId, title = "") {
    var canvas = document.querySelector('#' + elementId + ' canvas') as HTMLCanvasElement;
    var dataUrl = canvas.toDataURL();
    let windowContent = '<!DOCTYPE html>';
    windowContent += '<html>';
    windowContent += '<head><title>' + title + '</title></head>';
    windowContent += '<body>';
    windowContent += '<img src="' + dataUrl + '">';
    windowContent += '</body>';
    windowContent += '</html>';

    const printWin = window.open('', '', 'width=' + screen.availWidth + ',height=' + screen.availHeight);
    printWin.document.open();
    printWin.document.write(windowContent);

    printWin.document.addEventListener('load', function () {
      printWin.focus();
      printWin.print();
      printWin.document.close();
      printWin.close();
    }, true);
  }
}
