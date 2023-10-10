import { Component } from '@angular/core';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-dialog-prompt',
  templateUrl: './prompt.component.html',
  styleUrls: ['./prompt.component.css']
})

export class PromptComponent {
  value: string = null;
  messageHelper: any = null;
  multiline = false;

  constructor(public ref: DynamicDialogRef, public config: DynamicDialogConfig) {
    this.messageHelper = this.config.data.messageHelper;
    this.multiline = this.config.data.multiline;
  }

  ok() {
    if (!this.config.data.nullable && (this.value === null || this.value === '')) {
      this.messageHelper.showWarnMessage('Değer boş olamaz');
      return;
    }
    this.ref.close({ type: 'ok', value: this.value });
  }

  cancel() {
    this.ref.close({ type: 'cancel', value: this.value });
  }
}
