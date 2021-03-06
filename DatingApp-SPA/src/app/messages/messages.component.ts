import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/Message';
import { Pagination, PaginatedResult } from '../_models/Pagination';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/User.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer = 'unread';

  constructor(private route: ActivatedRoute, private authService: AuthService
    ,         private alertify: AlertifyService, private userService: UserService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data.messages.result;
      this.pagination = data.messages.pagination;
    });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }
  loadMessages() {
    this.userService.getMessages(this.authService.decodedToken.nameid, this.pagination.currentPage
      , this.pagination.itemsPerPage, this.messageContainer).subscribe((res: PaginatedResult<Message[]>) => {
      this.messages = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }
  deleteMessage(msgId: number) {
    this.alertify.confirm('Are you sure you want to delete this message', () => {
      this.userService.deleteMessage(msgId, this.authService.decodedToken.nameid)
        .subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === msgId), 1);
          this.alertify.success('Message has been deleted');
        }, error => {
          this.alertify.error(error);
        })
    })
  }
}
