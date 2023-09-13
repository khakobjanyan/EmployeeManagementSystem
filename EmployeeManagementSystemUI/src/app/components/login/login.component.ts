import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import ValidateForm from "../../helpers/validateForm";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {NgToastService} from "ng-angular-popup";
import {UserStoreService} from "../../services/user-store.service";
import {ResetPasswordService} from "../../services/reset-password.service";

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {

    public loginForm: FormGroup;
    public validationMessage = "";
    public resetPasswordEmail!: string;
    public isEmailValid: boolean = true;

    constructor(private formBuilder: FormBuilder,
                private auth: AuthService,
                private router: Router,
                private toast: NgToastService,
                private userStore: UserStoreService,
                private resetPasswordService: ResetPasswordService) {
    }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            Email: ['', [Validators.required, Validators.email]],
            Password: ['', Validators.required]
        });
    }


    public onLogin() {
        if (this.loginForm.valid) {
            this.auth.logeIn(this.loginForm.value).subscribe(res => {
                this.loginForm.reset();
                this.auth.storeAccessToken(res.accessToken)
                this.auth.storeRefreshToken(res.refreshToken)
                this.userStore.setNameForStore(this.auth.getNameFromAccessToken())
                this.userStore.setRoleForStore(this.auth.getRoleFromAccessToken())
                this.router.navigate(['/employees'])
            }, error => {
                if (error.status === 400 && error.error && error.error.errors) {
                    ValidateForm.handleServerValidationErrors(error.error.errors, this.loginForm);
                } else if ((error.status === 401 || error.status === 400) && error.error && error.error.message) {
                    this.validationMessage = "Incorrect email or password";
                } else {
                    this.toast.error({detail: "ERROR", summary: 'Something went wrong', duration: 3000});
                }
            })
        } else {
            ValidateForm.validateAllFields(this.loginForm);
        }
    }

    public checkValidEmail(value: string) {
        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,3}$/;
        this.isEmailValid = emailPattern.test(value);
        return this.isEmailValid;
    }

    public confirmSendValidationLink() {
        if (this.checkValidEmail(this.resetPasswordEmail)) {
            this.resetPasswordService.sendResetPasswordLink(this.resetPasswordEmail).subscribe(res => {
                    this.toast.success({
                        detail:"Success",
                        summary: "Reset success",
                        duration: 3000
                    })
                    this.resetPasswordEmail = "";
                    document.getElementById("closeBtn")?.click();
                },
                error => {
                    this.toast.error({
                        detail:"ERROR",
                        summary: "Something went wrong",
                        duration: 5000
                    })
                });
        }
    }

}
