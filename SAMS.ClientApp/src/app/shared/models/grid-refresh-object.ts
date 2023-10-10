
import { GridRefreshMode } from '../enums/grid-refresh-mode';
import { Table } from 'primeng/table';

export class GridRefreshObject {
  datatable?: Table;
  gridRefreshMode: GridRefreshMode = GridRefreshMode.Search;
}
