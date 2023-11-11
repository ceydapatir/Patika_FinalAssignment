import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DealersComponent } from './dealers/dealers.component';
import { ProductsComponent } from './products/products.component';
import { OrdersComponent } from './orders/orders.component';
import { OrderdetailsComponent } from './orderdetails/orderdetails.component';
import { MessagesComponent } from './messages/messages.component';
import { StocksComponent } from './stocks/stocks.component';
import { CompanyProfilesComponent } from './company-profiles/company-profiles.component';
import { CartsComponent } from './carts/carts.component';
import { EmployeesComponent } from '../base/employees/employees.component';
import { SupplierComponent } from './supplier/supplier.component';
import { ProfilesComponent } from './profiles/profiles.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'company-profile',
        component: CompanyProfilesComponent,
        data: {
          title: 'Company Profile',
        },
      },
      {
        path: 'supplier',
        component: SupplierComponent,
        data: {
          title: 'Supplier',
        },
      },
      {
        path: 'dealers',
        component: DealersComponent,
        data: {
          title: 'Dealers',
        },
      },
      {
        path: 'products',
        component: ProductsComponent,
        data: {
          title: 'Products',
        },
      },
      {
        path: 'profile',
        component: ProfilesComponent,
        data: {
          title: 'Profile',
        },
      },
      {
        path: 'orders/:id/orderdetails',
        component: OrderdetailsComponent,
        data: {
          title: 'OrderDetails',
        },
      },
      {
        path: 'orders',
        component: OrdersComponent,
        data: {
          title: 'Orders',
        },
      },
      {
        path: 'messages',
        component: MessagesComponent,
        data: {
          title: 'Messages',
        },
      },
      {
        path: 'stocks',
        component: StocksComponent,
        data: {
          title: 'Stocks',
        },
      },
      {
        path: 'cart',
        component: CartsComponent,
        data: {
          title: 'Cart',
        },
      },
      {
        path: ":companyType/:id/employees",
        component: EmployeesComponent,
        data: {
          title: 'Employee'
        }
      },
      {
        path: "employees",
        component: EmployeesComponent,
        data: {
          title: 'Employee'
        }
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BaseRoutingModule {}

