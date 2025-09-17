public class Order
{
    public string Country { get; set; }
    public List<Item> items { get; set; } = new List<Item>();
}