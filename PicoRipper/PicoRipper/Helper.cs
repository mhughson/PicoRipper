using System.IO;

/// <summary>
/// Helper functions.
/// </summary>
public static class Helper
{
    /// <summary>
    /// Removes extension from the end of file path.
    /// </summary>
    /// <param name="path">File path with extension.</param>
    /// <returns>File path without extension.</returns>
    public static string GetFullPathWithoutExtension(string path)
    {
        return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
    }

}
