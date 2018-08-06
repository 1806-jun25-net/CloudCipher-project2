import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '../../../node_modules/@angular/router';
import { Location } from '@angular/common';

import { Restaurant } from '../models/restaurant';
import { ApiService} from '../api.service'
import { jsonpCallbackContext } from '../../../node_modules/@angular/common/http/src/module';

@Component({
  selector: 'app-restaurant-detail',
  templateUrl: './restaurant-detail.component.html',
  styleUrls: ['./restaurant-detail.component.css']
})
export class RestaurantDetailComponent implements OnInit {
  //@Input() rId: string;
  restaurant: Restaurant;
  favCount: number;
  blackCount: number;
  isFav: boolean;
  isBlack: boolean;
  favStatus: string;
  blackStatus: string;
  
  constructor(
    private route: ActivatedRoute,
    private api: ApiService,
    private location: Location) { }

  ngOnInit() {
    console.log("starting details init");
    let rId = this.route.snapshot.paramMap.get("id");
    this.api.getRestaurantById(rId,
      (result) => {
        console.log("successfully retrieved restaurant id:"+rId);
        this.restaurant = result;
      },
      (result) => console.log("failure to retrieve restaurant id:"+rId)
    );
    this.api.getFavoritedForRestaurant(rId,
      (result) => {
        console.log("successfully retrieved favorited list");
        console.log(JSON.stringify(result));
        let uList = result
        this.favCount = uList.length;
      },
      (result) => console.log("failure to retrieve favorited list")
    );
    this.api.getBlacklistedForRestaurant(rId,
      (result) => {
        console.log("successfully retrieved blacklisted list");
        console.log(JSON.stringify(result));
        let uList = result
        this.blackCount = uList.length;
      },
      (result) => console.log("failure to retrieve blacklisted list")
    );
    this.api.getIfRestaurantIsFavorite(rId,
      (result) => {
        console.log("successfully checked if favorited");
        console.log(JSON.stringify(result));
        this.isFav = result;
      },
      (result) => console.log("failure to check if favorited")
    )
    this.api.getIfRestaurantIsBlacklisted(rId,
      (result) => {
        console.log("successfully checked if blacklisted");
        console.log(JSON.stringify(result));
        this.isBlack = result;
      },
      (result) => console.log("failure to check if blacklisted")
    )
    this.setBlackStatus();
    this.setFavStatus();
  }

  setBlackStatus()
  {
    if (this.isBlack)
    {
      this.blackStatus = "Remove From Blacklist";
    }
    else
    {
      this.blackStatus = "Add To Blacklist";
    }
  }

  setFavStatus()
  {
    if (this.isFav)
    {
      this.favStatus = "Remove From Favorites";
    }
    else
    {
      this.favStatus = "Add To Favorites";
    }
  }

  goBack(): void {
    this.location.back();
  }

  addToFavs(): void {
    this.api.addToFavorites(this.restaurant.id,
      (result) => {
        this.isFav=true;
        console.log("successfully added to favorites");
        this.setFavStatus();
      },
      (result) => {
        console.log("failed to add to favorites");
        this.setFavStatus();
      }
    )
  }

  addToBlack(): void {
    this.api.addToBlacklist(this.restaurant.id,
      (result) => {
        this.isBlack=true;
        console.log("successfully added to blacklist");
        this.setBlackStatus();
      },
      (result) => {
        console.log("failed to add to blacklist");
        this.setBlackStatus();
      }
    )
  }

  removeFromFavs(): void {
    this.api.removeFromFavorites(this.restaurant.id,
      (result) => {
        this.isFav=false;
        console.log("successfully removed from favorites");
        this.setFavStatus();
      },
      (result) => {
        console.log("failed to removed from favorites");
        this.setFavStatus();
      }
    )
  }

  removeFromBlack(): void {
    this.api.removeFromBlacklist(this.restaurant.id,
      (result) => {
        this.isFav=false;
        console.log("successfully removed from blacklist");
        this.setBlackStatus();
      },
      (result) => {
        console.log("failed to removed from blacklist");
        this.setBlackStatus();
      }
    )
  }

  toggleFavs(): void {
    this.favStatus = "Working...";
    if (!this.isFav)
    {
      this.addToFavs();
    }
    else 
    {
      this.removeFromFavs();
    }
  }

  toggleBlack(): void {
    this.blackStatus = "Working...";
    if (!this.isBlack)
    {
      this.addToBlack();
    }
    else 
    {
      this.removeFromBlack();
    }
  }


}
