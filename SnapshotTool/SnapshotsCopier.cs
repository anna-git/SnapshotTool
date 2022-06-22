using System.IO.Compression;

namespace SnapshotTool;

public static class SnapshotsCopier
{
    public static int Copy(string folder, string destfolder)
    {
        var d = new DirectoryInfo(folder);
        if (folder.EndsWith("zip"))
        {
            Console.WriteLine($"Folder {d.FullName} will be unzipped");
            var name = d.Name[..^4]; //remove zip extension
            var fullPath = Path.Combine(d.Parent.FullName, name);
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }

            var dInfo = d.Parent.CreateSubdirectory(name);
            ZipFile.ExtractToDirectory(folder, dInfo.FullName);
            d = dInfo.EnumerateDirectories().FirstOrDefault();
        }

        foreach (var file in d.GetFiles())
        {
            // run under linux if file path is too long, pb with win32 apis
            if (file.Name.EndsWith("verified.txt"))
            {
                file.Delete();
            }
        }

        var replacedFilesNumber = 0;
        Console.WriteLine($"Dest Folder: {destfolder}");

        foreach (var file in d.GetFiles())
        {
            if (file.Name.EndsWith("received.txt"))
            {
                replacedFilesNumber++;
                var n = file.Name;
                var newName = n.Replace("received.txt", "verified.txt");
                var newPath = Path.Combine(destfolder, newName);
                file.MoveTo(newPath, true);
            }
        }

        return replacedFilesNumber;
    }
}