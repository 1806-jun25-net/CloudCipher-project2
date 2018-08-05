import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { Router, } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  accountString: string;
  constructor(private authService: AuthenticationService,
              private router: Router) { }

  ngOnInit() {
    this.getAccountKey();
  }

  getAccountKey() : string
  {
    this.accountString = sessionStorage.getItem("AccountKey");
    return this.accountString;
  }

  logout()  {
    this.authService.logout(
      (result) => {
        console.log("log out successful");
        this.accountString = sessionStorage.getItem("AccountKey")
        this.router.navigateByUrl("/browse");
      },
      (result) => console.log("log out failure:"+ result)
    );
  }

}
