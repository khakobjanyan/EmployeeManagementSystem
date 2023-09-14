import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Employee} from "../models/employee.model";
import {map, Observable} from "rxjs";
import {ClassifierModel} from "../models/classifier.model";

@Injectable({
    providedIn: 'root'
})
export class EmployeeService {
    private baseUrl: string = "https://localhost:7262/api/Employee/";

    constructor(private http: HttpClient) {
    }

    public createEmployee(employee: FormData): Observable<ResponseDataAndMessage> {
        return this.http.post(`${this.baseUrl}add-employee`, employee).pipe(
          map((response: any) => {
            const employee: Employee = {
              id: response.data.id,
              firstName: response.data.firstName,
              lastName: response.data.lastName,
              position: response.data.position,
              startDate: new Date(response.data.startDate),
              status: {
                id: response.data.status,
                name: response.data.statusId,
              },
              profilePictureUrl: response.data.profilePictureUrl,
            } as Employee;

            return {
              message: response.message,
              data: employee,
            } as ResponseDataAndMessage;
          })
        );
    }

    public editEmployee(employeeId: number, employee: FormData) {
        return this.http.put(`${this.baseUrl}edit-employee/${employeeId}`, employee)
    }

    public getEmployeeById(employeeId: number): Observable<ResponseData> {
        return this.http.get(`${this.baseUrl}get-employee/${employeeId}`).pipe(
            map((response: any) => {
                const employee: Employee = {
                    id: response.data.id,
                    firstName: response.data.firstName,
                    lastName: response.data.lastName,
                    position: response.data.position,
                    startDate: new Date(response.data.startDate),
                    status: {
                        id: response.data.status,
                        name: response.data.statusId,
                    },
                    profilePictureUrl: response.data.profilePictureUrl,
                } as Employee;

                return {
                    totalCount: response.totalCount,
                    data: employee,
                } as ResponseData;
            })
        );
    }


    public getEmployees(page: number = 1, pageSize: number = 10): Observable<EmployeeListData> {
        const params = new HttpParams()
            .set('page', page.toString())
            .set('pageSize', pageSize.toString());

        return this.http.get<EmployeeListData>(`${this.baseUrl}employee-list`, {params});
    }

    public deleteEmployee(employeeId: number) {
        return this.http.delete(`${this.baseUrl}delete-employee/${employeeId}`);
    }

    getKeyValuePairs(): Observable<ClassifierModel[]> {
        return this.http.get<ClassifierModel[]>(`${this.baseUrl}employee-status`);
    }
}

interface ResponseData {
    totalCount: number,
    data: Employee
}
interface ResponseDataAndMessage {
  message: string,
  data: Employee
}
interface EmployeeListData {
    totalCount: number,
    data: Employee[],
    totalPages: number,
    currentPage: number
}
