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
        private readonly IQRCodeRepository _qrCodeRepository;
        private readonly IQRGeneratorService _qrGeneratorService;



        public ProductService(GenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductRepository productRepository, IQRCodeRepository qrCodeRepository, IQRGeneratorService qrGeneratorService) : base(repository, unitOfWork)
        {
            _productRepository=productRepository;
            _qrCodeRepository=qrCodeRepository;
            _qrGeneratorService=qrGeneratorService;
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

            var plainObject = "https://kitxapp.com/Detail/"+id;

            string planText = JsonSerializer.Serialize(plainObject);

            return _qrGeneratorService.GenerateQrCode(planText);


        }
    }
}
