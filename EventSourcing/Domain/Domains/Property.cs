namespace Demo.Models.Domains;

public class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Address { get; set; } = default!;
    public string OwnerName { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void Update(string address, string ownerName, decimal price)
    {
        Address = address;
        OwnerName = ownerName;
        Price = price;
    }
}
