using NLayer.Core.Concreate;

namespace NLayer.Core.Services
{
    public interface IAnimalProductFeatureService : IService<AnimalProductFeature>
    {
        Task<AnimalProductFeature> FindByProductIdAsync(int productId);
    }
}
