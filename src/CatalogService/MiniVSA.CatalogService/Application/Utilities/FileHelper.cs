using MimeDetective;
using MimeDetective.Storage;

namespace MiniVSA.CatalogService.Application.Utilities
{
    public class FileHelper
    {
        public static async Task<(string FileName, string FilePath)> UploadFileToLocalAsync(byte[] file, string fileName, string uploadDirectory)
        {
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var fileExtension = GetFileAttributesFromByteArray(file).Extensions[0];
            var uniqueFileName = $"{fileName}-{Guid.NewGuid()}.{fileExtension}";
            var filePath = Path.Combine(uploadDirectory, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, file);

            return (uniqueFileName, filePath);
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
            const double bytesInMB = 1024 * 1024;
            return fileData.Length / bytesInMB;
        }
    }
}
