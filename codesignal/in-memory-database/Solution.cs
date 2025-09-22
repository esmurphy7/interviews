
public class Solution
{
    private Dictionary<string, Record> recordsById = new Dictionary<string, Record>();
    private Dictionary<int, Solution> backupsByTimestamp = new Dictionary<int, Solution>();

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

    public string Backup(int timestamp)
    {
        // create a backup instance
        // collect every pair whose timestamp is <= the given backup timestamp
        // add each pair to the backup instance
        // store the backup
        // return the number of pairs added

        var backup = new Solution();
        foreach (var record in this.recordsById.Values)
        {
            var newRecord = new Record
            {
                Id = record.Id,
            };

            foreach (var pair in record.PairsByField.Values)
            {
                if (pair.Timestamp <= timestamp)
                {
                    newRecord.PairsByField[pair.Id] = new Pair
                    {
                        Id = pair.Id,
                        Value = pair.Value,
                        Timestamp = pair.Timestamp,
                        Ttl = pair.Ttl,
                    };
                }
            }

            if (newRecord.PairsByField.Values.Count > 0)
            {
                backup.recordsById[record.Id] = newRecord;
            }
        }

        this.backupsByTimestamp[timestamp] = backup;

        return backup.recordsById.Values.Count.ToString();
    }

    public string Restore(int timestamp, int timestampToRestore)
    {
        var backupTimestamp = 0;
        foreach (var t in this.backupsByTimestamp.Keys.Order().ToList())
        {
            if (t <= timestampToRestore)
            {
                backupTimestamp = t;
            }
        }

        var backup = this.backupsByTimestamp[backupTimestamp];

        // foreach record in the backup
        // foreach pair in the record
        // calculate the pair's remaining lifespan
        // --> remaining lifespan = pair's ttl - elapsed time
        // ---> elapsed time = backup time - pair's time
        // set the pair's ttl as the remaining lifespan
        // set the pair's timestamp as the time when restore was called
        // replace this instance's records with the backup's records

        foreach (var record in backup.recordsById.Values)
        {
            foreach (var pair in record.PairsByField.Values)
            {
                var elapsedTime = backupTimestamp - pair.Timestamp;
                var remainingLifespan = pair.Ttl - elapsedTime;
                pair.Ttl = remainingLifespan;
                pair.Timestamp = timestamp;
            }
        }

        this.recordsById = backup.recordsById;

        return string.Empty;
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