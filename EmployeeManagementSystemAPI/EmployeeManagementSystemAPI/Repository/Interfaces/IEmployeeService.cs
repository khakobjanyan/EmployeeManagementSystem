using EmployeeManagementSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemAPI.Repository.Interfaces
{
    public interface IEmployeeService
    {
        public Task<Employee> CreateEmployee(Employee employee);
        public Task<Employee> EditEmployee(int id, Employee employee);
        public Task<Employee> GetEmployeeById(int id);
        public Task<List<Employee>> GetEmployees(int page = 1, int pageSize = 10);
        public Task DeleteEmployeeById(int id);

    }
}
