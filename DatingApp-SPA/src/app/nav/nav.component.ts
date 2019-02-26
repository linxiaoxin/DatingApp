import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next =>{
      console.log('Logged in successfully');
    }, error => {
      console.log('Failed to login');
    });
  }
  loggedIn()  {
    const token = localStorage.getItem('token');
    // !! return a true or false value, short hand for if statement
    return !!token;
  }

  loggedOut() {
      localStorage.removeItem('token');
      console.log('Logged out');
  }
}