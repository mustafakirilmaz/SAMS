export interface MenuObject {
  menuItemName: string;
  route: string;
  moduleRowGuid: string;
  moduleName: string;
  moduleIcon: string;
  parentModuleRowGuid: string;
  subMenuItems: MenuObject[];
}
