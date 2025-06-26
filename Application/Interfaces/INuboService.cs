namespace Application.Interfaces
{
    public interface INuboService
    {
        public Task<string> CheckIfProductsAreInNubo(List<string> productsSkus, CancellationToken cancellationToken);
    }
}
