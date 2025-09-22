
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
            };
        }

        this.recordsById[recordId].PairsByField[field] = new Pair
        {
            Id = field,
            Value = value
        };

        return string.Empty;
    }

    public string Get(string recordId, string field)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            return string.Empty;
        }

        if (!this.recordsById[recordId].PairsByField.ContainsKey(field))
        {
            return string.Empty;
        }

        return this.recordsById[recordId].PairsByField[field].Value;
    }

    public string Delete(string recordId, string field)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            return "false";
        }

        if (!this.recordsById[recordId].PairsByField.ContainsKey(field))
        {
            return "false";
        }

        this.recordsById[recordId].PairsByField.Remove(field);

        return "true";
    }

    public string Scan(string recordId)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            return string.Empty;
        }

        var result = new List<string>();
        foreach (var pair in this.recordsById[recordId].PairsByField.Values)
        {
            result.Add($"{pair.Id}({pair.Value})");
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
        foreach (var pair in this.recordsById[recordId].PairsByField.Values)
        {
            if (pair.Id.StartsWith(fieldPrefix))
            {
                result.Add($"{pair.Id}({pair.Value})");
            }
        }

        result = result.Order().ToList();

        return string.Join(",", result);
    }

    public string SetAt(string recordId, string field, string value, int timestamp)
    {
        this.Set(recordId, field, value);

        var pair = this.recordsById[recordId].PairsByField[field];
        pair.Timestamp = timestamp;

        return string.Empty;
    }

    public string SetAtWithTtl(string recordId, string field, string value, int timestamp, int ttl)
    {
        this.SetAt(recordId, field, value, timestamp);

        var pair = this.recordsById[recordId].PairsByField[field];
        pair.Ttl = ttl;

        return string.Empty;
    }

    public string GetAt(string recordId, string field, int timestamp)
    {
        var result = this.Get(recordId, field);

        if (this.recordsById[recordId].PairsByField[field].IsExpired(timestamp))
        {
            return string.Empty;
        }

        return result;
    }

    public string DeleteAt(string recordId, string field, int timestamp)
    {
        if (!this.recordsById.ContainsKey(recordId))
        {
            return "false";
        }

        if (!this.recordsById[recordId].PairsByField.ContainsKey(field))
        {
            return "false";
        }

        if (this.recordsById[recordId].PairsByField[field].IsExpired(timestamp))
        {
            return "false";
        }

        return this.Delete(recordId, field);
    }

    public string ScanAt(string recordId, int timestamp)
    {

        var result = new List<string>();
        foreach (var pair in this.recordsById[recordId].PairsByField.Values)
        {
            if (!pair.IsExpired(timestamp))
            {
                result.Add($"{pair.Id}({pair.Value})");                
            }
        }

        return string.Join(",", result);        
    }

    public string ScanAtPrefix(string recordId, string fieldPrefix, int timestamp)
    {

        var result = new List<string>();
        foreach (var pair in this.recordsById[recordId].PairsByField.Values)
        {
            if (!pair.IsExpired(timestamp))
            {
                if (pair.Id.StartsWith(fieldPrefix))
                {
                    result.Add($"{pair.Id}({pair.Value})");
                }
            }
        }

        return string.Join(",", result);
    }

    public void PrintState()
    {
        Console.WriteLine($"state:");
        foreach (var record in this.recordsById)
        {
            Console.WriteLine($"'{record.Key}': ");
            foreach (var pair in record.Value.PairsByField.Values)
            {
                Console.WriteLine($"  '{pair.Id}':'{pair.Value}', {pair.Timestamp}, {pair.Ttl}");
            }
        }
    }
}


public class Record
{
    public string Id { get; set; }
    public Dictionary<string, Pair> PairsByField = new Dictionary<string, Pair>();
}

public class Pair
{
    public string Id { get; set; }
    public string Value { get; set; }
    public int Timestamp { get; set; }
    public int? Ttl { get; set; } = null;
    public bool IsExpired(int timestamp)
    {
        if (this.Ttl == null)
        {
            return false;
        }

        if ((this.Timestamp + this.Ttl) > timestamp)
        {
            return false;
        }

        return true;
    }
}