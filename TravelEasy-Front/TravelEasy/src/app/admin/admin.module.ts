import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProductsComponent } from './products/products.component';
import { OrdersComponent } from './orders/orders.component';
import { UsersComponent } from './users/users.component';
import { CreateProductComponent } from './create-product/create-product.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';


@NgModule({
  declarations: [
    AdminComponent,
    AdminLayoutComponent,
    DashboardComponent,
    ProductsComponent,
    OrdersComponent,
    UsersComponent,
    CreateProductComponent,
    ProductDetailComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule.forRoot()
  ]
})
export class AdminModule { }
