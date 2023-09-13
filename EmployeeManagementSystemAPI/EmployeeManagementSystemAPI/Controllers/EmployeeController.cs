using EmployeeManagementSystemAPI.Context;
using EmployeeManagementSystemAPI.Helpers;
using EmployeeManagementSystemAPI.Models;
using EmployeeManagementSystemAPI.Models.Dto;
using EmployeeManagementSystemAPI.Models.Enums;
using EmployeeManagementSystemAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemAPI.Controllers
{
    /// <summary>
    /// Controller for managing employee data.
    /// </summary>
    
    [Route("api/[controller]")]
    [EnableCors("AllowLocalhost4200")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _authContext;
        private readonly IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="appDbContext">The application database context.</param>
        /// <param name="fileService">The file service for handling images.</param>
        public EmployeeController(AppDbContext appDbContext, IFileService fileService)
        {
            _authContext = appDbContext;
            _fileService = fileService;
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">The employee data.</param>
        /// <returns>A response indicating success or validation errors.</returns>

        [Authorize]
        [HttpPost("add-employee"), DisableRequestSizeLimit]
        public async Task<IActionResult> CreateEmployee([FromForm] Employee employee)
        {
            // Check if the received data is valid
            if (ModelState.IsValid)
            {
                // If an image is provided, save it and update the profile picture URL
                if (employee.ImageFile != null)
                {
                    employee.ProfilePictureUrl = await _fileService.SaveImageAsync(employee.ImageFile);
                    
                }
                // Add the employee to the database and save changes
                await _authContext.Employees.AddAsync(employee);
                await _authContext.SaveChangesAsync();

                // Return a success message
                return Ok(new
                {
                    Message = "Succsess",
                });
            }
            else
            {
                // If data is not valid, return validation error messages
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }
            
        }

        /// <summary>
        /// Edits an existing employee's information.
        /// </summary>
        /// <param name="id">The ID of the employee to edit.</param>
        /// <param name="employee">The updated employee data.</param>
        /// <returns>A response indicating success, invalid data, not found, or validation errors.</returns>
        [Authorize]
        [HttpPut("edit-employee/{id}"), DisableRequestSizeLimit]
        public async Task<IActionResult> EditEmployee(int id, [FromForm] Employee employee)
        {
            // Check if the received data is valid
            if (ModelState.IsValid)
            {
                // Ensure that the provided ID matches the employee's ID
                if (id != employee.Id)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid data"
                    });
                }

                // Check if the employee exists
                if (!_authContext.Employees.Any(e => e.Id == id))
                {
                    return NotFound(new
                    {
                        Message = "Employee Not Found"
                    });
                }

                // If a new image is provided and different from the current one, update it
                if (employee.ProfilePictureUrl != _authContext.Employees.FirstOrDefault(e => e.Id == employee.Id).ProfilePictureUrl && employee.ImageFile != null) 
                {
                    await _fileService.DeleteImageAsync(_authContext.Employees.FirstOrDefault(e => e.Id == employee.Id).ProfilePictureUrl);
                    employee.ProfilePictureUrl = await _fileService.SaveImageAsync(employee.ImageFile);
                }

                // Detach the existing employee from the context and update the employee
                var existingEmployee = _authContext.Employees.Local.FirstOrDefault(e => e.Id == employee.Id);
                if (existingEmployee != null)
                {
                    _authContext.Entry(existingEmployee).State = EntityState.Detached;
                }

                // Save changes to the database
                _authContext.Entry(employee).State = EntityState.Modified;
                await _authContext.SaveChangesAsync();

                // Return a success message
                return Ok(new
                {
                    Message = "Edit done"
                });
            }
            else
            {
                // If data is not valid, return validation error messages
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }

        }

        /// <summary>
        /// Retrieves an employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>A response containing the employee data or an error message.</returns>
        [Authorize]
        [HttpGet("get-employee/{id}"), DisableRequestSizeLimit]
        public async Task<IActionResult> getEmployeeById(int id)
        {            
            var employee =  await _authContext.Employees.FirstOrDefaultAsync(e=> e.Id == id);
            if(employee == null)
            {
                return BadRequest(new
                {
                    Message = "Somthing went wrong"
                });
            }
            return Ok(new
            {
                TotalRowCount = 1,
                Data = employee
            });
        }

        /// <summary>
        /// Retrieves a list of employees with pagination.
        /// </summary>
        /// <param name="page">The page number for pagination (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>A response containing a paginated list of employees.</returns>

        [Authorize]
        [HttpGet("employee-list")]
        public async Task<IActionResult> GetEmploees(int page = 1, int pageSize = 10)
        {
            var totalCount = _authContext.Employees.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Retrieve employees with pagination from the database
            var employees = await _authContext.Employees
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return a JSON response with pagination information and employee data
            return Ok(new
            {
                TotalCount = totalCount,
                Data = employees,
                TotalPages = totalPages,
                CurrentPage = page
            });;
        }

        /// <summary>
        /// Deletes an employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>A response indicating success or not found.</returns>
        [Authorize]
        [HttpDelete("delete-employee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employeeToDelete = _authContext.Employees.Find(id);

            if (employeeToDelete == null)
            {
                return NotFound(); 
            }
            // Delete the employee's profile picture and remove the employee from the database
            await _fileService.DeleteImageAsync(employeeToDelete.ProfilePictureUrl);
            _authContext.Employees.Remove(employeeToDelete);
            await _authContext.SaveChangesAsync();

            return NoContent();

        }

        /// <summary>
        /// Retrieves a list of employee status values.
        /// </summary>
        /// <returns>A response containing a list of employee status values.</returns>
        [Authorize]
        [HttpGet("employee-status")]
        public ActionResult<IEnumerable<ClassifierModelDto>> GetEmployeeStatus()
        {
            var statusValues = Enum.GetValues(typeof(EmployeeStatus))
                                   .Cast<EmployeeStatus>()
                                   .Select(status => new ClassifierModelDto {Id = (int)status, Name = status.ToString() });

            return Ok(statusValues);
        }
    }
}
