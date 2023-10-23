using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface IBelongingProductfeatureRepository : GenericRepository<BelongingProductFeature>
    {
        Task<BelongingProductFeature> FindByProductIdAsync(int productId);
    }
}
