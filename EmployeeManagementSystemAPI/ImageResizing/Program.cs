
using SixLabors.ImageSharp.Formats.Jpeg;


string tempDirectory = @"C:\Users\Karen Hakobjanyan\source\repos\EmployeeManagementSystem\EmployeeManagementSystemAPI\EmployeeManagementSystemAPI\wwwroot\Temp\";
string imageDirectory = @"C:\Users\Karen Hakobjanyan\source\repos\EmployeeManagementSystem\EmployeeManagementSystemAPI\EmployeeManagementSystemAPI\wwwroot\Images\";

if (!Directory.Exists(tempDirectory))
{
    Console.WriteLine("Temporary directory does not exist.");
    return;
}

if (!Directory.Exists(imageDirectory))
{
    Directory.CreateDirectory(imageDirectory);
}

string[] imageFiles = Directory.GetFiles(tempDirectory, "*.*")
.Where(file => file.ToLower().EndsWith(".jpg") ||
               file.ToLower().EndsWith(".png") ||
               file.ToLower().EndsWith(".jpeg"))
.ToArray();

foreach (string imageFile in imageFiles)
{
    try
    {
        using (var image = Image.Load(imageFile))
        {
            image.Mutate(x => x
                    .Resize(new ResizeOptions
                    {
                        Size = new Size(50, 50),
                        Mode = ResizeMode.Max
                    }));

            string fileName = Path.GetFileName(imageFile);
            string destinationPath = Path.Combine(imageDirectory, fileName);

            using (var destinationStream = File.Create(destinationPath))
            {
                image.Save(destinationStream, new JpegEncoder());
            }

            File.Delete(imageFile);

            Console.WriteLine($"Image processed: {imageFile}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing image: {ex.Message}");
    }
}

Console.WriteLine("Image processing complete.");