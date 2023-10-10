import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { UserService } from 'src/app/services/user-service';
import { ColumnType } from 'src/app/shared/enums/column-type';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent extends BaseComponent implements OnInit {
  gridName = 'userGrid';
  selectedUser: any;
  formGridColumns = [
    ['name', 'Ad'],
    ['surname', 'Soyad'],
    ['email', 'Eposta'],
    ['operations', 'İşlem', ColumnType.Operation]
  ];

  constructor(public userService: UserService, private router: Router) { super(); }

  ngOnInit() {
    this.ch.clearComponent(this.gridName);
    this.createFilterForm();
    this.ch.createColumns(this.formGridColumns, this.gridName);
  }

  createFilterForm() {
    this.ch.setFilterForm(this.ch.formBuilder.group({
      name: [''],
      surname: [''],
      email: [''],
    }), this.gridName);
  }

  gridRefresh() {
    this.ch.beforeGridRefresh(this.gridName);
    this.userService.getUserForGrid(this.gridName).subscribe(result => {
      this.ch.gridDatabind(result, this.gridName);
    });
  }

  customClear() {
    this.ch.getFilterForm(this.gridName).reset();
    this.gridRefresh();
  }

  deleteUser(userId: number) {
    this.ch.messageHelper.deleteConfirm(() => {
      this.userService.deleteUser(userId).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.goLastPage(this.gridName, null, true);
          this.gridRefresh();
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
        }
      });
    });
  }

  goUserDetail(user) {
    const queryParams = this.ch.urlEncrypt({ id: user.id });
    this.router.navigate(['/user/edit-user'], { queryParams: queryParams });
  }

  setSelectedUser(user: any) {
    this.selectedUser = user;
  }
}
