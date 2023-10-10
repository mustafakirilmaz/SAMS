export enum GridRefreshMode {
  /**
  * Paging, ordering, page size değiştirme ve gridin ilk yüklenmesi için kullanılır.
  */
  LazyLoad = 1,
  /**
  * Sayfada arama butonuna tıklandığında kullanılır.
  */
  Search = 2,
  /**
  * Excele aktarma işlemi için kullanılır.
  */
  ExportExcel = 3
}
