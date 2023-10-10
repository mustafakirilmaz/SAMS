import { Component, Input, OnInit, TemplateRef, ViewEncapsulation, Output, EventEmitter, ElementRef } from '@angular/core';
import { BaseComponent } from '../../bases/base.component';
import { CommonService } from 'src/app/services/common.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-file-upload',
  templateUrl: './app-file-upload.component.html',
  styleUrls: ['./app-file-upload.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class FileUploadComponent extends BaseComponent implements OnInit {
  @Input() templateRef: TemplateRef<any>;
  @Input() id: string;
  @Input() fileLimit: number = null;

  private _folderName: string;
  @Input() set folderName(value: string) {
    this._folderName = value;
    this.getUploadedFiles();
  }
  get folderName(): string {
    return this._folderName;
  }

  private _referenceGuid: string;
  @Input() set referenceGuid(value: string) {
    this._referenceGuid = value;
    this.uploadUrl = this.ch.getFileUploadUrl(`${this._folderName}`, this._referenceGuid);
    this.getUploadedFiles();
  }
  get referenceGuid(): string {
    return this._referenceGuid;
  }

  @Input() multiple = false;
  @Input() auto = false;
  @Input() disabled = false;
  @Input() accept = "image/*, video/*, .pdf";
  @Output() uploadCallback = new EventEmitter<any>();
  @Output() removedCallback = new EventEmitter<any>();
  @Output() selectCallback = new EventEmitter<any>();

  uploadUrl: string;
  uploadedFiles: any[] = [];

  constructor(public cs: CommonService, private sanitizer: DomSanitizer, public elementRef: ElementRef) { super(); }

  ngOnInit() {}

  getUploadedFiles(callback = null) {
    if(this.ch.isNullOrUndefined(this._folderName) || this.ch.isNullOrUndefined(this._referenceGuid)){
      return;
    }
    this.cs.getUploadedFiles(this._folderName, this._referenceGuid).subscribe(result => {
      const files = <any>result;
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const splittedPaths = file.url.split('/');
        file.name = splittedPaths[splittedPaths.length-1];
      }
      this.uploadedFiles = files;
      if(this.fileLimit == files.length){
        this.hideFileUploadButtonBar();
      }
      else{
        this.showFileUploadButtonBar();
      }
      if (this.disabled) {
        this.hideFileUploadButtonBar();
      }
      if (!!callback) {
        callback();
      }
    });
  }

  onSelect(event) {
    for (const file of event.files) {
      this.selectCallback.emit({ id: this.id, file: file });
    }
  }

  onUpload() {
    this.ch.messageHelper.showInfoMessage('Yükleme tamamlandı...');
    this.getUploadedFiles(()=>{
      this.uploadCallback.emit(this.uploadedFiles);
    });
  }

  showFile(file) {
    const fileUrl = environment.baseUrl + file.url.substring(1, file.url.length);
    window.open(fileUrl, '_blank');
  }

  deleteFile(file) {
    this.ch.messageHelper.confirm(
      'Dosya silinecek. Devam etmek istiyor musunuz?', () => {
        this.cs.deleteFile(file.url).subscribe(result => {
          this.ch.messageHelper.showSuccessMessage();
          this.getUploadedFiles(()=>{
            this.removedCallback.emit(this.uploadedFiles);
          });
        });
      });
  }

  hideFileUploadButtonBar() {
    const dom: HTMLElement = this.elementRef.nativeElement;
    const elements = dom.querySelectorAll('.p-fileupload-buttonbar');
    for (let i = 0; i < elements.length; i++) {
      const element = elements[i];
      element.classList.add('hidden');
    }
  }

  showFileUploadButtonBar() {
    const dom: HTMLElement = this.elementRef.nativeElement;
    const elements = dom.querySelectorAll('.p-fileupload-buttonbar');
    for (let i = 0; i < elements.length; i++) {
      const element = elements[i];
      element.classList.remove('hidden');
    }
  }
}
