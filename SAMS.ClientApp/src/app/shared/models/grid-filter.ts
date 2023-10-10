import { IdNamePair } from './id-name-pair';

export class GridFilter {
  pageFirstIndex: number;
  sortBy: string;
  isSortAscending: boolean;
  pageSize: number;
  customProperties: IdNamePair[];
}
