using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class BelongingProductFeatureService : Service<BelongingProductFeature>, IBelongingProductFeatureService
    {
        private readonly IBelongingProductfeatureRepository _belongingProductfeatureRepository;



        public BelongingProductFeatureService(GenericRepository<BelongingProductFeature> repository, IUnitOfWork unitOfWork, IBelongingProductfeatureRepository belongingProductfeatureRepository) : base(repository, unitOfWork)
        {
            _belongingProductfeatureRepository=belongingProductfeatureRepository;
        }

        public async Task<BelongingProductFeature> FindByProductIdAsync(int productId)
        {
            var values = await _belongingProductfeatureRepository.FindByProductIdAsync(productId);
            if (values!=null)
            {
                return values;
            }

            return values;
        }
    }
}
