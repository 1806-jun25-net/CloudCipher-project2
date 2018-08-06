import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ApiService {
  //apiUrl: string = "https://cors-anywhere.herokuapp.com/https://cloudcipher-restrauntrecommendations.azurewebsites.net/api/";
  //apiUrl: string = "https://cloudcipher-restrauntrecommendations.azurewebsites.net/api/";
  //apiUrl: string= "https://restaurantrecommendationsapi.azurewebsites.net/api/";
  apiUrl: string = "http://localhost:58756/api/";

  //Create an HttpClient property equal to parameter taken in
  constructor(private httpClient: HttpClient) { }

  getRequestWithoutCredentials(
    url: string,
    success,
    failure
  ) {
    let request = this.httpClient.get(url);
    let promise = request.toPromise();
    promise.then(success,failure);
  }

  getRequestWithCredentials(
    url: string,
    success,
    failure
  ) {
    let request = this.httpClient.get(url, { withCredentials: true});
    let promise = request.toPromise();
    promise.then(success,failure);
  }

   //get all restaurants in db
  getRestaurants(
    success,
    failure
  ) {
    this.getRequestWithoutCredentials(this.apiUrl+"restaurant", success, failure);
  }

  //gets a single restaurant matching the id given
  getRestaurantById(
    rId: string,
    success,
    failure
  ) {
    this.getRequestWithoutCredentials(this.apiUrl+"restaurant/"+rId, success, failure);
  }

  //gets all restaurants matching the search string
  searchRestaurants(
    searchText: string,
    success,
    failure
  ) {
    this.getRequestWithCredentials(this.apiUrl+"BrowseRestaurant/"+searchText, success, failure);
  }

  getBlacklistedForRestaurant(
    rId: string,
    success,
    failure
  ) {
    this.getRequestWithoutCredentials(this.apiUrl+"blacklistAnalytics/"+rId, success, failure);
  }

  getFavoritedForRestaurant(
    rId: string,
    success,
    failure
  ) {
    this.getRequestWithoutCredentials(this.apiUrl+"favoritesAnalytics/"+rId, success, failure);
  }

  getIfRestaurantIsFavorite(
    rId: string,
    success,
    failure
  ) {
    this.getRequestWithCredentials(this.apiUrl+"favorites/"+rId, success, failure);
  }

  getIfRestaurantIsBlacklisted(
    rId: string,
    success,
    failure
  ) {
    this.getRequestWithCredentials(this.apiUrl+"blacklist/"+rId, success, failure);
  }

  getFavoritesForUser(
    success,
    failure
  ) {
    this.getRequestWithCredentials(this.apiUrl+"favorites", success, failure);
  }

  getBlacklistedForUser(
    success,
    failure
  ) {
    this.getRequestWithCredentials(this.apiUrl+"blacklist", success, failure);
  }

}
