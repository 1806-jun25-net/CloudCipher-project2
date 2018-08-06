import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from "@angular/router";

import { RestaurantListComponent } from './restaurant-list/restaurant-list.component';
import { RestaurantDetailComponent } from './restaurant-detail/restaurant-detail.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';

const appRoutes: Routes = [
  { path: '', redirectTo:'home', pathMatch: 'full' },
  { path: "home", component: HomeComponent },
  { path: "browse", component: RestaurantListComponent },
  { path: "browse/:searchText", component: RestaurantListComponent },
  { path: "details/:id", component: RestaurantDetailComponent },
  { path: "login", component: LoginComponent}
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(appRoutes)
  ],
  declarations: []
})
export class AppRoutingModule { }
