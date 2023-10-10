import { FormGroup } from '@angular/forms';
import { GridRefreshMode } from '../enums/grid-refresh-mode';
import { Table } from 'primeng/table';

export class DatatableInfo {
  id: string;
  index: number;
  url: string;
  datatable: Table;
  filterForm?: FormGroup;
  searchFilter?: Object;
  selectedColumns: any[];
  loading = true;
  cols: any[];
  totalRecords = 0;
  gridData: any;
  gridRefreshMode: GridRefreshMode = GridRefreshMode.Search;
  displayGrid = false;
  isCleared = false;
}
