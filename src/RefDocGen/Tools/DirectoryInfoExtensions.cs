namespace RefDocGen.Tools;

/// <summary>
/// Class containing extension methods for <see cref="DirectoryInfo"/> class.
/// </summary>
internal static class DirectoryInfoExtensions
{
    // the following code is taken from https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
    /// <summary>
    /// Copies a directory to a new destination.
    /// </summary>
    /// <param name="sourceDir">The directory to copy.</param>
    /// <param name="destination">The destination where to copy the <paramref name="sourceDir"/></param>
    /// <param name="recursive">Indicates whether the subdirectories are also recursively copied.</param>
    /// <exception cref="DirectoryNotFoundException">Thrown if the <paramref name="sourceDir"/> is not found.</exception>
    internal static void CopyTo(this DirectoryInfo sourceDir, string destination, bool recursive)
    {
        // Check if the source directory exists
        if (!sourceDir.Exists)
        {
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir.FullName}");
        }

        // Cache directories before we start copying
        var dirs = sourceDir.GetDirectories();

        // Create the destination directory
        _ = Directory.CreateDirectory(destination);

        // Get the files in the source directory and copy to the destination directory
        foreach (var file in sourceDir.GetFiles())
        {
            string targetFilePath = Path.Combine(destination, file.Name);
            _ = file.CopyTo(targetFilePath, true);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive)
        {
            foreach (var subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destination, subDir.Name);
                subDir.CopyTo(newDestinationDir, true);
            }
        }
    }
}
