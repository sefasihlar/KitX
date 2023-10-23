using NLayer.Core.Concreate;

namespace NLayer.Core.Services
{
    public interface IPersonelProductFeatureService : IService<PersonProductFeature>
    {
        Task<PersonProductFeature> FindByProductIdAsync(int productId);
    }
}
