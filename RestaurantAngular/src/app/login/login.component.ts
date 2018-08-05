import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';

import { Account } from '../models/account';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  account: Account;

  constructor(private authService: AuthenticationService) { }

  ngOnInit() {
  }

  login () {
    this.authService.login(
      this.account,
      (result) => {
        console.log("log in successful");
      },
      (result) => console.log("log in failure:"+ result)
    );
  }
}