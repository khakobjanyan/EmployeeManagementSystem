using EmployeeManagementSystemAPI.Context;
using EmployeeManagementSystemAPI.Models;
using EmployeeManagementSystemAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemAPI.Repository.Impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        public EmployeeService(AppDbContext appDbContext, IFileService fileService)
        {
            _dbContext = appDbContext;
            _fileService = fileService;

        }
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            // If an image is provided, save it and update the profile picture URL
            if (employee.ImageFile != null)
            {
                employee.ProfilePictureUrl = await _fileService.SaveImageAsync(employee.ImageFile);

            }
            // Add the employee to the database and save changes
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            return employee;
        }

        

        public async Task<Employee> EditEmployee(int id, Employee employee)
        {
            // Ensure that the provided ID matches the employee's ID
            if (id != employee.Id)
            {
                throw new ArgumentException("Invalid data");
            }

            // Check if the employee exists
            var existingEmployee = await _dbContext.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                throw new ArgumentNullException("Employee not found");
            }

            // If a new image is provided and different from the current one, update it
            if (employee.ProfilePictureUrl != existingEmployee.ProfilePictureUrl && employee.ImageFile != null)
            {
                await _fileService.DeleteImageAsync(existingEmployee.ProfilePictureUrl);
                employee.ProfilePictureUrl = await _fileService.SaveImageAsync(employee.ImageFile);
            }

            // Update the existing employee with the new data
            _dbContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
            await _dbContext.SaveChangesAsync();

            return existingEmployee; // Return the edited employee
        }

        public async Task<List<Employee>> GetEmployees(int page, int pageSize)
        {
            var employees = await _dbContext.Employees
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return employees;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            // Check if the employee exists
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                throw new ArgumentNullException("Employee not found");
            }

            return employee;
        }

        public async Task DeleteEmployeeById(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee != null)
            {
                // Delete the employee's profile picture if it exists
                if (!string.IsNullOrEmpty(employee.ProfilePictureUrl))
                {
                    await _fileService.DeleteImageAsync(employee.ProfilePictureUrl);
                }

                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
