using System.IO;
using System.Threading.Tasks;

public class FileService
{
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

    public FileService()
    {
        Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> SaveFileAsync(string fileName, Stream fileStream)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }
        return filePath;
    }

    public Stream GetFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
        return null;
    }
}
