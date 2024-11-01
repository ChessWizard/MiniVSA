using MimeDetective;
using FileType = MimeDetective.Storage.FileType;

namespace MiniVSA.CatalogService.Application.Utilities
{
    public class FileHelper(IWebHostEnvironment environment)
    {
        private readonly IWebHostEnvironment _environment = environment;

        public static async Task<(string FileName, string FilePath)> UploadFileToLocalAsync(byte[] file, Domain.Enums.FileType fileType, string uploadDirectory)
        {
            if(file is null || !file.Any())
                throw new ArgumentNullException(nameof(file));

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var fileExtension = GetFileAttributesFromByteArray(file).Extensions[0];
            var filePrefix = fileType == Domain.Enums.FileType.Image ? "image" : "document";
            var uniqueFileName = $"{filePrefix}-{Guid.NewGuid()}.{fileExtension}";
            var filePath = Path.Combine(uploadDirectory, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, file);

            return (uniqueFileName, filePath);
        }

        public static async Task DeleteFileFromLocalAsync(string basePath, string filePath)
        {
            var fullPath = Path.Combine(basePath, filePath);

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }
        }

        public static FileType GetFileAttributesFromByteArray(byte[] fileBinary)
        {
            if (fileBinary is null || !fileBinary.Any())
                return null;

            var inspector = new ContentInspectorBuilder()
            {
                Definitions = MimeDetective.Definitions.Default.All()
            }.Build();

            var matches = inspector.Inspect(fileBinary);
            return matches[0].Definition.File;
        }

        public static double GetFileSizeAsMB(byte[] fileData)
        {
            if(fileData is null || !fileData.Any()) return 0;

            const double bytesInMB = 1024 * 1024;
            return fileData.Length / bytesInMB;
        }
    }
}
