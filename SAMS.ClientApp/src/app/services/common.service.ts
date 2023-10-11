import { Injectable } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { HttpHelper } from './http-helper.service';
import { CommonHelper } from '../shared/helpers/common-helper';

@Injectable()
export class CommonService {
    constructor(private httpHelper: HttpHelper, private ch: CommonHelper) { }

    getRoles() {
        return this.httpHelper.get<SelectItem[]>('common', 'roles');
    }

    getUploadedFiles(folderName, referenceGuid) {
        return this.httpHelper.get('files', `${folderName}/list/${referenceGuid}`, null, null);
    }

    deleteFile(fileUrl) {
        const params = this.ch.createParams({ fileUrl: fileUrl });
        return this.httpHelper.delete('files', null, params, null);
    }

    getUsers(searchTerm) {
        const params = this.ch.createParams({ searchTerm: searchTerm });
        return this.httpHelper.get<SelectItem[]>('common', 'users', params);
    }

    getCities() {
        return this.httpHelper.get<SelectItem[]>('common', 'cities');
    }

    getTowns(cityCode: number) {
        return this.httpHelper.get<SelectItem[]>('common', 'towns', this.ch.createParams({ 'cityCode': cityCode }));
    }

    getDistricts(townCode: number) {
        return this.httpHelper.get<SelectItem[]>('Common', 'districts', this.ch.createParams({ 'townCode': townCode }));
    }
}
