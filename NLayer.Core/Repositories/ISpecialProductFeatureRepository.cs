using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface ISpecialProductFeatureRepository : GenericRepository<SpecialProductFeature>
    {
        Task<SpecialProductFeature> FindByProductIdAsync(int productId);
    }
}
