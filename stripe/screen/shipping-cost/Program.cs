public class Program
{
    public static void Main(string[] args)
    {
        var solution = new Solution();

        var orderUS = new Order
        {
            Country = "US",
            items = new List<Item>
            {
                new Item { Product = "mouse", quantity = 20, },
                new Item { Product = "laptop", quantity = 5, },
            }
        };

        var orderCA = new Order
        {
            Country = "CA",
            items = new List<Item>
            {
                new Item { Product = "mouse", quantity = 20, },
                new Item { Product = "laptop", quantity = 5, },
            }
        };

        /// ===========================
        /// Part 1
        /// ===========================
        var shippingCosts = new ShippingCost
        {
            costsByCountry = new Dictionary<string, List<Cost>>
            {
                { "US",
                    new List<Cost> {
                        new Cost {Product = "mouse", cost = 550},
                        new Cost {Product = "laptop", cost = 1000},
                    }
                },
                { "CA",
                    new List<Cost> {
                        new Cost {Product = "mouse", cost = 750},
                        new Cost {Product = "laptop", cost = 1100},
                    }
                },
            }
        };

        // var cost = solution.calculateCost(orderUS, shippingCosts);
        // Console.WriteLine($"US cost: {cost}");

        // cost = solution.calculateCost(orderCA, shippingCosts);
        // Console.WriteLine($"CA cost: {cost}");

        /// ===========================
        /// Part 2
        /// ===========================
        var incrementalShippingCosts = new IncrementalShippingCost
        {
            costsByCountry = new Dictionary<string, List<IncrementalCost>>
            {
                {
                    "US",
                    new List<IncrementalCost>
                    {
                        new IncrementalCost
                        {
                            Product = "mouse",
                            costRanges = new List<CostRange>
                            {
                                new CostRange { MinQuantity = 0, MaxQuantity = null, cost = 550 },
                            }
                        },
                        new IncrementalCost
                        {
                            Product = "laptop",
                            costRanges = new List<CostRange>
                            {
                                new CostRange { MinQuantity = 0, MaxQuantity = 2, cost = 1000 },
                                new CostRange { MinQuantity = 3, MaxQuantity = null, cost = 900 },
                            }
                        }
                    }
                },
                {
                    "CA",
                    new List<IncrementalCost>
                    {
                        new IncrementalCost
                        {
                            Product = "mouse",
                            costRanges = new List<CostRange>
                            {
                                new CostRange { MinQuantity = 0, MaxQuantity = null, cost = 750 },
                            }
                        },
                        new IncrementalCost
                        {
                            Product = "laptop",
                            costRanges = new List<CostRange>
                            {
                                new CostRange { MinQuantity = 0, MaxQuantity = 2, cost = 1100 },
                                new CostRange { MinQuantity = 3, MaxQuantity = null, cost = 1000 },
                            }
                        }
                    }
                },
                {
                    "MX",
                    new List<IncrementalCost>
                    {
                        new IncrementalCost
                        {
                            Product = "laptop",
                            costRanges = new List<CostRange>
                            {
                                new CostRange { MinQuantity = 0, MaxQuantity = 2, cost = 1100 },
                                new CostRange { MinQuantity = 3, MaxQuantity = 4, cost = 1000 },
                                new CostRange { MinQuantity = 5, MaxQuantity = null, cost = 900 },
                            }
                        }
                    }
                },
            }
        };

        var orderMX = new Order
        {
            Country = "MX",
            items = new List<Item>
            {
                new Item { Product = "laptop", quantity = 6, },
            }
        };

        var cost = solution.calculateIncrementalCost(orderUS, incrementalShippingCosts);
        Console.WriteLine($"incremental US cost: {cost}");

        cost = solution.calculateIncrementalCost(orderCA, incrementalShippingCosts);
        Console.WriteLine($"incremental CA cost: {cost}");

        // answer: 6000
        cost = solution.calculateIncrementalCost(orderMX, incrementalShippingCosts);
        Console.WriteLine($"incremental MX cost: {cost}");

        /// ===========================
        /// Part 3
        /// ===========================

        var hybridShippingCosts = new HybridShippingCost
        {
            costsByCountry = new Dictionary<string, List<HybridCost>>
            {
                {
                    "US",
                    new List<HybridCost>
                    {
                        new HybridCost
                        {
                            Product = "mouse",
                            costRanges = new List<HybridCostRange>
                            {
                                new HybridCostRange {
                                    RangeType = RangeType.Incremental,
                                    MinQuantity = 0, MaxQuantity = null, cost = 550 },
                            }
                        },
                        new HybridCost
                        {
                            Product = "laptop",
                            costRanges = new List<HybridCostRange>
                            {
                                new HybridCostRange {
                                    RangeType = RangeType.Fixed,
                                    MinQuantity = 0, MaxQuantity = 2, cost = 1000 },
                                new HybridCostRange {
                                    RangeType = RangeType.Incremental,
                                    MinQuantity = 3, MaxQuantity = null, cost = 900 },
                            }
                        }
                    }
                },
                {
                    "CA",
                    new List<HybridCost>
                    {
                        new HybridCost
                        {
                            Product = "mouse",
                            costRanges = new List<HybridCostRange>
                            {
                                new HybridCostRange {
                                    RangeType = RangeType.Incremental,
                                    MinQuantity = 0, MaxQuantity = null, cost = 750 },
                            }
                        },
                        new HybridCost
                        {
                            Product = "laptop",
                            costRanges = new List<HybridCostRange>
                            {
                                new HybridCostRange {
                                    RangeType = RangeType.Fixed,
                                    MinQuantity = 0, MaxQuantity = 2, cost = 1100 },
                                new HybridCostRange {
                                    RangeType = RangeType.Incremental,
                                    MinQuantity = 3, MaxQuantity = null, cost = 1000 },
                            }
                        }
                    }
                },
                {
                    "MX",
                    new List<HybridCost>
                    {
                        new HybridCost
                        {
                            Product = "laptop",
                            costRanges = new List<HybridCostRange>
                            {
                                new HybridCostRange {
                                    RangeType = RangeType.Fixed,
                                    MinQuantity = 0, MaxQuantity = 2, cost = 1100 },
                                new HybridCostRange {
                                    RangeType = RangeType.Incremental,
                                    MinQuantity = 3, MaxQuantity = 4, cost = 1000 },
                                new HybridCostRange {
                                    RangeType = RangeType.Fixed,
                                    MinQuantity = 5, MaxQuantity = null, cost = 900 },
                            }
                        }
                    }
                },
            }
        };

        cost = solution.calculateHybridCost(orderUS, hybridShippingCosts);
        Console.WriteLine($"hybrid US cost: {cost}");

        cost = solution.calculateHybridCost(orderCA, hybridShippingCosts);
        Console.WriteLine($"hybrid CA cost: {cost}");

        // answer: 4000
        cost = solution.calculateHybridCost(orderMX, hybridShippingCosts);
        Console.WriteLine($"hybrid MX cost: {cost}");
    }
}