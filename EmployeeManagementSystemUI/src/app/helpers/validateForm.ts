import {FormControl, FormGroup} from "@angular/forms";

export default class ValidateForm{
  static validateAllFields(formGroup: FormGroup){
    Object.keys(formGroup.controls).forEach(field =>{
      const control = formGroup.get(field);
      if(control instanceof FormControl){
        control.markAllAsTouched()
      } else if(control instanceof FormGroup){
        this.validateAllFields(control)
      }
    })
  }

  static handleServerValidationErrors(validationErrors: any, formGroup: FormGroup) {
    for (const field in validationErrors) {
      if (validationErrors.hasOwnProperty(field)) {
        const control = formGroup.get(field);
        if (control) {
          control.setErrors({ serverError: validationErrors[field] });
        }
      }
    }
  }
}
