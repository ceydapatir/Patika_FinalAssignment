import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PagesRoutingModule } from './pages-routing.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { Page404Component } from './page404/page404.component';
import { Page500Component } from './page500/page500.component';
import { AccordionModule, BadgeModule, BreadcrumbModule, ButtonModule, CardModule, CarouselModule, CollapseModule, DropdownModule, FormModule, GridModule, ListGroupModule, NavModule, PaginationModule, PlaceholderModule, PopoverModule, ProgressModule, SharedModule, SpinnerModule, TableModule, TabsModule, TooltipModule, UtilitiesModule } from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { HttpClientModule } from '@angular/common/http';
import { CompanyService } from 'src/app/services/company.service';
import { AddressService } from 'src/app/services/address.service';
import { ProductService } from 'src/app/services/product.service';
import { CardService } from 'src/app/services/card.service';
import { DocsComponentsModule } from '@docs-components/docs-components.module';
import { BaseRoutingModule } from '../base/base-routing.module';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    Page404Component,
    Page500Component
  ],
  imports: [
    PagesRoutingModule,
    HttpClientModule,
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
  ],
  providers: [
    AuthService,
    CompanyService,
    AddressService,
    EmployeeService,
    ProductService,
    CardService
  ]
})
export class PagesModule {
}
