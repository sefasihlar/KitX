using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface IAnimalProductFeatureRepository : GenericRepository<AnimalProductFeature>
    {
        Task<AnimalProductFeature> FindByProductIdAsync(int productId);
    }
}
