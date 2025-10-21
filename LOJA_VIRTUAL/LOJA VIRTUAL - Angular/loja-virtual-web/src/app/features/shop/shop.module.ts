import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShopRoutingModule } from './shop-routing.module';
import { ShopComponent } from './shop.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { CartComponent } from './components/cart/cart.component';


@NgModule({
  declarations: [
    ShopComponent,
    ProductListComponent,
    ProductCardComponent,
    CartComponent
  ],
  imports: [
    CommonModule,
    ShopRoutingModule
  ]
})
export class ShopModule { }
