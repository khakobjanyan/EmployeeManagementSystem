import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ResetPassword} from "../models/reset-password.model";

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {

  private baseUrl: string = "https://localhost:7262/api/User/";
  constructor(private http: HttpClient) { }

  public sendResetPasswordLink(email: string){
      return this.http.post(`${this.baseUrl}send-reset-email/${email}`,{});
  }

  public resetPassword(resetPasswordObj: ResetPassword){
    return this.http.post(`${this.baseUrl}reset-password`, resetPasswordObj)
  }
}
