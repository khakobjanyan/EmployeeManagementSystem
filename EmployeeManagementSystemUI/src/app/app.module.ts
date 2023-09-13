import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppComponent} from './app.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';
import {AppRoutingModule} from "./app-routing.module";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {EmplyeeListComponent} from './components/emplyee-list/emplyee-list.component';
import {NgToastModule} from "ng-angular-popup";
import {TokenInterceptor} from "./intercepters/token.interceptor";
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { CreateEmployeeComponent } from './components/create-employee/create-employee.component';
import { ViewEmployeeComponent } from './components/view-employee/view-employee.component';
import {MatInputModule} from "@angular/material/input";
import {MatTableModule} from "@angular/material/table";
import {MatPaginatorModule} from "@angular/material/paginator";

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    EmplyeeListComponent,
    ResetPasswordComponent,
    NavbarComponent,
    CreateEmployeeComponent,
    ViewEmployeeComponent
  ],
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        AppRoutingModule,
        HttpClientModule,
        NgToastModule,
        FormsModule,
        MatInputModule,
        MatTableModule,
        MatPaginatorModule
    ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
