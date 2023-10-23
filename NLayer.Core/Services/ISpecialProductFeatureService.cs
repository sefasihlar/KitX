using NLayer.Core.Concreate;

namespace NLayer.Core.Services
{
    public interface ISpecialProductFeatureService : IService<SpecialProductFeature>
    {
        Task<SpecialProductFeature> FindByProductIdAsync(int productId);
    }
}
