using EmployeeManagementSystemAPI.Controllers;
using EmployeeManagementSystemAPI.Repository.Interfaces;
using Hangfire;

namespace EmployeeManagementSystemAPI.Repository.Impl
{   
    /// <summary>
    /// Service for managing files and images.
    /// </summary>
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _envirement;
        private readonly ILogger<FileService> _logger;

        
        public FileService(IWebHostEnvironment envirement, ILogger<FileService> logger)
        {

            _envirement = envirement;
            _logger = logger;
        }

        /// <summary>
        /// Saves an image file to the server.
        /// </summary>
        /// <param name="file">The image file to be saved.</param>
        /// <returns>The filename of the saved image.</returns>
        public async Task<string> SaveImageAsync(IFormFile file)
        {
            var contentPath = _envirement.WebRootPath;
            var path = Path.Combine(contentPath, "Images");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var ext = Path.GetExtension(file.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".JPG", ".JPEG", ".PNG" };
            if(!allowedExtensions.Contains(ext))
            {
                string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                throw new FormatException(msg);
            }
            string uniqueString = Guid.NewGuid().ToString();
            var newFileName = uniqueString + ext;
            var fileWithPath = Path.Combine(path, newFileName);
            using (var stream = new FileStream(fileWithPath, FileMode.Create))
            {
               await file.CopyToAsync(stream);

            }

            BackgroundJob.Enqueue(() => ResizeImageInBackgroundAsync(fileWithPath));

            return newFileName;
        }

        /// <summary>
        /// Deletes an image file from the server.
        /// </summary>
        /// <param name="imageFileName">The filename of the image to be deleted.</param>
        /// <returns>True if the image was deleted, false otherwise.</returns>
        public async Task<bool> DeleteImageAsync(string imageFileName)
        {
            var wwwPath = _envirement.WebRootPath;
            var path = Path.Combine(wwwPath, "Images", imageFileName);
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }catch(IOException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            
        }

        public async Task ResizeImageInBackgroundAsync(string imagePath)
        {
            // Load, resize, and save the image asynchronously
            using (var image = Image.Load(imagePath))
            {
                
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(50, 50), 
                    Mode = ResizeMode.Max
                }));

                await image.SaveAsync(imagePath);
            }
        }

    }
}
