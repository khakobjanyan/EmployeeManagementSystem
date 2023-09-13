import {Injectable} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import {catchError, Observable, switchMap, throwError} from 'rxjs';
import {AuthService} from "../services/auth.service";
import {Router} from "@angular/router";
import {TokenApiModel} from "../models/token-api.model";

@Injectable({
  providedIn: 'root',
})
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService, private router: Router) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.auth.getAccessToken();
      const headers = request.headers.set('Authorization', `Bearer ${token}`);
      return next.handle(request.clone({headers})).pipe(
        catchError(err => {
          if (err instanceof HttpErrorResponse && err.status === 401) {
            return this.handleUnAuthorisedError(request, next);
          }
            return throwError(err)
        })
      );
  }

  public handleUnAuthorisedError(req: HttpRequest<any>, next: HttpHandler) {
    let tokenApi = new TokenApiModel();
    tokenApi.accessToken = this.auth.getAccessToken();
    tokenApi.refreshToken = this.auth.getRefreshToken();
    return this.auth.refreshToken(tokenApi).pipe(
      switchMap(data => {
          this.auth.storeAccessToken(data.accessToken);
          this.auth.storeRefreshToken(data.refreshToken);
          const headers = req.headers.set('Authorization', `Bearer ${data.accessToken}`);
          return next.handle(req.clone({headers}));
        }),
      catchError(err => {
          this.auth.signOut();
          this.router.navigate(["/login"]);
          return throwError(err)
      })
    );
  }

}
