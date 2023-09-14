import {Component, OnInit} from '@angular/core';
import {EmployeeService} from "../../services/employee.service";
import {NgToastService} from "ng-angular-popup";
import {ActivatedRoute, Router} from "@angular/router";
import {Employee} from "../../models/employee.model";
import {ClassifierModel} from "../../models/classifier.model";
import {NgForm} from "@angular/forms";

@Component({
    selector: 'app-create-employee',
    templateUrl: './create-employee.component.html',
    styleUrls: ['./create-employee.component.sass']
})
export class CreateEmployeeComponent implements OnInit {
    public employee: Employee = new Employee();
    public initialEmployee: Employee = new Employee();
    public uploadFile: File | null;
    public showValidation = false;
    public isEditForm = false;
    public statusList: ClassifierModel[] = [];

    constructor(private employeeService: EmployeeService,
                private toast: NgToastService,
                private activeRouter: ActivatedRoute,
                private router: Router) {
        this.employee.status = new class implements ClassifierModel {
            id: number;
            name: string;
        }
    }

    ngOnInit(): void {
        this.activeRouter.params.subscribe((params) => {
            const employeeId = params['id'];

            if (employeeId) {
                this.isEditForm = true;
                this.employeeService.getEmployeeById(employeeId).subscribe(resp => {
                    this.employee = resp.data;
                    this.employee.startDate = new Date(this.employee.startDate);
                    this.initialEmployee = JSON.parse(JSON.stringify(this.employee));
                })


            }
        });
        this.employeeService.getKeyValuePairs().subscribe(resp => {
            this.statusList = resp;
        })
    }


    handleFileInput(files: FileList) {
        if (files.length > 0) {
            this.uploadFile = files.item(0);
        }
    }

    statusChange(event: any){
        this.employee.status.id = event;
    }

    onSubmit() {
        this.showValidation = false;
        if (this.employeeValidation(this.employee)) {
            if (JSON.stringify(this.employee) === JSON.stringify(this.initialEmployee)  && !this.uploadFile) {
                this.toast.warning({detail: "Warning", summary: "NoChanges"})
            } else {
                const formData = new FormData();
                formData.append('firstName', this.employee.firstName);
                formData.append('lastName', this.employee.lastName);
                formData.append('position', this.employee.position);
                formData.append('startDate', this.employee.startDate.toISOString());
                formData.append('status', this.employee.status.id.toString());
                if (this.uploadFile) {
                    formData.append('imageFile', this.uploadFile);
                } else {
                    formData.append('profilePictureUrl', this.employee.profilePictureUrl || "");
                }
                if (this.isEditForm) {
                    formData.append('id', this.employee.id.toString());
                    this.employeeService.editEmployee(this.employee.id, formData).subscribe(resp => {
                            this.toast.success({detail: "Success", summary: "Employee edit"})
                            window.location.reload();
                        },
                        error => {
                            this.showValidation = true;
                        });
                } else {
                    this.employeeService.createEmployee(formData).subscribe(resp => {

                            this.toast.success({detail: "Success", summary: "Employee saved", duration: 5000})
                            this.router.navigate([`/view-employee/${resp.data.id}`])
                        },
                        error => {
                            this.showValidation = true;
                        });
                }
            }
        } else {
            this.toast.error({detail: "Error", summary: "Invalid data"})
            this.showValidation = true;
        }
    }

    onClose() {
        if (this.isEditForm) {
            this.router.navigate([`/view-employee/${this.employee.id}`])
        } else {
            this.router.navigate([`/employees`])
        }
    }


    public employeeValidation(employee: Employee): boolean {
        return !(!employee.firstName || !employee.lastName || !employee.position || !employee.startDate || !employee.status || employee.status.id < 0);

    }

    startDateChange(date: Date) {
        this.employee.startDate = date;
    }

    formatDate(date: Date): string {
        if (date) {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`;
        }
        return ""
    }
}
