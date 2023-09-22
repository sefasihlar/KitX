using AutoMapper;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;
using System.Text.Json;

namespace NLayer.Service.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        readonly IQRCodeService _qrCodeService;
        private readonly IMapper _mapper;
        public ProductService(GenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductRepository productRepository, IQRCodeService qrCodeService, IMapper mapper) : base(repository, unitOfWork)
        {
            _productRepository=productRepository;
            _qrCodeService=qrCodeService;
            _mapper=mapper;
        }

        public async Task<Product> GetAnimalWithProductId(int id)
        {
            var values = await _productRepository.GetAnimalWithProductId(id);
            return values;
        }


        public async Task<Product> GetByUserProduct(int productId)
        {

            var values = await _productRepository.GetByUserProduct(productId);
            return values;
        }

        public async Task<List<Product>> GetProductWithUserId(int id)
        {
            var values = await _productRepository.GetProductWithUserId(id);
            return values;
        }

        public async Task<byte[]> QrCodeToProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsycn(id);

            var plainObject = "https://yonetim.metaakdeniz.com/Product/Detail/"+id;

            string planText = JsonSerializer.Serialize(plainObject);

            return _qrCodeService.GenerateQrCode(planText);


        }
    }
}
