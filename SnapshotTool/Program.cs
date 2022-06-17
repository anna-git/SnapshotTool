using System.Diagnostics;
using System.IO.Compression;

var folder = args[0] ?? Console.ReadLine();
var d = new DirectoryInfo(folder);
if (folder.EndsWith("zip"))
{
    Console.WriteLine($"Folder {d.FullName} will be unzipped");
    var name = d.Name[..^4];
    var fullPath = Path.Combine(d.Parent.FullName, name);
    if (Directory.Exists(fullPath))
    {
        Directory.Delete(fullPath, true);
    }

    var dInfo = d.Parent.CreateSubdirectory(name);
    ZipFile.ExtractToDirectory(folder, dInfo.FullName);
    d = dInfo.EnumerateDirectories().First();
}

foreach (var file in d.GetFiles())
{
    // run under linux if file path is too long, pb with win32 apis
    if (file.Name.EndsWith("verified.txt"))
    {
        file.Delete();
    }
}

var destfolder = args[1];
var replacedFilesNumber = 0;
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
Console.WriteLine($"Number of files replaced: {replacedFilesNumber}");