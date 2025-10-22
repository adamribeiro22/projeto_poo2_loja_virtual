import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { ShopRoutingModule } from './shop-routing.module';
import { ShopComponent } from './shop.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { CartComponent } from './components/cart/cart.component';

import { ProductCardComponent } from './components/product-card/product-card.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    ShopComponent,
    ProductListComponent,
    CartComponent,
    ProductCardComponent
  ],
  imports: [
    CommonModule,
    ShopRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class ShopModule { }