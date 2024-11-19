namespace warehub.model.interfaces
{
    public interface IProduct
    {
        Guid Id { get; }
        string Name { get; }
        decimal Price { get; }
        public bool ValidateAttributesPresent();
        string ToString();
    }
}