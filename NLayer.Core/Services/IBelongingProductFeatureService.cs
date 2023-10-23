using NLayer.Core.Concreate;

namespace NLayer.Core.Services
{
    public interface IBelongingProductFeatureService : IService<BelongingProductFeature>
    {
        Task<BelongingProductFeature> FindByProductIdAsync(int productId);
    }
}
