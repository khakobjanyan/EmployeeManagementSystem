import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {RegisterComponent} from "./components/register/register.component";
import {EmplyeeListComponent} from "./components/emplyee-list/emplyee-list.component";
import {authGuard} from "./guards/auth.guard";
import {ResetPasswordComponent} from "./components/reset-password/reset-password.component";
import {CreateEmployeeComponent} from "./components/create-employee/create-employee.component";
import {ViewEmployeeComponent} from "./components/view-employee/view-employee.component";

const routes = [

  {
    path: "login",
    component: LoginComponent,
  },
  {
    path: "register",
    component: RegisterComponent
  },
  {
    path: "employees",
    component: EmplyeeListComponent,
    canActivate: [authGuard],
  },
  {
    path: "add-employee",
    component: CreateEmployeeComponent,
    canActivate: [authGuard],
  },
  {
    path: "view-employee/:id",
    component: ViewEmployeeComponent,
    canActivate: [authGuard],
  },
  {
    path: 'add-employee/:id',
    component: CreateEmployeeComponent,
    canActivate: [authGuard]
  },
  {
    path: "reset",
    component: ResetPasswordComponent
  },
  {
    path: "**",
    redirectTo: "/login"
  }

];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
