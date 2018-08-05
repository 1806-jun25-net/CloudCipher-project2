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
  }

  goBack(): void {
    this.location.back();
  }

}
