var folder = args[0];
var d = new DirectoryInfo(folder);
foreach (var file in d.GetFiles())
{
// run under linux if file path is too long, pb with win32 apis
    if (file.Name.EndsWith("verified.txt"))
    {
        file.Delete();
    }
}

var destfolder = args[1];

foreach (var file in d.GetFiles())
{
    if (file.Name.EndsWith("received.txt"))
    {
        var n = file.Name;
        var newName = n.Replace("received.txt", "verified.txt");
        var newPath = Path.Combine(destfolder, newName);
        file.MoveTo(newPath, true);
    }
}