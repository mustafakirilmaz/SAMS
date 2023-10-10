import { AfterViewInit, Directive, HostListener } from '@angular/core';
import { Dialog } from 'primeng/dialog';

@Directive({
  selector: 'p-dialog'
})

export class DialogDirective implements AfterViewInit {

  constructor(public dialog: Dialog) {
    dialog.transitionOptions = '0ms';
    dialog.responsive = true;
    dialog.modal = true;
    dialog.closeOnEscape = false;
    dialog.blockScroll = true;
    dialog.draggable = false;
    const dialogMaxHeight = (window.innerHeight - 100) + 'px';
    const dialogMaxWidth = (window.innerWidth - 10) + 'px';
    dialog.contentStyle = { 'max-height': dialogMaxHeight, 'max-width': dialogMaxWidth };
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    const dialogMaxHeight = (window.innerHeight - 100) + 'px';
    const dialogMaxWidth = (window.innerWidth - 10) + 'px';
    this.dialog.contentStyle = { 'max-height': dialogMaxHeight, 'max-width': dialogMaxWidth };
  }

  ngAfterViewInit() {
    this.dialog.positionTop = 20;
  }
}
