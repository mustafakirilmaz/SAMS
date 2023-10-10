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

    getDeviceTypes() {
        return this.httpHelper.get<SelectItem[]>('common', 'device-types');
    }

    getDeviceEnum() {
        return this.httpHelper.get<SelectItem[]>('common', 'device-enums');
    }

    getUsers(searchTerm) {
        const params = this.ch.createParams({ searchTerm: searchTerm });
        return this.httpHelper.get<SelectItem[]>('common', 'users', params);
    }

    getDevices(keywords = null) {
        const params = this.ch.createParams({ keywords: keywords });
        return this.httpHelper.get<SelectItem[]>('common', 'devices', params);
    }

    getDeviceProperties(keywords, parentId = null) {
        const params = this.ch.createParams({ keywords: keywords, parentId: parentId });
        return this.httpHelper.get<SelectItem[]>('common', 'device-properties', params);
    }
}
