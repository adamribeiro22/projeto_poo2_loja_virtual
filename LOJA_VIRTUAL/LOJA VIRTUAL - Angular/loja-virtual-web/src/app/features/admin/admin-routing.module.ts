import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { OrderListComponent } from './components/order-list/order-list.component';
import { ProductManagementComponent } from './components/product-management/product-management.component';
import { OrderDetailComponent } from './components/order-detail/order-detail.component';
import { ProductListComponent } from './components/product-list/product-list.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      { 
        path: '', 
        redirectTo: 'pedidos', 
        pathMatch: 'full'
      },
      { 
        path: 'pedidos', 
        component: OrderListComponent
      },
      { 
        path: 'pedidos/:id', 
        component: OrderDetailComponent
      },
      { 
        path: 'produtos',
        component: ProductListComponent 
      },
      { 
        path: 'produtos/novo',
        component: ProductManagementComponent 
      },
      { 
        path: 'produtos/editar/:id',
        component: ProductManagementComponent 
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }