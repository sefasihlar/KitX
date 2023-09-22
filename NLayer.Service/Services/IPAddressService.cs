using AutoMapper;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class IPAddressService : Service<IPAddress>, IIPAddressService
    {
        private readonly IIPAddressRepository _ipAddressRepository;
        private readonly IMapper _mapper;
        public IPAddressService(GenericRepository<IPAddress> repository, IUnitOfWork unitOfWork, IIPAddressRepository ipAddressRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _ipAddressRepository=ipAddressRepository;
            _mapper=mapper;
        }

        public async Task<IPAddress> GetByIpAddressWithProductId(int productId)
        {
            var values = await _ipAddressRepository.GetByIpAddressWithProductId(productId);
            return values;
        }

        public async Task<List<IPAddress>> GetWithProductListAsync()
        {
            var values = await _ipAddressRepository.GetWithProductListAsync();
            return values;
        }

        //public async Task<List<IPAddress>> GetWithList()
        //{
        //    var values = await _iPAddressRepository.GetWithList();
        //    return  values;
        //}
    }
}
