import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Account } from './models/account';
//import { environment } from '../environments/environment';


@Injectable({
  providedIn: 'root'
})

export class AuthenticationService {
  apiUrl: string = "https://cloudcipher-restrauntrecommendations.azurewebsites.net/api/";
  //apiUrl: string = "http://localhost:58756";
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
        client = <Account>data;
        sessionStorage.setItem("AccountKey", JSON.stringify(client));
        pass(data);
      },
      fail
    );
    // this.http.post(
    //   this.mvcUrl + 'account/login',
    //   client,
    //   { withCredentials: true }
    //);
  }
  
}
