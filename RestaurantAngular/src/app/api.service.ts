import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  apiUrl: string = "https://cloudcipher-restrauntrecommendations.azurewebsites.net/api/";

  //Create an HttpClient property equal to parameter taken in
  constructor(private httpClient: HttpClient) { }

  //get a specific restaurant by its id
  getRestaurant(
    rId: string, //Id of restaurant to get details for
    success,
    failure
  ) {
    let url = this.apiUrl+"restaurant/" + rId;
    let request = this.httpClient.get(url, { withCredentials: true });

    let promise = request.toPromise();

    promise.then(success,failure);
  }

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
  //for now, only checks the first keyword given
  getRestaurantsByKeyword(
    searchText: string,
    success,
    failure
  ) {
    let keyword: string[] = searchText.split(" ");
    let url = this.apiUrl+"keyword/"+keyword[0];
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
