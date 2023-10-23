using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class SpecialProductFeatureService : Service<SpecialProductFeature>, ISpecialProductFeatureService
    {
        private readonly ISpecialProductFeatureRepository _specialProductFeatureRepository;

        public SpecialProductFeatureService(GenericRepository<SpecialProductFeature> repository, IUnitOfWork unitOfWork, ISpecialProductFeatureRepository specialProductFeatureRepository) : base(repository, unitOfWork)
        {
            _specialProductFeatureRepository=specialProductFeatureRepository;
        }

        public async Task<SpecialProductFeature> FindByProductIdAsync(int productId)
        {
            var values = await _specialProductFeatureRepository.FindByProductIdAsync(productId);
            if (values!=null)
            {
                return values;
            }

            return values;
        }
    }
}
