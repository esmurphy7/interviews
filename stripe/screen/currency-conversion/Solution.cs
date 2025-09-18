
public class Solution
{
    private Dictionary<string, Dictionary<string, double>> ConversionRates { get; set; } = new Dictionary<string, Dictionary<string, double>>();

    public Solution(string inputRates)
    {
        var rates = inputRates.Split(',');
        foreach (var rate in rates)
        {
            var props = rate.Split(':');
            var srcCountry = props[0];
            var destCountry = props[1];
            var val = double.Parse(props[2]);

            this.AddConversionRate(srcCountry, destCountry, val);
            this.AddConversionRate(destCountry, srcCountry, 1 / val);
        }
    }

    private void AddConversionRate(string srcCountry, string destCountry, double rate)
    {
        if (this.ConversionRates.TryGetValue(srcCountry, out var conversionRates))
        {
            conversionRates[destCountry] = rate;
        }
        else
        {
            this.ConversionRates[srcCountry] = new Dictionary<string, double>
            {
                { destCountry, rate },
            };
        }
    }

    public double GetDirectConversionRate(string srcCountry, string destCountry)
    {
        if (this.ConversionRates.TryGetValue(srcCountry, out var conversionRates))
        {
            if (conversionRates.TryGetValue(destCountry, out var rate))
            {
                return rate;
            }
        }

        return -1;
    }

    public double GetSingleIntermediateConversionRate(string srcCountry, string destCountry)
    {
        var directRate = this.GetDirectConversionRate(srcCountry, destCountry);
        if (directRate != -1)
        {
            return directRate;
        }

        // target: CAD to AUD
        // CAD:USD->0.8
        // USD:AUD->1.33,CAD->1.25
        if (this.ConversionRates.TryGetValue(srcCountry, out var srcRates))
        {
            foreach (var intermediateKey in srcRates.Keys)
            {
                if (this.ConversionRates.TryGetValue(intermediateKey, out var destRates))
                {
                    if (destRates.TryGetValue(destCountry, out var destRate))
                    {
                        var intermediateRate = srcRates[intermediateKey] * destRate;
                        return intermediateRate;
                    }
                }
            }
        }

        return -1;
    }

    public double GetBestConversionRate(string srcCountry, string destCountry)
    {
        var directRate = this.GetDirectConversionRate(srcCountry, destCountry);
        if (directRate != -1)
        {
            return directRate;
        }

        // target: CAD to AUD
        // CAD: USD->0.8,MX->13.3
        // USD: AUD->1.33
        // MX: AUD->0.082
        if (this.ConversionRates.TryGetValue(srcCountry, out var srcRates))
        {
            var intermediateRates = new List<double>();
            foreach (var intermediateKey in srcRates.Keys)
            {
                if (this.ConversionRates.TryGetValue(intermediateKey, out var destRates))
                {
                    if (destRates.TryGetValue(destCountry, out var destRate))
                    {
                        var intermediateRate = srcRates[intermediateKey] * destRate;
                        intermediateRates.Add(intermediateRate);
                    }
                }
            }

            var bestRate = intermediateRates.Max();
            return bestRate;
        }

        return -1;
    }

    public bool IsConversionPossible(string srcCountry, string destCountry)
    {
        // store list of country nodes that have been visited
        // store queue of next country nodes to traverse
        // add the src country to the queue
        // while there are still countries to traverse
        // dequeue a country
        // if the country has been visited, continue
        // if the country is the dest country, return true
        // ->foreach of its neighbouring countries
        // --> if it hasn't been visited, add it to the queue
        // mark the country as visited
        // return false

        if (srcCountry == destCountry)
        {
            return true;
        }

        var visited = new HashSet<string>();
        var nextCountries = new Queue<string>();
        nextCountries.Enqueue(srcCountry);

        while (nextCountries.Count > 0)
        {
            var country = nextCountries.Dequeue();

            if (visited.Contains(country))
            {
                continue;
            }

            if (country == destCountry)
            {
                return true;
            }

            if (this.ConversionRates.TryGetValue(country, out var nextRates))
            {
                foreach (var neighbor in nextRates.Keys)
                {
                    nextCountries.Enqueue(neighbor);
                }
            }

            visited.Add(country);
        }

        return false;
    }
}