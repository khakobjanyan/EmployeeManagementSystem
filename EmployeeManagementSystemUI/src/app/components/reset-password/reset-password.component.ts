import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ResetPassword} from "../../models/reset-password.model";
import {ConfirmPasswordValidator} from "../../helpers/confirm-password.validator";
import {ActivatedRoute, Router} from "@angular/router";
import ValidateForm from "../../helpers/validateForm";
import {ResetPasswordService} from "../../services/reset-password.service";
import {NgToastService} from "ng-angular-popup";

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.sass']
})
export class ResetPasswordComponent implements OnInit {
    public resetPasswordForm!: FormGroup;
    public emailToReset!: string;
    public emailToken!: string;
    public resetPasswordObj = new ResetPassword();

    constructor(private fb: FormBuilder,
                private activatedRout: ActivatedRoute,
                private resetPasswordService: ResetPasswordService,
                private toast: NgToastService,
                private router: Router) {
    }

    ngOnInit() {
        this.resetPasswordForm = this.fb.group({
            password: ['', [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$/)]],
            confirmPassword: ['', [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$/)]],

        }, {
            validators: ConfirmPasswordValidator("password", "confirmPassword")
        });

        this.activatedRout.queryParams.subscribe(params => {
            this.emailToReset = params["email"];
            let urlToken = params["code"];
            this.emailToken = urlToken.replace(/ /g, '+');
        })
    }

    public reset() {
        if (this.resetPasswordForm.valid) {
            this.resetPasswordObj.email = this.emailToReset;
            this.resetPasswordObj.newPassword = this.resetPasswordForm.value.password;
            this.resetPasswordObj.confirmPassword = this.resetPasswordForm.value.confirmPassword;
            this.resetPasswordObj.emailToken = this.emailToken;

            this.resetPasswordService.resetPassword(this.resetPasswordObj).subscribe(res => {
                    this.toast.success({
                        detail:"Success",
                        summary: "Password reset successfully",
                        duration: 3000
                    })
                this.router.navigate(['login'])
                },
                error => {
                    this.toast.error({
                        detail:"ERROR",
                        summary: "Something went wrong",
                        duration: 3000
                    })
                });
        } else {
            ValidateForm.validateAllFields(this.resetPasswordForm);
        }
    }
}
