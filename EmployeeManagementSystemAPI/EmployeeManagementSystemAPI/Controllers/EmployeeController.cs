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
        private readonly AppDbContext _dbContext;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="appDbContext">The application database context.</param>
        /// <param name="fileService">The file service for handling images.</param>
        public EmployeeController(AppDbContext appDbContext, IEmployeeService employeeService, ILogger logger)
        {
            _dbContext = appDbContext;
            _employeeService = employeeService;
            _logger = logger;
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
                try
                {
                    var createdEmployee = await _employeeService.CreateEmployee(employee);

                    // Return a success message
                    return Ok(new
                    {
                        Message = "Success",
                        Data = createdEmployee
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    return StatusCode(500, new
                    {
                        Message = "An error occurred while creating the employee.",
                        Error = ex.Message
                    });
                }

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
                try
                {
                    var editedEmployee = await _employeeService.EditEmployee(id, employee);

                    return Ok(new
                    {
                        Message = "Edit done",
                        Data = editedEmployee
                    });
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(new
                    {
                        Message = ex.Message
                    });
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid data",
                        Error = ex.Message
                    });
                } catch(Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    return StatusCode(500, new
                    {
                        Message = "An error occurred while editing the employee.",
                        Error = ex.Message
                    });
                }
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
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                // Retrieve the employee using the EmployeeService
                var employee = await _employeeService.GetEmployeeById(id);

                return Ok(new
                {
                    TotalRowCount = 1,
                    Data = employee
                });
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving the employee.",
                    Error = ex.Message
                });
            }
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
            try
            {
                // Validate input values (e.g., page, pageSize)
                if (page < 1 || pageSize < 1)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid pagination parameters",
                    });
                }

                // Retrieve employees with pagination using the EmployeeService
                var employees = await _employeeService.GetEmployees(page, pageSize);

                var totalCount = await _dbContext.Employees.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return Ok(new
                {
                    TotalCount = totalCount,
                    Data = employees,
                    TotalPages = totalPages,
                    CurrentPage = page
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving employees.",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Deletes an employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>A response indicating success or not found.</returns>
        [Authorize]
        [HttpDelete("delete-employee/{id}")]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            try
            {
                // Delete the employee using the EmployeeService
                await _employeeService.DeleteEmployeeById(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, new
                {
                    Message = "An error occurred while deleting the employee.",
                    Error = ex.Message
                });
            }

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
