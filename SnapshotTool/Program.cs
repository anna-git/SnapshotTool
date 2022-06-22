using System.Diagnostics;
using System.IO.Compression;
using SnapshotTool;

var replacedFilesNumber = 0;
var destfolder = Environment.GetEnvironmentVariable("dest-folder");
if (string.IsNullOrEmpty(destfolder))
    throw new ArgumentNullException($@"dest-folder env variable should exist and is null");
var folder = Environment.GetEnvironmentVariable("snapshots-folder");
if (string.IsNullOrEmpty(folder))
{
    var containingfolder = Environment.GetEnvironmentVariable("snapshots-folder-container");
    var dir = new DirectoryInfo(containingfolder);
    var dirs = dir.GetFiles("*.zip");
    replacedFilesNumber += dirs.Where (zip => zip.Name.Contains("snapshots")).Sum(zip => SnapshotsCopier.Copy(zip.FullName, destfolder));
}
else
{
    replacedFilesNumber = SnapshotsCopier.Copy(folder, destfolder);
}

Console.WriteLine($"Number of files replaced: {replacedFilesNumber}");