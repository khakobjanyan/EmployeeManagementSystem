import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {Observable} from "rxjs";
import {JwtHelperService} from "@auth0/angular-jwt";
import {TokenApiModel} from "../models/token-api.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string = "https://localhost:7262/api/User/"
  constructor(private http: HttpClient, private router: Router) { }


  public register(userObj: any){
      return this.http.post<any>(`${this.baseUrl}register`, userObj)
  }

  public logeIn(loginOgj: any){
    return this.http.post<any>(`${this.baseUrl}authenticate`, loginOgj)
  }

  public storeAccessToken(accessTokenValue: string){
    localStorage.setItem("accessToken", accessTokenValue)
  }

  public storeRefreshToken(refreshTokenValue: string){
    localStorage.setItem("refreshToken", refreshTokenValue)
  }
  public getAccessToken() : string{
      return localStorage.getItem("accessToken") || "";
  }

  public getRefreshToken() : string{
    return localStorage.getItem("refreshToken") || "";
  }
  public isLoggedIn(){
    return !!localStorage.getItem("accessToken")
  }

  public signOut(){
    localStorage.removeItem("accessToken");
    this.router.navigate(["/login"])
  }

  private decodedAccessToken(){
    const jwtHelper = new JwtHelperService();
    return jwtHelper.decodeToken(this.getAccessToken()!)
  }

  public getNameFromAccessToken(){
    if(this.decodedAccessToken())
      return this.decodedAccessToken().unique_name;
  }

  public getRoleFromAccessToken(){
    if(this.decodedAccessToken())
      return this.decodedAccessToken().role;
  }

  public refreshToken(token: TokenApiModel): Observable<TokenApiModel>{
    return this.http.post(`${this.baseUrl}refresh`, token) as Observable<TokenApiModel>
  }
}
