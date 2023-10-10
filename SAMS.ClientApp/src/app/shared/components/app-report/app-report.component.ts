import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { BaseComponent } from '../../bases/base.component';
import { SelectItem } from 'primeng/api';

// import * as jsPDF from 'jspdf';
// import * as XLSX from 'xlsx';

@Component({
  selector: 'app-report',
  templateUrl: './app-report.component.html',
  styleUrls: ['./app-report.component.css']
})

export class ReportComponent extends BaseComponent implements OnInit {

  @ViewChild('reportContent') reportContent: ElementRef;
  @Input() reportTitle = 'Report';
  @Input() displayExcel = true;
  @Input() displayPdf = false;
  @Input() displayYazdir = false;
  @Input() displayYeniSekme = false;
  @Input() border = true;
  @Input() vertical = false;
  @Input() displayCustomToolbar = false;
  displayEImzaDialog: boolean;
  reportHtml: string;



  constructor() { super(); }

  ngOnInit() {

  }
  showEimza() {
    this.reportHtml = document.getElementById('content').innerHTML;
    this.displayEImzaDialog = true;
  }

  downloadPdf() {
    // const element = document.getElementById('content');
    // html2pdf(element, {
    //   // margin: 1,
    //   // filename: 'myfile.pdf',
    //   image: { type: 'jpeg', quality: 1 },
    //   html2canvas: { scale: 2, logging: true },
    //   // jsPDF: {unit: 'in', format: 'a4', orientation: 'l'}
    // });

    // if (this.pdfConvertMethod === 1) {
    //   const element = document.getElementById('content');
    //   html2pdf(element);
    // }
    // else if (this.pdfConvertMethod === 2) {
    //   const doc = new jsPDF();
    //   doc.addHTML(this.reportContent.nativeElement, function () {
    //     doc.save('Rapor.pdf');
    //   });
    // } else if (this.pdfConvertMethod === 3) {
    //   return xepOnline.Formatter.Format('content', { render: 'download' });
    // }
  }


  downloadExcel() {
    let content = this.reportContent.nativeElement;
    let style = '';
    if (this.border === true) {
      style = '<style>table,td,th {border: 1px solid black;border-collapse: collapse;}</style>';
    }

    const uri = 'data:application/vnd.ms-excel;base64,'
      , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/>' + style + '</head><body><table>{table}</table></body></html>'
      , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))); }
      , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }); };

    if (!content.nodeType) { content = document.getElementById(content); }
    const ctx = { worksheet: this.ch.createGuid() || 'Sayfa1', table: content.innerHTML };
    window.location.href = uri + base64(format(template, ctx));
  }

  // // downloadExcel() {
  // //   const content = this.reportContent.nativeElement;

  // //   const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(content);
  // //   const wb: XLSX.WorkBook = XLSX.utils.book_new();
  // //   XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

  // //   XLSX.writeFile(wb, 'SheetJS.xlsx', {cellStyles: true});
  // // }

  yeniSekmedeAc() {
    const newWindow = window.open();
    newWindow.document.write(this.reportContent.nativeElement.innerHTML);
    // newWindow.document.body.innerHTML = this.reportContent.nativeElement.innerHTML;
  }

  print(): void {
    let orientationCss = '';
    if (this.vertical) {
      orientationCss = '<link href="/assets/css/print-window-landscape.css" rel="stylesheet" type="text/css"/>';
    } else {
      orientationCss = '<link href="/assets/css/print-window-letter.css" rel="stylesheet" type="text/css"/>';
    }
    const printContents = this.reportContent.nativeElement.innerHTML;
    const popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
      <html>
        <head>
          <title></title>
          ${orientationCss}
          <style></style>
        </head>
        <body onload="window.print();window.close()">${printContents}</body>
      </html>`
    );
    popupWin.document.close();
  }





}
