import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { Router, } from '@angular/router';

import { Account } from '../models/account';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  account: Account = new Account;

  constructor(private authService: AuthenticationService,
              private router: Router) { }

  ngOnInit() {
  }

  login () {
    this.authService.login(
      this.account,
      (result) => {
        console.log("log in successful");
        this.router.navigateByUrl("/browse");
      },
      (result) => console.log("log in failure:"+ result)
    );
  }
}