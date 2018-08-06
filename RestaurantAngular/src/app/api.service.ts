import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ApiService {
  //apiUrl: string = "https://cors-anywhere.herokuapp.com/https://cloudcipher-restrauntrecommendations.azurewebsites.net/api/";
  apiUrl: string = "https://cloudcipher-restrauntrecommendations.azurewebsites.net/api/";
  //apiUrl: string = "http://localhost:58756/api/";

  //Create an HttpClient property equal to parameter taken in
  constructor(private httpClient: HttpClient) { }

   //get all restaurants in db
  getRestaurants(
    success,
    failure
  ) {
    let url = this.apiUrl+"restaurant";
    let request = this.httpClient.get(url);
    let promise = request.toPromise();

    promise.then(success,failure);

  }

  //gets a single restaurant matching the id given
  getRestaurantById(
    rId: string,
    success,
    failure
  ) {
    let url = this.apiUrl+"restaurant/"+rId;
    let request = this.httpClient.get(url);
    let promise = request.toPromise();

    promise.then(success,failure);
  }

  //gets all restaurants matching the search string
  searchRestaurants(
    searchText: string,
    success,
    failure
  ) {
    console.log(sessionStorage.getItem("AccountKey"));
    let url = this.apiUrl+"BrowseRestaurant/"+searchText;
    let request = this.httpClient.get(url, { withCredentials: true});
    let promise = request.toPromise();

    promise.then(success, failure);
  }

  getBlacklistedForRestaurant(
    rId: string,
    success,
    failure
  ) {
    let url = this.apiUrl+"blacklistAnalytics/"+rId;
    //let request = this.httpClient.get(url);
    let request = this.httpClient.get(url);
    let promise = request.toPromise();

    promise.then(success, failure);
  }

  getFavoritedForRestaurant(
    rId: string,
    success,
    failure
  ) {
    let url = this.apiUrl+"favoritesAnalytics/"+rId;
    let request = this.httpClient.get(url);
    let promise = request.toPromise();

    promise.then(success, failure);
  }

  getIfRestaurantIsFavorite(
    rId: string,
    success,
    failure
  ) {
    let url = this.apiUrl+"favorites/"+rId;
    let request = this.httpClient.get(url, { withCredentials: true });
    let promise = request.toPromise();
    promise.then(success, failure);
  }

  getIfRestaurantIsBlacklisted(
    rId: string,
    success,
    failure
  ) {
    let url = this.apiUrl+"blacklist/"+rId;
    let request = this.httpClient.get(url, { withCredentials: true });
    let promise = request.toPromise();
    promise.then(success, failure);
  }

  getFavoritesForUser(
    success,
    failure
  ) {
    let url = this.apiUrl+"favorites";
    let request = this.httpClient.get(url, { withCredentials: true });
    let promise = request.toPromise();
    promise.then(success, failure);
  }

  getBlacklistedForUser(
    success,
    failure
  ) {
    let url = this.apiUrl+"blacklist";
    let request = this.httpClient.get(url, { withCredentials: true });
    let promise = request.toPromise();
    promise.then(success, failure);
  }

}
