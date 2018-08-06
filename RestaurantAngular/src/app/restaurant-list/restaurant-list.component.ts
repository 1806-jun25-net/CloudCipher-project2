import { Component, OnInit } from '@angular/core';
import { Restaurant } from '../models/restaurant';
import { ApiService} from '../api.service'

@Component({
  selector: 'app-restaurant-list',
  templateUrl: './restaurant-list.component.html',
  styleUrls: ['./restaurant-list.component.css']
})
export class RestaurantListComponent implements OnInit {
  restaurants: Restaurant[] = []; //all restaurants received back from search
  searchText: string;
  fixedText: string;
  start: number = 0;  // 0 based index for what number to give each result in ol
  currentResultsPage: number = 1;  //for only displaying 4 restaurants per page
  resultsPerPage: number = 4; //how many restraunts to display per page
  maxPages: number = 0;
  restaurantsSubset: Restaurant[] = []; //the current set of restraunts to put on page
  emptyResults: boolean = false;

  //take in parameter and store in property
  constructor(private api: ApiService) { }

  //user for view setup
  ngOnInit() {
    this.searchRestaurants();
  }

  //for now, only searches by first keyword if any are given
  searchRestaurants() {
    this.currentResultsPage = 1;
    this.start = 0;
    this.fixedText = this.searchText;
    if (!this.searchText)
    {
      this.emptyResults = false;
    }
    else
    {
      var fn= (result) => {
        console.log("successfully retrieved");
            this.restaurants = result;
            this.produceRestaurantsSubset();
            this.maxPages = Math.ceil(this.restaurants.length/4);
            this.emptyResults = true;
      };
      if (this.searchText === "") {
        this.api.getRestaurants(
          fn,
          (result) => console.log("failure")
        );
      } else {
        this.api.searchRestaurants(this.searchText,
          fn,
          (result) => console.log("failure")
        );
      }
    }
    
  }

  produceRestaurantsSubset(): void {
    this.start = (this.currentResultsPage-1)*this.resultsPerPage;
    this.restaurantsSubset = [];
    for (var i = 0; i+this.start < this.restaurants.length && i<this.resultsPerPage; i++)
    {
      this.restaurantsSubset[i] = this.restaurants[this.start+i];
    }
  }

  changeCurrentResultsPage(x): void {
    if (Number.isInteger(x) && x>0 && x<=this.maxPages)
    {
      this.currentResultsPage = x;
      this.produceRestaurantsSubset();
    }
  }

  goToPage1(): void   {
    this.changeCurrentResultsPage(1);
  }

  goToPrevPage(): void   {
    this.changeCurrentResultsPage(this.currentResultsPage-1);
  }

  goToNextPage(): void {
    this.changeCurrentResultsPage(this.currentResultsPage+1);
  }

  goToLastPage(): void {
    this.changeCurrentResultsPage(this.maxPages);
  }

  changeResultsPerPage(x): void {
    if (x.isInteger() && x>0)
    {
      this.resultsPerPage = x;
      this.maxPages = Math.ceil(this.restaurants.length/4);
      this.produceRestaurantsSubset();
    }
  }
}
