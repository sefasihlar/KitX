using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class LocationService : Service<Location>, ILocationservicecs
    {
        private readonly ILocationRepository _locationRepository;
        public LocationService(GenericRepository<Location> repository, IUnitOfWork unitOfWork, ILocationRepository locationRepository) : base(repository, unitOfWork)
        {
            _locationRepository=locationRepository;
        }

        public async Task<List<Location>> GetUserLocations(int productId)
        {
            var values = await _locationRepository.GetUserLocations(productId);
            return values;
        }
    }
}
