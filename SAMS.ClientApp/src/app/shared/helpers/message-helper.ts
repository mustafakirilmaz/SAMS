import { Injectable } from '@angular/core';
import { Confirmation, ConfirmationService, Message, MessageService } from 'primeng/api';
import { MessageType } from '../enums/message-type';

@Injectable({
  providedIn: 'root'
})

export class MessageHelper {

  constructor(private messageService: MessageService, private confirmationService: ConfirmationService) { }

  showSuccessMessage(message: string = 'İşlem başarıyla tamamlandı.') {
    const msg: Message = { severity: 'success', summary: 'Başarılı', detail: message };
    if (message.length > 50) {
      msg.sticky = true;
    }
    this.messageService.add(msg);
  }

  showInfoMessage(message: string) {
    const msg: Message = { severity: 'info', summary: 'Bilgi', detail: message };
    if (message.length > 50) {
      msg.sticky = true;
    }
    this.messageService.add(msg);
  }

  showWarnMessage(message: string) {
    const msg: Message = { severity: 'warn', summary: 'Uyarı', detail: message };
    if (message.length > 50) {
      msg.sticky = true;
    }
    this.messageService.add(msg);
  }

  showErrorMessage(message: string = 'işlem sırasında bir hata oluştu.') {
    const msg: Message = { severity: 'error', summary: 'Hata', detail: message };
    if (message.length > 50) {
      msg.sticky = true;
    }
    this.messageService.add(msg);
  }

  showMessage(messageType: MessageType, message?: string) {
    if (messageType === MessageType.Success) {
      this.showSuccessMessage(message);
    } else if (messageType === MessageType.Error) {
      this.showErrorMessage(message);
    } else if (messageType === MessageType.Warning) {
      this.showWarnMessage(message);
    } else if (messageType === MessageType.Info) {
      this.showInfoMessage(message);
    }
  }

  confirm(message: string, successCallback: Function, rejectCallback?: Function, header?: string, showCancel: boolean = true) {
    const confirmation: Confirmation = {
      message: message,
      header: header === null ? '' : 'İşlem Onayı',
      icon: 'pi pi-exclamation-triangle',
      rejectLabel: 'İptal',
      acceptLabel: 'Evet',
      rejectVisible: showCancel,
      accept: successCallback,
      reject: rejectCallback ? rejectCallback : () => { return; }
    };

    this.confirmationService.confirm(confirmation);
  }

  deleteConfirm(successCallback: Function, rejectCallback?: Function, header?: string, showCancel: boolean = true) {
    this.confirm('Kayıt silinecektir. Devam etmek istiyor musunuz?', successCallback, rejectCallback, header, showCancel);
  }
}