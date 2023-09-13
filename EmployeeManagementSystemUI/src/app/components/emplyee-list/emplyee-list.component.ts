import {Component, OnInit, ViewChild} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {UserStoreService} from "../../services/user-store.service";
import {Router} from "@angular/router";
import {Employee} from "../../models/employee.model";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";
import {EmployeeService} from "../../services/employee.service";
import {ClassifierModel} from "../../models/classifier.model";

@Component({
  selector: 'app-emplyee-list',
  templateUrl: './emplyee-list.component.html',
  styleUrls: ['./emplyee-list.component.sass']
})
export class EmplyeeListComponent implements OnInit{
  public employees : Employee[];
  public userName: string = "";
  public role: string = "";
  public totalCount = 0;
  public pageSize = 5;
  public currentPage = 1;
  public totalPages = 0;
  public statusList: ClassifierModel[] = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private auth: AuthService,
              private userStore: UserStoreService,
              private employeeService: EmployeeService) {
  }
  ngOnInit() {
    this.loadEmployee();
    this.userStore.getNameFromStore().subscribe(name => {
      this.userName = name || this.auth.getNameFromAccessToken();
    })
    this.userStore.getRoleFromStore().subscribe(role =>{
      this.role = role || this.auth.getAccessToken();
    })
    this.employeeService.getKeyValuePairs().subscribe(resp => {
      this.statusList = resp;
    })
  }

  getStatusName(status : number): string{
    const index = this.statusList.findIndex(e => e.id === status);
    if(index > -1){
      return this.statusList[index].name
    }
    return "";
  }


  clickPaging(page: number){
    this.loadEmployee();
  }

  loadEmployee(){
    this.employeeService.getEmployees(this.currentPage, this.pageSize).subscribe(res => {
      this.employees = res.data;
      this.totalCount = res.totalCount;
      this.currentPage = res.currentPage;
      this.totalPages = res.totalPages;
    });
  }


  protected readonly Array = Array;
}
