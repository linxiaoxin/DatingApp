import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';
import { AdminService } from 'src/app/_services/admin.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { RoleModalComponent } from '../roleModal/roleModal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: User[];
  bsModalRef: BsModalRef;

  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit() {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles()
      .subscribe((users: User[]) =>
        {
          this.users = users;
        }, error => {
          console.log(error);
        }
        );
  }

  editRolesModal(user: User) {
    const initialState = {
      user,
      roles : this.getUserRoles(user)
    };
    this.bsModalRef = this.modalService.show(RoleModalComponent, {initialState});
    this.bsModalRef.content.updateSelectedRoles.subscribe(values =>{
      const rolesToUpdate = {
        rolesName: [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if(rolesToUpdate) {
        this.adminService.updateUserRoles(user, rolesToUpdate).subscribe(() => {
          user.roles = [...rolesToUpdate.rolesName];
        }, error => {
          console.log(error);
        });
      }
    });
  }

  getUserRoles(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
        {name: 'Admin', value: 'Admin'},
        {name: 'Moderator', value: 'Moderator'},
        {name: 'Member', value: 'Member'},
        {name: 'VIP', value: 'VIP'},
    ];

    for (let i = 0; i < availableRoles.length; i++) {
      let isMatch = false;
      for (let j = 0; j < userRoles.length; j++) {
        if (userRoles[j] === availableRoles[i].name) {
          isMatch = true;
          availableRoles[i].checked = true;
          roles.push(availableRoles[i]);
          break;
        }
      }
      if (!isMatch) {
        isMatch = false;
        availableRoles[i].checked = false;
        roles.push(availableRoles[i]);
      }
    }
    return roles;
  }
}
