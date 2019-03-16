import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-roleModal',
  templateUrl: './roleModal.component.html',
  styleUrls: ['./roleModal.component.css']
})
export class RoleModalComponent implements OnInit {
  @Output() updateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[];

  constructor(public bsModalRef: BsModalRef) {}

  ngOnInit() {
  }

  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }
}
