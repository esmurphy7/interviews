public class Solution
{
    public Dictionary<string, Dictionary<int, FileData>> fileVersionsByName = new Dictionary<string, Dictionary<int, FileData>>();
    public Dictionary<string, Dictionary<int, FileData>> trashFileVersionsByName = new Dictionary<string, Dictionary<int, FileData>>();
}

public class FileData
{
    public string Name { get; set; }
    public int Size { get; set; }
}