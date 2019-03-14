import { Component, OnInit } from '@angular/core';
import { User } from '../_models/User';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from '../_models/Pagination';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/User.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParam: string;

  constructor(private authService: AuthService, private route: ActivatedRoute, private alertify: AlertifyService, private userService: UserService) { }

  ngOnInit() {
    this.route.data.subscribe(data =>{
      this.users = data.user.result;
      this.pagination = data.user.pagination;
    });
    this.likesParam = 'Likers';
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }
  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.likesParam)
    .subscribe((res: PaginatedResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }
    , error => {
      this.alertify.error(error);
    });
  }
}
