namespace EmployeeManagementSystemAPI.Repository.Interfaces
{
    public interface IFileService
    {
        public Task<string> SaveImageAsync(IFormFile file);
        public Task<bool> DeleteImageAsync(string imageFileName);
    }
}
