public class Solution
{

    /// <summary>
    /// Part1 - Calculates total shipping cost of an order
    /// </summary>
    public int calculateCost(Order order, ShippingCost allCosts)
    {
        // find the shipping costs for the order's country
        // track the sum of all order costs
        // for each item in the order
        // find the cost for the product
        // multiply the quantity of the product by the cost
        // add the result to the sum of all order costs
        if (!allCosts.costsByCountry.TryGetValue(order.Country, out var countryCosts))
        {
            throw new Exception($"cannot find shipping costs for country {order.Country}");
        }

        var totalCost = 0;
        foreach (var orderItem in order.items)
        {
            var shippingCost = countryCosts.FirstOrDefault(c => c.Product == orderItem.Product);
            if (shippingCost == null)
            {
                throw new Exception($"could not find shipping cost for {orderItem.Product} in {order.Country}");
            }

            var totalProductCost = shippingCost.cost * orderItem.quantity;
            totalCost += totalProductCost;
        }

        return totalCost;
    }

    /// <summary>
    /// Part 2 - Calculates total shipping costs of an order where costs are incremental
    /// </summary>
    public int calculateIncrementalCost(Order order, IncrementalShippingCost allCosts)
    {
        // lookup the shipping cost for the order's country
        // store sum of total costs
        // foreach item in the order
        // lookup the cost definition for the item

        // store a sum of the item's total cost
        // store a count of remaining item quantities that need to be costed
        // foreach of the item's shipping cost ranges
        // if there are no remaining item quantities left, break
        // if the range's max is null
        // -> multiply the range's cost by all remaining item quantities
        // -> add the result to the item's cost sum
        // -> set the remaining item quantities to 0
        // otherwise
        // -> calculate the number of items within the range
        // --> the smaller of the range's min or the remaining items
        // --> increment the number of items in range by 1 if the range's min is 0
        // -> multiply the range's cost by the number of items within the range
        // -> add the result to the item's cost sum
        // -> subtract the range's max from the remaining items
        // multiply the range's cost value with the item's quantity and add it to the sum

        if (!allCosts.costsByCountry.TryGetValue(order.Country, out var countryCosts))
        {
            throw new Exception($"cannot find shipping costs for country {order.Country}");
        }

        var totalOrderCost = 0;
        foreach (var orderItem in order.items)
        {
            var shippingCost = countryCosts.FirstOrDefault(c => c.Product == orderItem.Product);
            if (shippingCost == null)
            {
                throw new Exception($"could not find shipping cost for {orderItem.Product} in {order.Country}");
            }

            var totalItemCost = 0;
            var remainingItemsCount = orderItem.quantity;
            foreach (var costRange in shippingCost.costRanges)
            {
                if (remainingItemsCount < 1)
                {
                    break;
                }

                if (costRange.MaxQuantity == null)
                {
                    totalItemCost += costRange.cost * remainingItemsCount;
                    remainingItemsCount = 0;
                }
                else
                {
                    var range = (int)costRange.MaxQuantity - costRange.MinQuantity;
                    if (costRange.MinQuantity != 0)
                    {
                        range++;
                    }

                    var numItems = Math.Min(range, remainingItemsCount);
                     var itemCost = costRange.cost * numItems;
                    totalItemCost += itemCost;
                    remainingItemsCount -= numItems;
                }
            }

            totalOrderCost += totalItemCost;
        }

        return totalOrderCost;        
    }

    /// <summary>
    /// Part 3 - Calculates total shipping costs of an order where costs are a combination of flat rates and incremental costs
    /// </summary>
    public int calculateHybridCost(Order order, HybridShippingCost allCosts)
    {
        // lookup the shipping cost for the order's country
        // store sum of total costs
        // foreach item in the order
        // lookup the cost definition for the item

        // store a sum of the item's total cost
        // store a count of remaining item quantities that need to be costed
        // foreach of the item's shipping cost ranges
        // if there are no remaining item quantities left, break
        // if the range is fixed
        // -> calculate the number of items within the range
        // -> add the range's cost once to the item's cost sum
        // -> subtract the number of items within the range from the remaining items
        // if the range's max is null
        // -> multiply the range's cost by all remaining item quantities
        // -> add the result to the item's cost sum
        // -> set the remaining item quantities to 0
        // otherwise
        // -> calculate the number of items within the range
        // -> multiply the range's cost by the number of items within the range
        // -> add the result to the item's cost sum
        // -> subtract the number of items within the range from the remaining items
        // multiply the range's cost value with the item's quantity and add it to the sum

        if (!allCosts.costsByCountry.TryGetValue(order.Country, out var countryCosts))
        {
            throw new Exception($"cannot find shipping costs for country {order.Country}");
        }

        var totalOrderCost = 0;
        foreach (var orderItem in order.items)
        {
            var shippingCost = countryCosts.FirstOrDefault(c => c.Product == orderItem.Product);
            if (shippingCost == null)
            {
                throw new Exception($"could not find shipping cost for {orderItem.Product} in {order.Country}");
            }

            var totalItemCost = 0;
            var remainingItemsCount = orderItem.quantity;
            foreach (var costRange in shippingCost.costRanges)
            {
                if (remainingItemsCount < 1)
                {
                    break;
                }

                if (costRange.RangeType == RangeType.Fixed)
                {
                    if (costRange.MaxQuantity == null)
                    {
                        totalItemCost += costRange.cost;
                        remainingItemsCount = 0;
                    }
                    else
                    {
                        var range = (int)costRange.MaxQuantity - costRange.MinQuantity;
                        if (costRange.MinQuantity != 0)
                        {
                            range++;
                        }

                        var numItems = Math.Min(range, remainingItemsCount);
                        totalItemCost += costRange.cost;
                        remainingItemsCount -= numItems;
                    }
                }
                else if (costRange.RangeType == RangeType.Incremental)
                {

                    if (costRange.MaxQuantity == null)
                    {
                        totalItemCost += costRange.cost * remainingItemsCount;
                        remainingItemsCount = 0;
                    }
                    else
                    {
                        var range = (int)costRange.MaxQuantity - costRange.MinQuantity;
                        if (costRange.MinQuantity != 0)
                        {
                            range++;
                        }

                        var numItems = Math.Min(range, remainingItemsCount);
                        var itemCost = costRange.cost * numItems;
                        totalItemCost += itemCost;
                        remainingItemsCount -= numItems;
                    }
                }
            }

            totalOrderCost += totalItemCost;
        }

        return totalOrderCost;
    }
}