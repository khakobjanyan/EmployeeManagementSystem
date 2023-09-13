import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthService} from "../services/auth.service";
import {NgToastService} from "ng-angular-popup";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toast = inject(NgToastService)
  if(authService.isLoggedIn()){
    return true;
  } else {
    toast.error({detail:"ERROR",summary:'Please log in', duration: 3000});
    router.navigate(["/login"]);
    return  false;
  }
};
