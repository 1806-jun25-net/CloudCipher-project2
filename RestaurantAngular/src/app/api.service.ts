import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
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
    //let request = this.httpClient.get(url, { withCredentials: true });
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
    //let request = this.httpClient.get(url, { withCredentials: true });
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
    //let request = this.httpClient.get(url);
    //let request = this.httpClient.get(url, this.httpOptions);
    let request = this.httpClient.get(url,{ withCredentials: true});
    let promise = request.toPromise();

    promise.then(success, failure);
  }

  getBlacklistedForRestaurant(
    rId: string,
    success,
    failure
  ) {
    let url = this.apiUrl+"blacklistAnalytics/"+rId;
    let request = this.httpClient.get(url);
    //let request = this.httpClient.get(url, { withCredentials: true });
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
    //let request = this.httpClient.get(url, { withCredentials: true });
    let promise = request.toPromise();

    promise.then(success, failure);
  }

  //for testing that I'm actually capable of calling any api
  //TODO:  remove eventually once everything's working
  getPokemon(
    searchText: string,
    success,
    failure
  ) {
    let url = "https://pokeapi.co/api/v2/pokemon/" + searchText;
    let request = this.httpClient.get(url);
    //let request = this.httpClient.get(url, { withCredentials: true });
    let promise = request.toPromise();

    promise.then(success,failure);
  }

}
