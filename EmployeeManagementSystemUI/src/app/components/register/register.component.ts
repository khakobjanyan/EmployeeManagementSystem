import {Component,  OnInit} from '@angular/core';
import {FormBuilder,  FormGroup, Validators} from "@angular/forms";
import ValidateForm from "../../helpers/validateForm";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {NgToastService} from "ng-angular-popup";
import {PasswordEncryptorService} from "../../services/password-encryptor.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass']
})
export class RegisterComponent implements OnInit {

  public registerForm: FormGroup;
  constructor(private formBuilder: FormBuilder,
              private auth: AuthService,
              private router: Router,
              private toast: NgToastService,
              private passwordEncryptorService: PasswordEncryptorService) {
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],
      UserName: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$/)]]
    });
  }



  public onRegister(){
    if(this.registerForm.valid){
      this.registerForm.get("Password")?.setValue(this.passwordEncryptorService.encryptPassword(this.registerForm.get("Password")?.value));
      this.auth.register(this.registerForm.value).subscribe(res => {
        this.registerForm.reset();
        this.router.navigate(['login']);

      }, error => {
        if (error.status === 400 && error.error && error.error.errors) {
          ValidateForm.handleServerValidationErrors(error.error.errors, this.registerForm);
        } else {
          this.toast.error({detail:"ERROR",summary:'Something went wrong', duration: 3000});
        }
      })
    } else {
      ValidateForm.validateAllFields(this.registerForm);
    }
  }

}
