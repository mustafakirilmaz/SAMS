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
                {
                    label: 'Tanımlamalar', icon: 'pi pi-fw pi-users', routerLink: ['/definitions'], roles: [Roles.Admin],
                    items: [
                        //{ label: 'Site Ekle', icon: 'pi pi-fw pi-plus-circle', routerLink: ['/site/add-site'] },
                        { label: 'Site Listele', icon: 'pi pi-fw pi-home', routerLink: ['/site/list-site'] },
                        //{ label: 'Bina Ekle', icon: 'pi pi-fw pi-plus-circle', routerLink: ['/building/add-building'] },
                        { label: 'Bina Listele', icon: 'pi pi-fw pi-th-large', routerLink: ['/building/list-building'] },
                        //{ label: 'Bağımsız Bölüm Ekle', icon: 'pi pi-fw pi-plus-circle', routerLink: ['/unit/add-unit'] },
                        { label: 'Bağımsız Bölüm Listele', icon: 'pi pi-fw pi-home', routerLink: ['/unit/list-unit'] },
                    ]
                },
                {
                    label: 'İşletme Projesi', icon: 'pi pi-fw pi-book', roles: [Roles.Admin],
                    items: [
                        { label: 'Başvuru', icon: 'pi pi-fw pi-book', routerLink: ['/business-project/detail'] },
                        // { label: 'Upload Documents', icon: 'pi pi-fw pi-upload', routerLink: ['/pica-form/documents'] },
                    ]
                },
            ]
        }
    ];
}