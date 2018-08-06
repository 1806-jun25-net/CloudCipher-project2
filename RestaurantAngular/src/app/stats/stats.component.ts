import { Component, OnInit } from '@angular/core';

import { FrequencyModel } from '../models/frequencyModel';
import { Restaurant } from '../models/restaurant';
import { ApiService} from '../api.service'
import { RestaurantFrequency } from '../models/restaurantFrequency';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.css']
})
export class StatsComponent implements OnInit {
  keywords : FrequencyModel[];
  topKeywords : FrequencyModel[] = [];
  favorites : RestaurantFrequency[];
  topFavorites : RestaurantFrequency[] = [];
  blackls : RestaurantFrequency[];
  topBlacklisted : RestaurantFrequency[] = [];
  queried : RestaurantFrequency[];
  topQueried : RestaurantFrequency[] = [];
  view : number = 1;

  constructor(private api: ApiService) { }

  ngOnInit() {
    console.log("starting stats init");
    this.api.getMostSearchedKeywords(
      (result) => {
        console.log("successfully retrieved top keywords");
        this.keywords = result;
        for (let i = 0; i < 10&& i <this.keywords.length; i++)
        {
          this.topKeywords[i] = this.keywords[i];
        }
      },
      (result) => ("failed to retrieve top keywords")
    );
    
    this.api.getMostFavoritedRestaurants(
      (result) => {
        console.log("successfully retrieved top fav restaurants");
        this.favorites = result;
        for (let i = 0; i < 5&& i <this.favorites.length; i++)
        {
          this.topFavorites[i] = this.favorites[i];
        }
      },
      (result) => ("failed to retrieve top fav restaurants")
    );

    this.api.getMostBlacklistedRestaurants(
      (result) => {
        console.log("successfully retrieved top blackl restaurants");
        this.blackls = result;
        for (let i = 0; i < 5&& i <this.blackls.length; i++)
        {
          this.topBlacklisted[i] = this.blackls[i];
        }
      },
      (result) => ("failed to retrieve top blackl restaurants")
    );

    this.api.getMostQueriedRestaurants(
      (result) => {
        console.log("successfully retrieved top queried restaurants");
        this.queried = result;
        for (let i = 0; i < 5&& i <this.queried.length; i++)
        {
          this.topQueried[i] = this.queried[i];
        }
      },
      (result) => ("failed to retrieve top queried restaurants")
    );

  }

  
  changeView( x : number)
  {
    this.view = x;
  }


}
