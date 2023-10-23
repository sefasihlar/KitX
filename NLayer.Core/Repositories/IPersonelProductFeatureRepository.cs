using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface IPersonelProductFeatureRepository : GenericRepository<PersonProductFeature>
    {
        Task<PersonProductFeature> FindByProductIdAsync(int productId);
    }
}
