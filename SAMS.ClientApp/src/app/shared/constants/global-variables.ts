import { Injectable } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { DatatableInfo } from '../models/datatable-info';
import { IdNamePair } from '../models/id-name-pair';
import { environment } from 'src/environments/environment';

@Injectable()
export class GlobalVariables {
  loaderCount = 0;
  displayLoader = false;
  pageSizeOptions: number[] = [5, 10, 20, 50, 100, 250];

  TCKimlikNoMask = '99999999999';
  phoneNumberMask = '0(999) 999-9999';

  dataTableInfos?: DatatableInfo[] = [];
  fileUploadFormData = new FormData();

  menuItem: MenuItem[] = [];
  menuItemDefinitions: IdNamePair[] = [];

  dateFormat = 'DD.MM.YYYY';
  dateTimeFormat = 'DD.MM.YYYY HH:mm:ss';
  dateTimeWithoutSecondFormat = 'DD.MM.YYYY HH:mm';
  timeFormat = 'HH:mm:ss';
  dateTimeFormatForCalendar = 'YYYY-MM-DDTHH:mm:ss';
  tempData = {};
  selectedContents:any = [];

  isShowedEnvironmentMessage = false;

  safeUrls = {
    main: '/',
    jobs: environment.baseUrl,
  };

  localStorageItems = {
    authToken: 'auth_token'
  }
  
}
