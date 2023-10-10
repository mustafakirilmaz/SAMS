import { AfterContentInit, Component, ContentChildren, ElementRef, EventEmitter, Input, OnInit, Output, QueryList, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from '../../bases/base.component';
import { CellTemplateDirective } from '../../directives/cell-template.directive';
import { ColumnType } from '../../enums/column-type';
import { GridRefreshMode } from '../../enums/grid-refresh-mode';
import { DatatableInfo } from '../../models/datatable-info';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-grid',
  templateUrl: './app-grid.component.html',
  styleUrls: ['./app-grid.component.css']
})

export class GridComponent extends BaseComponent implements OnInit, AfterContentInit {
  @ContentChildren(CellTemplateDirective) cellTemplates: QueryList<CellTemplateDirective>;
  cellTemplatesMap: { [key: string]: TemplateRef<any> };
  @ViewChild('dt') pTableRef: any;

  @Output() gridRefresh: EventEmitter<any> = new EventEmitter();
  @Output() rowSelect = new EventEmitter();
  @Output() rowUnselect = new EventEmitter();
  @Input() coloring: any;
  @Input() dataTableId = '';
  @Input() rowCount = true;
  @Input() sorting = true;
  @Input() reorderableColumns = true;
  @Input() resizableColumns = true;
  @Input() totalRecordSummary = true;
  @Input() paginator = true;
  @Input() exportExcelButton = true;
  @Input() exportPdfButton = true;
  @Input() columnSelection = true;
  @Input() dataKey = 'id';
  @Input() deferred = false;
  @Input() rowExpansion = false;

  dataTableInfo: DatatableInfo = null;
  columnTypeEnum = ColumnType;

  constructor() { super(); }

  ngOnInit() {}

  ngAfterContentInit() {
    this.cellTemplatesMap = this.cellTemplates.reduce((acc, cur) => {
      acc[cur.name] = cur.template;
      return acc;
    }, {});
  }

  callGridRefresh(datatable?: Table) {
    if (this.dataTableInfo.datatable == null) {
      this.dataTableInfo.datatable = datatable;
    }

    this.dataTableInfo.gridRefreshMode = datatable != null ? GridRefreshMode.LazyLoad : GridRefreshMode.ExportExcel;

    if (this.deferred === false) {
      this.gridRefresh.emit(this.dataTableId);
    }
  }

  getColoring(col, rowData, colField) {
    if (this.coloring) {
      return this.coloring(col, rowData, colField);
    }
  }

  isDataExists(): boolean {
    return this.dataTableInfo.totalRecords > 0 ? true : false;
  }

  onRowSelect(value) {
    this.rowSelect.emit(value);
  }

  onRowUnselect(value) {
    this.rowUnselect.emit(value);
  }

  gridColumnsReorder(datatable: DatatableInfo) {
    const selectedColumns = datatable.selectedColumns;
    datatable.selectedColumns = [];

    for (let i = 0; i < datatable.cols.length; i++) {
      const column = datatable.cols[i];
      if (selectedColumns.findIndex(d => d.field === column.field) > -1) {
        datatable.selectedColumns.push(column);
      }
    }
  }

  printTable() {
    let defaultColumns = JSON.stringify(this.dataTableInfo.selectedColumns);
    for (let i = 0; i < this.dataTableInfo.selectedColumns.length; i++) {
      const column = this.dataTableInfo.selectedColumns[i];
      if (column.type == ColumnType.Operation) {
        this.dataTableInfo.selectedColumns.splice(i, 1);
        break;
      }
    }
    // setTimeout(() => {
    //   //burası düzeltilecek aynı sayfada birden fazla grid olduğunda düzgün çalışmıyor. değişiklik yapıldı şu anda hiç çalışmıyor
    //   let button = document.getElementById("printButton");
    //   button.setAttribute("printSectionId", this.dataTableId);
    //   button.setAttribute("ng-reflect-print-section-id", this.dataTableId);
    //   button.click();
    //   this.dataTableInfo.selectedColumns = JSON.parse(defaultColumns);
    // }, 1);
  }

  getDataTableInfo(){
    if(!this.ch.isNullOrUndefined(this.dataTableId) && this.ch.isNullOrUndefined(this.dataTableInfo)){
      this.dataTableInfo = this.ch.getDataTableInfo(this.dataTableId);
    }

    return this.dataTableInfo;
  }
}
