
public class Solution
{
    private Dictionary<string, Record> recordsById = new Dictionary<string, Record>();

    public string Set(string recordId, string field, string value)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            this.recordsById[recordId] = new Record
            {
                Id = recordId,
                ValuesByField = new Dictionary<string, string>
                {
                    { field, value },
                },
            };
        }
        else
        {
            this.recordsById[recordId].ValuesByField[field] = value;
        }

        return string.Empty;
    }

    public string Get(string recordId, string field)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            return string.Empty;
        }

        if (!this.recordsById[recordId].ValuesByField.ContainsKey(field))
        {
            return string.Empty;
        }

        return this.recordsById[recordId].ValuesByField[field];
    }

    public string Delete(string recordId, string field)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            return "false";
        }

        if (!this.recordsById[recordId].ValuesByField.ContainsKey(field))
        {
            return "false";
        }

        this.recordsById[recordId].ValuesByField.Remove(field);

        return "true";
    }

    public string Scan(string recordId)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            return string.Empty;
        }

        var result = new List<string>();
        foreach (var pair in this.recordsById[recordId].ValuesByField)
        {
            result.Add($"{pair.Key}({pair.Value})");
        }

        return string.Join(",", result);
    }

    public string ScanByPrefix(string recordId, string fieldPrefix)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            return string.Empty;
        }

        var result = new List<string>();
        foreach (var pair in this.recordsById[recordId].ValuesByField)
        {
            if (pair.Key.StartsWith(fieldPrefix))
            {
                result.Add($"{pair.Key}({pair.Value})");
            }
        }

        result = result.Order().ToList();

        return string.Join(",", result);
    }



    public void PrintState()
    {
        Console.WriteLine($"state:");
        foreach (var record in this.recordsById)
        {
            Console.WriteLine($"'{record.Key}': ");
            foreach (var pair in record.Value.ValuesByField)
            {
                Console.WriteLine($"  '{pair.Key}':'{pair.Value}'");
            }
        }
    }
}


public class Record
{
    public string Id { get; set; }
    public Dictionary<string, string> ValuesByField = new Dictionary<string, string>();
}