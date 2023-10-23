using AutoMapper;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class AnimalProductFeatureService : Service<AnimalProductFeature>, IAnimalProductFeatureService
    {
        private readonly IAnimalProductFeatureRepository _animalProductFeatureRepository;
        private readonly IMapper _animalmapper;

        public AnimalProductFeatureService(GenericRepository<AnimalProductFeature> repository, IUnitOfWork unitOfWork, IAnimalProductFeatureRepository animalProductFeatureRepository, IMapper animalmapper) : base(repository, unitOfWork)
        {
            _animalProductFeatureRepository=animalProductFeatureRepository;
            _animalmapper=animalmapper;
        }

        public async Task<AnimalProductFeature> FindByProductIdAsync(int productId)
        {
            var values = await _animalProductFeatureRepository.FindByProductIdAsync(productId);
            if (values!=null)
            {
                return values;
            }

            return values;
        }
    }
}
