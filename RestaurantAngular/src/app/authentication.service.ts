import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Account } from './models/account';
//import { environment } from '../environments/environment';


@Injectable({
  providedIn: 'root'
})

export class AuthenticationService {
  //apiUrl: string = "https://cors-anywhere.herokuapp.com/https://cloudcipher-restrauntrecommendations.azurewebsites.net/api/";
  apiUrl: string = "https://cloudcipher-restrauntrecommendations.azurewebsites.net/api/";
  //apiUrl: string= "https://restaurantrecommendationsapi.azurewebsites.net/api/";
  //apiUrl: string = "http://localhost:58756/api/";
  mvcUrl: string = "https://cloudcipher-restaurantrecommendationsfrontend.azurewebsites.net/";


  constructor(private http: HttpClient) { }

  login(
    client: Account,
    pass = (data: Object) => { },
    fail = err => { }
  ) {
    this.http.post(
      this.apiUrl + 'account/login',
      client,
      { withCredentials: true }
    ).subscribe(
      data => {
        sessionStorage.setItem("AccountKey", JSON.stringify(client));
        pass(data);
        console.log(data);
        console.log( JSON.stringify(client));
        console.log(sessionStorage.getItem("AccountKey"));
      },
      fail
    );
    //potential code for getting logged into mvc as well
    // this.http.post(
    // this.mvcUrl + 'account/login',
    // client,
    // this.httpOptions
    // );
  }

  logout(pass = (data: Object) => { }, fail = err => { }): void {
    var client = JSON.parse(sessionStorage.getItem('AccountKey'));
    this.http.post(
      this.apiUrl + 'account/logout',
      client,
      //this.httpOptions
      { withCredentials: true }
    ).subscribe(pass, fail);
    sessionStorage.removeItem('AccountKey');
  }

}
