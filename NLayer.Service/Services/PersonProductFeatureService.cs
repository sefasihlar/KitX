using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class PersonProductFeatureService : Service<PersonProductFeature>, IPersonelProductFeatureService
    {
        private readonly IPersonelProductFeatureRepository _personelProductFeatureRepository;
        public PersonProductFeatureService(GenericRepository<PersonProductFeature> repository, IUnitOfWork unitOfWork, IPersonelProductFeatureRepository personelProductFeatureRepository) : base(repository, unitOfWork)
        {
            _personelProductFeatureRepository=personelProductFeatureRepository;
        }

        public async Task<PersonProductFeature> FindByProductIdAsync(int productId)
        {
            var values = await _personelProductFeatureRepository.FindByProductIdAsync(productId);
            if (values!=null)
            {
                return values;
            }

            return values;
        }
    }
}
