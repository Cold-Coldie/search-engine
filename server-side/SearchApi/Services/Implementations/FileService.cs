using SearchApi.Services.Interfaces; // Importing the interface for the file service.

namespace SearchApi.Services.Implementations
{
    // Implementation of the IFileService interface.
    public class FileService : IFileService
    {
        // Private field to store the hosting environment, used to access the content root path.
        private readonly IWebHostEnvironment hostingEnvironment;

        // Constructor to inject the hosting environment dependency.
        public FileService(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment; // Assigning the injected hosting environment to the local field.
        }

        // Method to store an uploaded image file to a local folder on the server.
        public async Task<string> StoreImageToLocalFolder(IFormFile file)
        {
            // Determine the path to the 'Uploads' folder within the content root.
            string uploadsFolder = Path.Combine(hostingEnvironment.ContentRootPath, "Uploads");

            // Create a unique file name by combining a GUID with the original file name to avoid collisions.
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            // Combine the folder path and the unique file name to get the full file path.
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Create a file stream and copy the contents of the uploaded file to the server's file system.
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream); // Asynchronously copies the file data to the file stream.

            // Return the path where the file was saved.
            return filePath;
        }

        // Method to delete a file from the local file system.
        public void DeleteFile(string filePath)
        {
            // Check if the file exists at the specified path.
            if (File.Exists(filePath))
            {
                // If the file exists, delete it.
                File.Delete(filePath);
            }
        }
    }
}
