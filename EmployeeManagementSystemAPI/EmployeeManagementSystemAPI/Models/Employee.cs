using EmployeeManagementSystemAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystemAPI.Models
{
    public class Employee
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmployeeStatus StatusId { get; set; }

        public string ProfilePictureUrl { get; set; }

        
        // Gets or sets the status of the employee as an enum value.
       
        [NotMapped]
        public EmployeeStatus Status
        {
            get => (EmployeeStatus)StatusId;
            set => StatusId = (EmployeeStatus)(int)value;
        }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }


}
