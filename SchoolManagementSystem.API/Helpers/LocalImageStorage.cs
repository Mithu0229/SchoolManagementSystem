namespace SchoolManagementSystem.API.Helpers;

public static class LocalImageStorage
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

    public static async Task<string?> SaveAsync(IWebHostEnvironment env, IFormFile? image, string folderName)
    {
        if (image is null || image.Length == 0)
        {
            return null;
        }

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(extension))
        {
            return null;
        }

        var safeFolderName = folderName.Trim('/', '\\');
        if (string.IsNullOrWhiteSpace(safeFolderName)
            || safeFolderName.Contains("..")
            || safeFolderName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        {
            throw new ArgumentException("Invalid upload folder name.", nameof(folderName));
        }

        var webRootPath = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        var uploadFolder = Path.Combine(webRootPath, "uploads", safeFolderName);
        Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        return $"/uploads/{safeFolderName}/{fileName}";
    }
}
