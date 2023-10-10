import { Roles } from "./shared/enums/role";

export class MenuRoutes {
    studentRoutes: any = [{ label: 'Anasayfa', icon: 'pi pi-fw pi-home', routerLink: ['/'] }];
    otherRoutes: any = [
        {
            label: 'Menü', icon: 'pi pi-fw pi-home',
            items: [
                { label: 'Anasayfa', icon: 'pi pi-fw pi-home', routerLink: ['/'] },
                {
                    label: 'Kullanıcı Yönetimi', icon: 'pi pi-fw pi-users', routerLink: ['/user'], roles: [Roles.Admin],
                    items: [
                        { label: 'Kullanıcı Ekle', icon: 'pi pi-fw pi-user-plus', routerLink: ['/user/add-user'] },
                        { label: 'Kullanıcı Listele', icon: 'pi pi-fw pi-users', routerLink: ['/user/list-user'] }
                    ]
                },
            ]
        }
    ];
}