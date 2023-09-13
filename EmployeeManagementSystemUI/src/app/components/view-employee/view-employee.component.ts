import {Component, OnInit} from '@angular/core';
import {Employee} from "../../models/employee.model";
import {EmployeeService} from "../../services/employee.service";
import {ActivatedRoute, Router} from "@angular/router";
import {ClassifierModel} from "../../models/classifier.model";

@Component({
  selector: 'app-view-employee',
  templateUrl: './view-employee.component.html',
  styleUrls: ['./view-employee.component.sass']
})
export class ViewEmployeeComponent implements OnInit{
  public employee: Employee = new Employee();
  constructor(private employeeService: EmployeeService,
              private activeRouter: ActivatedRoute,
              private router: Router) {
  }


  ngOnInit(): void {
    this.activeRouter.params.subscribe((params) => {
      const employeeId = params['id'];

      if (employeeId) {
        this.employeeService.getEmployeeById(employeeId).subscribe(resp => {
          this.employee = resp.data
          this.employee.startDate = new Date(this.employee.startDate)
        })
      }
    });

  }


  editEmployee(){
    this.router.navigate([`add-employee/${this.employee.id}`])
  }

  deleteEmployee(){
    this.employeeService.deleteEmployee(this.employee.id);
    this.router.navigate([`employees`])
  }
}
