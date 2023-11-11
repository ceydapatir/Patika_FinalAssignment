import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// CoreUI Modules
import {
  CardModule,
  DropdownModule,
  AccordionModule,
  FormModule,
  CarouselModule,
  CollapseModule,
  GridModule,
  ButtonModule,
  BadgeModule,
  BreadcrumbModule,
  ListGroupModule,
  NavModule,
  PaginationModule,
  PlaceholderModule,
  PopoverModule,
  ProgressModule,
  SharedModule,
  SpinnerModule,
  TableModule,
  TabsModule,
  TooltipModule,
  UtilitiesModule
} from '@coreui/angular';

import { IconModule } from '@coreui/icons-angular';

// utils
import { DocsComponentsModule } from '@docs-components/docs-components.module';

// views
import { CompanyProfilesComponent } from './company-profiles/company-profiles.component';
import { DealersComponent } from './dealers/dealers.component';
import { ProductsComponent } from './products/products.component';
import { OrdersComponent } from './orders/orders.component';
import { OrderdetailsComponent } from './orderdetails/orderdetails.component';
import { MessagesComponent } from './messages/messages.component';
import { StocksComponent } from './stocks/stocks.component';
import { CartsComponent } from './carts/carts.component';
import { SupplierComponent } from './supplier/supplier.component';
import { ProfilesComponent } from './profiles/profiles.component';

// Components Routing
import { BaseRoutingModule } from './base-routing.module';
import { EmployeesComponent } from '../base/employees/employees.component';

@NgModule({
  imports: [
    CommonModule,
    BaseRoutingModule,
    AccordionModule,
    BadgeModule,
    BreadcrumbModule,
    ButtonModule,
    CardModule,
    CollapseModule,
    GridModule,
    UtilitiesModule,
    SharedModule,
    ListGroupModule,
    IconModule,
    ListGroupModule,
    PlaceholderModule,
    ProgressModule,
    SpinnerModule,
    TabsModule,
    NavModule,
    TooltipModule,
    CarouselModule,
    FormModule,
    ReactiveFormsModule,
    DropdownModule,
    PaginationModule,
    PopoverModule,
    TableModule,
    DocsComponentsModule,
    FormsModule
  ],
  declarations: [
    DealersComponent,
    ProductsComponent,
    OrdersComponent,
    MessagesComponent,
    StocksComponent,
    CartsComponent,
    CompanyProfilesComponent,
    EmployeesComponent,
    SupplierComponent,
    OrderdetailsComponent,
    ProfilesComponent
  ],
})
export class BaseModule {}
