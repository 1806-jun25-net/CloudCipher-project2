import { Component, OnInit } from '@angular/core';
import { Restaurant } from '../models/restaurant';
import { ApiService} from '../api.service'

@Component({
  selector: 'app-restaurant-list',
  templateUrl: './restaurant-list.component.html',
  styleUrls: ['./restaurant-list.component.css']
})
export class RestaurantListComponent implements OnInit {
  restaurants: Restaurant[] = [];
  searchText: string = "";
  currentResultsPage: number = 1;  //for only displaying 4 restaurants per page
  restaurantsSubset: Restaurant[] = [];
  //take in parameter and store in property
  constructor(private api: ApiService) { }

  //user for view setup
  ngOnInit() {
  }

  //for now, only searches by first keyword if any are given
  searchRestaurants() {
    this.currentResultsPage = 0;
    if (this.searchText === "") {
      this.api.getRestaurants(
        (result) => {
          console.log("successfully retrieved all restaurants");
          this.restaurants = result;
        },
        (res) => console.log("failure")
      );
    } else {
      this.api.getRestaurantsByKeyword(this.searchText,
        (result) => {
          console.log("successfully retrieved restaurants by keyword");
          this.restaurants = result;
        },
        (res) => console.log("failure")
      );
    }
  }

  getRestaurantsSubset() {
    
  }
}
